//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetNumberOfReceipts static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.File
{
    public static class GetNumberOfReceipts
    {
        // returns: number of receipts existing for a file
        public static async Task<int> SendRequestAsync(Web3 web3, string fileAddress)
        {
            var message = new GetNumberOfReceiptsFunction()
            { };

            var handler = web3.Eth.GetContractQueryHandler<GetNumberOfReceiptsFunction>();
            return await handler.QueryAsync<int>(fileAddress, message);
        }
    }
}
