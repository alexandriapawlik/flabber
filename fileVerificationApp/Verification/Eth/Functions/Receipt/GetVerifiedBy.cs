//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetVerificationDateTime static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using FV.Verification.Eth.Objects;

namespace FV.Verification.Eth.Functions
{
    public static class GetVerifiedBy
    {
        // returns verification date and time string for receipt
        public static async Task<string> SendRequestAsync(EthInteraction eth, string receiptAddress)
        {
            var message = new GetVerifiedByFunction()
            { };

            var handler = eth.MyWeb3.Eth.GetContractQueryHandler<GetVerifiedByFunction>();
            return await handler.QueryAsync<string>(receiptAddress, message);
        }
    }
}
