//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetState static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.Receipt
{
    public static class GetState
    {
        // returns: verification state of the receipt
        public static async Task<int> SendRequestAsync(Web3 web3, string receiptAddress)
        {
            var message = new GetStateFunction()
            { };

            var handler = web3.Eth.GetContractQueryHandler<GetStateFunction>();
            return await handler.QueryAsync<int>(receiptAddress, message);
        }
    }
}
