//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetNumberOfRegistries static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using Nethereum.Web3;

using Flabber.Infrastructure;

namespace Flabber.Eth.Functions.Library
{
    public static class GetNumberOfRegistries
    {
        // returns: number of registries in the library
        public static async Task<int> SendRequestAsync(Web3 web3)
        {
            var message = new GetNumberOfRegistriesFunction()
            { };

            var handler = web3.Eth.GetContractQueryHandler<GetNumberOfRegistriesFunction>();
            return await handler.QueryAsync<int>(Constants.LibraryAddress, message);
        }
    }
}
