//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetState static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using FV.Verification.Eth.Objects;

namespace FV.Verification.Eth.Functions
{
    public static class GetState
    {
        // returns verification state of the receipt
        public static async Task<int> SendRequestAsync(EthInteraction eth, string receiptAddress)
        {
            var message = new GetStateFunction()
            { };

            var handler = eth.MyWeb3.Eth.GetContractQueryHandler<GetStateFunction>();
            return await handler.QueryAsync<int>(receiptAddress, message);
        }
    }
}
