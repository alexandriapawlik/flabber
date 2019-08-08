//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetRegistryAddressAtIndex static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

using Flabber.Infrastructure;

namespace Flabber.Eth.Functions.Library
{
    public static class GetRegistryAddressAtIndex
    {
        // returns: address of registry at specified index in library's registry array
        public static async Task<string> SendRequestAsync(Web3 web3, int index)
        {
            var message = new GetRegistryAddressAtIndexFunction()
            {
                Index = index
            };

            var handler = web3.Eth.GetContractQueryHandler<GetRegistryAddressAtIndexFunction>();
            return await handler.QueryAsync<string>(Constants.LibraryAddress, message);
        }
    }
}
