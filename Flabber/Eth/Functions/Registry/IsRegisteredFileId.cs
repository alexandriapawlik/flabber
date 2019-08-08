//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		IsRegisteredFileId static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.Registry
{
    public static class IsRegisteredFileId
    {
        // returns: whether or not a file is already in registry
        public static async Task<bool> SendRequestAsync(Web3 web3, string registryAddress, string fileId)
        {
            var message = new IsRegisteredFileIdFunction()
            {
                FileId = fileId,
            };

            var handler = web3.Eth.GetContractQueryHandler<IsRegisteredFileIdFunction>();
            return await handler.QueryAsync<bool>(registryAddress, message);
        }
    }
}

