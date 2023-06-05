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

    public static class EthHelper
    {
        public static UInt160 BuildMapAddress(this string ethAddress, uint lockindex = 0)
        {
            return new EthereumMapTransaction { EthereumAddress = ethAddress, LockExpirationIndex = lockindex }.GetContract().ScriptHash;
        }
        public static bool IsOnlyFromEthereumMapAddress(this Transaction tx, out string ethAddress)
        {
            ethAddress = string.Empty;
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
                        {
                            if (witness.ScriptHash == Contract.CreateSignatureRedeemScript(ECPoint.DecodePoint(engine.pubkey, ECCurve.Secp256r1)).ToScriptHash())
                            {
                                return false;
                            }
                        }
                    }
                }
                catch { }
            }
            if (tx.Attributes.IsNotNullAndEmpty())
            {
                var attrs = tx.Attributes.Where(p => p.Usage == TransactionAttributeUsage.EthSignature);
                if (attrs.IsNotNullAndEmpty())
                {
                    var message = tx.InputOutputHash.ToArray().ToHexString();
                    var signer = new Nethereum.Signer.EthereumMessageSigner();
                    foreach (var attr in attrs)
                    {
                        try
                        {
                            var signature = Encoding.UTF8.GetString(attr.Data);
                            var address = signer.EncodeUTF8AndEcRecover(message, signature);
                            if (address.IsNotNullAndEmpty())
                            {
                                ethAddress = address;
                                return true;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            return false;
        }
    }
}
