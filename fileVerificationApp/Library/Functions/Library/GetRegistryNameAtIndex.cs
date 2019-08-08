//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetRegistryNameAtIndex static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

namespace FV.Library.Functions
{
    public static class GetRegistryNameAtIndex
    {
        // returns name of registry at specified index in library's registry array
        public static async Task<string> SendRequestAsync(LibraryManager library, int index)
        {
            var message = new GetRegistryNameAtIndexFunction()
            {
                Index = index
            };

            var handler = library.MyWeb3.Eth.GetContractQueryHandler<GetRegistryNameAtIndexFunction>();
            return await handler.QueryAsync<string>(library.LibraryAddress, message);
        }
    }
}
