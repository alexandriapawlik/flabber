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

using FV.Eth.Objects;

namespace FV.Eth.Functions
{
    public static class GetReceiptAddressAtIndex
    {
        // returns address of receipt at specified index in file's receipt array
        public static async Task<string> SendRequestAsync(EthInteraction eth, BigInteger index, string fileAddress)
        {
            var message = new GetReceiptAddressAtIndexFunction()
            {
                Index = index
            };

            var handler = eth.MyWeb3.Eth.GetContractQueryHandler<GetReceiptAddressAtIndexFunction>();
            return await handler.QueryAsync<string>(fileAddress, message);
        }
    }
}
