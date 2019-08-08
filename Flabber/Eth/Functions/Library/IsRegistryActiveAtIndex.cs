//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		IsRegistryActiveAtIndex static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

using Flabber.Infrastructure;

namespace Flabber.Eth.Functions.Library
{
    public static class IsRegistryActiveAtIndex
    {
        // returns: true if registry is active/open
        public static async Task<bool> SendRequestAsync(Web3 web3, int index)
        {
            var message = new IsRegistryActiveAtIndexFunction()
            {
                Index = index
            };

            var handler = web3.Eth.GetContractQueryHandler<IsRegistryActiveAtIndexFunction>();
            return await handler.QueryAsync<bool>(Constants.LibraryAddress, message);
        }
    }
}
