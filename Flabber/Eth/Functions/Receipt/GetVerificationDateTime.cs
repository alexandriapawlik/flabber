//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetVerificationDateTime static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.Receipt
{
    public static class GetVerificationDateTime
    {
        // returns: verification date and time string for receipt
        public static async Task<string> SendRequestAsync(Web3 web3, string receiptAddress)
        {
            var message = new GetVerificationDateTimeFunction()
            { };

            var handler = web3.Eth.GetContractQueryHandler<GetVerificationDateTimeFunction>();
            return await handler.QueryAsync<string>(receiptAddress, message);
        }
    }
}
