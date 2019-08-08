//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DeployFile static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using Nethereum.JsonRpc.Client;
using Nethereum.Web3;

namespace Flabber.Eth.Functions.File
{
    public static class DeployFile
    {
        public static async Task SendRequestAsync(Web3 web3, string registryAddress, string name, string fileId,
            string fileHash, string metadataHash, string type, string etag)
        {

            var message = new DeployFileFunction
            {
                RegistryAddress = registryAddress,
                Name = name,
                FileId = fileId,
                FileHash = fileHash,
                MetadataHash = metadataHash,
                Type = type,
                Etag = etag
            };

            try
            {
                var handler = web3.Eth.GetContractDeploymentHandler<DeployFileFunction>();
                var transactionReceipt = await handler.SendRequestAndWaitForReceiptAsync(message);
            }
            catch (RpcResponseException ex)
            {
                throw new Exception("The RPC request failed due to an RPC error or a smart contract code error", ex);
            }
        }
    }
}
