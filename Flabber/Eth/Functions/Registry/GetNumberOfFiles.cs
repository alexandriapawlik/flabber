//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetNumberOfFiles static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.Registry
{
    public static class GetNumberOfFiles
    {
        // returns: number of files in the registry
        public static async Task<int> SendRequestAsync(Web3 web3, string registryAddress)
        {
            var message = new GetNumberOfFilesFunction()
            { };

            var handler = web3.Eth.GetContractQueryHandler<GetNumberOfFilesFunction>();
            return await handler.QueryAsync<int>(registryAddress, message);
        }
    }
}
