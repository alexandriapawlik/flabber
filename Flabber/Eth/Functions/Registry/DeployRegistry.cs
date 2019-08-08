//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DeployRegistry static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

using Flabber.Infrastructure;

namespace Flabber.Eth.Functions.Registry
{
    public static class DeployRegistry
    {
        // returns: address of deployed registry contract
        public static async Task<string> SendRequestAsync(Web3 web3, string name, string description)
        {
            var message = new DeployRegistryFunction
            {
                LibraryAddress = Constants.LibraryAddress,
                Name = name,
                Description = description
            };

            var handler = web3.Eth.GetContractDeploymentHandler<DeployRegistryFunction>();
            var transactionReceipt = await handler.SendRequestAndWaitForReceiptAsync(message);
            return transactionReceipt.ContractAddress;
        }
    }
}
