//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetReceiptAddressAtIndex static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Numerics;
using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.File
{
    public static class GetReceiptAddressAtIndex
    {
        // returns: address of receipt at specified index in file's receipt array
        public static async Task<string> SendRequestAsync(Web3 web3, string fileAddress, BigInteger index)
        {
            var message = new GetReceiptAddressAtIndexFunction()
            {
                Index = index
            };

            var handler = web3.Eth.GetContractQueryHandler<GetReceiptAddressAtIndexFunction>();
            return await handler.QueryAsync<string>(fileAddress, message);
        }
    }
}
