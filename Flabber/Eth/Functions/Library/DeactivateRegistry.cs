//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DeactivateRegistry static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using Nethereum.JsonRpc.Client;
using Nethereum.Web3;

using Flabber.Infrastructure;

namespace Flabber.Eth.Functions.Library
{
    public static class DeactivateRegistry
    {
        public static async Task SendRequestAsync(Web3 web3, string registryAddress)
        {
            var message = new DeactivateRegistryFunction
            {
                RegistryAddress = registryAddress
            };

            try
            {
                var handler = web3.Eth.GetContractTransactionHandler<DeactivateRegistryFunction>();
                var transactionReceipt = await handler.SendRequestAndWaitForReceiptAsync(Constants.LibraryAddress, message);
            }
            catch (RpcResponseException ex)
            {
                throw new Exception("The RPC request failed due to an RPC error or a smart contract code error", ex);
            }
        }
    }
}
