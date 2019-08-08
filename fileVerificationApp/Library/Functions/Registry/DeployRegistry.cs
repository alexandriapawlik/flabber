//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DeployRegistry static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

namespace FV.Library.Functions
{
    public static class DeployRegistry
    {
        // returns address of deployed registry contract
        public static async Task<string> SendRequestAsync(LibraryManager library, string name, string description)
        {
            var message = new DeployRegistryFunction
            {
                LibraryAddress = library.LibraryAddress,
                Name = name,
                Description = description
            };

            var handler = library.MyWeb3.Eth.GetContractDeploymentHandler<DeployRegistryFunction>();
            var transactionReceipt = await handler.SendRequestAndWaitForReceiptAsync(message);
            return transactionReceipt.ContractAddress;
        }
    }
}
