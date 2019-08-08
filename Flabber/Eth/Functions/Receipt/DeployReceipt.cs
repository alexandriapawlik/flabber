//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DeployReceipt static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.Receipt
{
    public static class DeployReceipt
    {
        // returns: address of deployed receipt contract
        public static async Task<string> SendRequestAsync(Web3 web3, string registryAddress,
            string fileId, string fileHash, string metadataHash, string verifiedBy)
        {
            var message = new DeployReceiptFunction
            {
                RegistryAddress = registryAddress,
                FileId = fileId,
                FileHash = fileHash,
                MetadataHash = metadataHash,
                VerifiedBy = verifiedBy
            };

            var handler = web3.Eth.GetContractDeploymentHandler<DeployReceiptFunction>();
            var transactionReceipt = await handler.SendRequestAndWaitForReceiptAsync(message);
            return transactionReceipt.ContractAddress;
        }
    }
}
