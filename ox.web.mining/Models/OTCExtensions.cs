using System;
using System.Numerics;
using System.Threading.Tasks;
using OX;
using OX.Wallets;
using System.Reflection.Metadata.Ecma335;
using OX.Cryptography.AES;
using OX.Network.P2P.Payloads;
using OX.Mining.OTC;
using OX.Mining;
using OX.IO;

namespace OX.Web.Models
{
    public static class OTCExtensions
    {
        public static bool DoDeposit(this INotecase notecase, WalletAccount account, string fromEthAddress, string toEthPoolAddress, UInt160 oxReceiptAddress, string ethTxId, string stringToSign, string signatureData, bool agenFee = false)
        {
            var key = account.GetKey();
            EthDepositInfo depositInfo = new EthDepositInfo
            {
                EthTxId = ethTxId,
                EthAddress = fromEthAddress.ToLower(),
                UnSignedData = stringToSign,
                SignedData = signatureData
            };
            var customersharekey = key.DiffieHellman(invest.OTCAccountPubKey);
            var encryptData = depositInfo.ToArray().Encrypt(customersharekey);
            OTCDepositRequest request = new OTCDepositRequest
            {
                EthTxInfo_Encrypt = encryptData,
                EthPoolAddress = toEthPoolAddress,
                ReceiptAddress = oxReceiptAddress,
                AgentFee = agenFee
            };
            SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(account);
            var tx = notecase.Wallet.MakeSingleAskTransaction(stw, invest.MasterAccountAddress, (byte)InvestType.OTCDepositRequest, request);
            if (tx.IsNotNull())
            {
                return notecase.SignAndSendTx(tx);
            }
            return false;
        }
    }
}