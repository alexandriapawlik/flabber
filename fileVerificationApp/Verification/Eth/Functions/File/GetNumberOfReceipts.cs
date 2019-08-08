//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetNumberOfReceipts static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using FV.Verification.Eth.Objects;

namespace FV.Verification.Eth.Functions
{
    public static class GetNumberOfReceipts
    {
        // returns number of receipts existing for a file
        public static async Task<int> SendRequestAsync(EthInteraction eth, string fileAddress)
        {
            var message = new GetNumberOfReceiptsFunction()
            { };

            var handler = eth.MyWeb3.Eth.GetContractQueryHandler<GetNumberOfReceiptsFunction>();
            return await handler.QueryAsync<int>(fileAddress, message);
        }
    }
}
