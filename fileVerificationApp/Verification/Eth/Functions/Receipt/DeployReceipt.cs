//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DeployReceipt static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

namespace FV.Verification.Eth.Functions
{
    public static class DeployReceipt
    {
        // returns address of deployed receipt contract
        public static async Task<string> SendRequestAsync(EthInteraction eth, string fileId, string fileHash, string metadataHash, string verifiedBy)
        {
            var message = new DeployReceiptFunction
            {
                RegistryAddress = eth.RegistryContract.Address,
                FileId = fileId,
                FileHash = fileHash,
                MetadataHash = metadataHash,
                VerifiedBy = verifiedBy
            };

            var handler = eth.MyWeb3.Eth.GetContractDeploymentHandler<DeployReceiptFunction>();
            var transactionReceipt = await handler.SendRequestAndWaitForReceiptAsync(message);
            return transactionReceipt.ContractAddress;
        }
    }
}
