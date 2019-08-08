//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetNumberOfRegistries static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

namespace FV.Library.Functions
{
    public static class GetNumberOfRegistries
    {
        // returns number of registries in the library
        public static async Task<int> SendRequestAsync(LibraryManager library)
        {
            var message = new GetNumberOfRegistriesFunction()
            { };

            var handler = library.MyWeb3.Eth.GetContractQueryHandler<GetNumberOfRegistriesFunction>();
            return await handler.QueryAsync<int>(library.LibraryAddress, message);
        }
    }
}
