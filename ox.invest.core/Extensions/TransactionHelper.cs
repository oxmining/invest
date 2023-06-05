using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.VM.Types;
using OX.VM;
using VMArray = OX.VM.Types.Array;
using OX.Cryptography.ECC;
using OX.SmartContract;
using System.Runtime.CompilerServices;

namespace OX.Mining
{
    public class CenterExecutionContext
    {
        /// <summary>
        /// Number of items to be returned
        /// </summary>
        public int RVCount { get; }

        /// <summary>
        /// Script
        /// </summary>
        public Script Script { get; }

        /// <summary>
        /// Evaluation stack
        /// </summary>
        public RandomAccessStack<StackItem> EvaluationStack { get; } = new RandomAccessStack<StackItem>();

        /// <summary>
        /// Alternative stack
        /// </summary>
        public RandomAccessStack<StackItem> AltStack { get; } = new RandomAccessStack<StackItem>();

        /// <summary>
        /// Instruction pointer
        /// </summary>
        public int InstructionPointer { get; set; }

        public Instruction CurrentInstruction
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GetInstruction(InstructionPointer);
            }
        }

        /// <summary>
        /// Next instruction
        /// </summary>
        public Instruction NextInstruction
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GetInstruction(InstructionPointer + CurrentInstruction.Size);
            }
        }

        /// <summary>
        /// Cached script hash
        /// </summary>
        public byte[] ScriptHash
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Script.ScriptHash;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="script">Script</param>
        /// <param name="rvcount">Number of items to be returned</param>
        internal CenterExecutionContext(Script script, int rvcount)
        {
            this.RVCount = rvcount;
            this.Script = script;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Instruction GetInstruction(int ip) => Script.GetInstruction(ip);

        internal bool MoveNext()
        {
            InstructionPointer += CurrentInstruction.Size;
            return InstructionPointer < Script.Length;
        }
    }
    public class CenterExecutionEngine : IDisposable
    {
        #region Limits Variables



        /// <summary>
        /// Set the max Stack Size
        /// </summary>
        public virtual uint MaxStackSize => 2 * 1024;

        /// <summary>
        /// Set Max Item Size
        /// </summary>
        public virtual uint MaxItemSize => 1024 * 1024;



        #endregion
        private static readonly byte[] EmptyBytes = new byte[0];

        private int stackitem_count = 0;
        private bool is_stackitem_count_strict = true;
        public IScriptContainer ScriptContainer { get; }
        public ICrypto Crypto { get; } = Cryptography.Crypto.Default;
        public VMState State { get; internal protected set; } = VMState.BREAK;
        public RandomAccessStack<CenterExecutionContext> InvocationStack { get; } = new RandomAccessStack<CenterExecutionContext>();
        public RandomAccessStack<StackItem> ResultStack { get; } = new RandomAccessStack<StackItem>();
        public CenterExecutionContext CurrentContext => InvocationStack.Peek();
        public byte[] pubkey { get; private set; }
        public byte[] signature { get; private set; }
        public CenterExecutionEngine(IScriptContainer container)
        {
            this.ScriptContainer = container;
        }

        #region Limits



        /// <summary>
        /// Check if the is possible to overflow the MaxItemSize
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Return True if are allowed, otherwise False</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckMaxItemSize(int length) => length >= 0 && length <= MaxItemSize;



        /// <summary>
        /// Check if the is possible to overflow the MaxStackSize
        /// </summary>
        /// <param name="count">Stack item count</param>
        /// <param name="strict">Is stack count strict?</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckStackSize(bool strict, int count = 1)
        {
            is_stackitem_count_strict &= strict;
            stackitem_count += count;

            if (stackitem_count < 0) stackitem_count = int.MaxValue;
            if (stackitem_count <= MaxStackSize) return true;
            if (is_stackitem_count_strict) return false;

            // Deep inspect

            stackitem_count = GetItemCount(InvocationStack.SelectMany(p => p.EvaluationStack.Concat(p.AltStack)));
            if (stackitem_count > MaxStackSize) return false;
            is_stackitem_count_strict = true;

            return true;
        }

        /// <summary>
        /// Get item count
        /// </summary>
        /// <param name="items">Items</param>
        /// <returns>Return the number of items</returns>
        private static int GetItemCount(IEnumerable<StackItem> items)
        {
            Queue<StackItem> queue = new Queue<StackItem>(items);
            List<StackItem> counted = new List<StackItem>();
            int count = 0;
            while (queue.Count > 0)
            {
                StackItem item = queue.Dequeue();
                count++;
                switch (item)
                {
                    case OX.VM.Types.Array array:
                        {
                            if (counted.Any(p => ReferenceEquals(p, array)))
                                continue;
                            counted.Add(array);
                            foreach (StackItem subitem in array)
                                queue.Enqueue(subitem);
                            break;
                        }
                    case Map map:
                        {
                            if (counted.Any(p => ReferenceEquals(p, map)))
                                continue;
                            counted.Add(map);
                            foreach (StackItem subitem in map.Values)
                                queue.Enqueue(subitem);
                            break;
                        }
                }
            }
            return count;
        }

        #endregion

        public virtual void Dispose()
        {
            InvocationStack.Clear();
        }



        public CenterExecutionContext LoadScript(byte[] script, int rvcount = -1)
        {
            return LoadScript(new Script(Crypto, script), rvcount);
        }

        protected virtual CenterExecutionContext LoadScript(Script script, int rvcount = -1)
        {
            CenterExecutionContext context = new CenterExecutionContext(script, rvcount);
            InvocationStack.Push(context);
            return context;
        }
        public void Execute()
        {
            State &= ~VMState.BREAK;
            while (!State.HasFlag(VMState.HALT) && !State.HasFlag(VMState.FAULT) && !State.HasFlag(VMState.BREAK))
                ExecuteNext();
        }
        protected virtual bool PostExecuteInstruction(Instruction instruction)
        {
            return true;
        }

        protected virtual bool PreExecuteInstruction()
        {
            return true;
        }
        internal protected void ExecuteNext()
        {
            if (InvocationStack.Count == 0)
            {
                State = VMState.HALT;
            }
            else
            {
                try
                {
                    Instruction instruction = CurrentContext.CurrentInstruction;
                    if (!PreExecuteInstruction() || !ExecuteInstruction() || !PostExecuteInstruction(instruction))
                        State = VMState.FAULT;
                }
                catch
                {
                    State = VMState.FAULT;
                }
            }
        }
        private bool ExecuteInstruction()
        {
            CenterExecutionContext context = CurrentContext;
            Instruction instruction = context.CurrentInstruction;
            if (instruction.OpCode >= OpCode.PUSHBYTES1 && instruction.OpCode <= OpCode.PUSHDATA4)
            {
                if (!CheckMaxItemSize(instruction.Operand.Length)) return false;
                context.EvaluationStack.Push(instruction.Operand);
                if (!CheckStackSize(true))
                    return false;
            }
            else switch (instruction.OpCode)
                {

                    case OpCode.RET:
                        {
                            CenterExecutionContext context_pop = InvocationStack.Pop();
                            int rvcount = context_pop.RVCount;
                            if (rvcount == -1) rvcount = context_pop.EvaluationStack.Count;
                            if (rvcount > 0)
                            {
                                if (context_pop.EvaluationStack.Count < rvcount)
                                    return false;
                                RandomAccessStack<StackItem> stack_eval;
                                if (InvocationStack.Count == 0)
                                    stack_eval = ResultStack;
                                else
                                    stack_eval = CurrentContext.EvaluationStack;
                                context_pop.EvaluationStack.CopyTo(stack_eval, rvcount);
                            }
                            if (context_pop.RVCount == -1 && InvocationStack.Count > 0)
                            {
                                context_pop.AltStack.CopyTo(CurrentContext.AltStack);
                            }
                            CheckStackSize(false, 0);
                            if (InvocationStack.Count == 0)
                            {
                                State = VMState.HALT;
                            }
                            return true;
                        }

                    case OpCode.CHECKSIG:
                        {
                            pubkey = context.EvaluationStack.Pop().GetByteArray();
                            signature = context.EvaluationStack.Pop().GetByteArray();

                            try
                            {
                                context.EvaluationStack.Push(Crypto.VerifySignature(ScriptContainer.GetMessage(), signature, pubkey));
                            }
                            catch (ArgumentException)
                            {
                                context.EvaluationStack.Push(false);
                            }
                            CheckStackSize(true, -1);
                            break;
                        }
                    case OpCode.VERIFY:
                        {
                            pubkey = context.EvaluationStack.Pop().GetByteArray();
                            signature = context.EvaluationStack.Pop().GetByteArray();
                            byte[] message = context.EvaluationStack.Pop().GetByteArray();

                            try
                            {
                                context.EvaluationStack.Push(Crypto.VerifySignature(message, signature, pubkey));
                            }
                            catch (ArgumentException)
                            {
                                context.EvaluationStack.Push(false);
                            }
                            CheckStackSize(true, -2);
                            break;
                        }
                    case OpCode.CHECKMULTISIG:
                        {
                            int n;
                            byte[][] pubkeys;
                            StackItem item = context.EvaluationStack.Pop();

                            if (item is VMArray array1)
                            {
                                pubkeys = array1.Select(p => p.GetByteArray()).ToArray();
                                n = pubkeys.Length;
                                if (n == 0) return false;
                                CheckStackSize(false, -1);
                            }
                            else
                            {
                                n = (int)item.GetBigInteger();
                                if (n < 1 || n > context.EvaluationStack.Count)
                                    return false;
                                pubkeys = new byte[n][];
                                for (int i = 0; i < n; i++)
                                    pubkeys[i] = context.EvaluationStack.Pop().GetByteArray();
                                CheckStackSize(true, -n - 1);
                            }

                            int m;
                            byte[][] signatures;
                            item = context.EvaluationStack.Pop();
                            if (item is VMArray array2)
                            {
                                signatures = array2.Select(p => p.GetByteArray()).ToArray();
                                m = signatures.Length;
                                if (m == 0 || m > n)
                                    return false;
                                CheckStackSize(false, -1);
                            }
                            else
                            {
                                m = (int)item.GetBigInteger();
                                if (m < 1 || m > n || m > context.EvaluationStack.Count)
                                    return false;
                                signatures = new byte[m][];
                                for (int i = 0; i < m; i++)
                                    signatures[i] = context.EvaluationStack.Pop().GetByteArray();
                                CheckStackSize(true, -m - 1);
                            }
                            byte[] message = ScriptContainer.GetMessage();
                            bool fSuccess = true;
                            try
                            {
                                for (int i = 0, j = 0; fSuccess && i < m && j < n;)
                                {
                                    if (Crypto.VerifySignature(message, signatures[i], pubkeys[j]))
                                        i++;
                                    j++;
                                    if (m - i > n - j)
                                        fSuccess = false;
                                }
                            }
                            catch (ArgumentException)
                            {
                                fSuccess = false;
                            }
                            context.EvaluationStack.Push(fSuccess);
                            break;
                        }


                    default:
                        return false;
                }
            context.MoveNext();
            return true;
        }
    }
    public static class TransactionHelper
    {

        public static ECPoint[] GetPublicKeys(this Transaction tx)
        {
            List<ECPoint> list = new List<ECPoint>();
            foreach (var witness in tx.Witnesses)
            {
                try
                {
                    using (CenterExecutionEngine engine = new CenterExecutionEngine(tx))
                    {
                        engine.LoadScript(witness.VerificationScript);
                        engine.LoadScript(witness.InvocationScript);
                        engine.Execute();
                        if (engine.pubkey.IsNotNullAndEmpty())
                            list.Add(ECPoint.DecodePoint(engine.pubkey, ECCurve.Secp256r1));
                    }
                }
                catch { }
            }
            return list.ToArray();
        }
       
        public static ECPoint GetBestWitnessPublicKey(this Transaction tx)
        {
            var pubkeys = tx.GetPublicKeys();
            if (pubkeys.IsNullOrEmpty()) return default;
            var pubkey = pubkeys.FirstOrDefault(p => tx.Outputs.Select(m => m.ScriptHash).Contains(Contract.CreateSignatureRedeemScript(p).ToScriptHash()));
            if (pubkey.IsNull())
            {
                pubkey = pubkeys.FirstOrDefault();
            }
            return pubkey;
        }
    }
}
