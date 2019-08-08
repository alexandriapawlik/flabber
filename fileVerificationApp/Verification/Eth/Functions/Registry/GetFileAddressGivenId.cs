//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetFileAddressFivenId static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using FV.Verification.Eth.Objects;

namespace FV.Verification.Eth.Functions
{
    public static class GetFileAddressGivenId
    {
        // returns address of file contract with matching file id
        public static async Task<string> SendRequestAsync(EthInteraction eth, string fileId)
        {
            var message = new GetFileAddressGivenIdFunction()
            {
                FileId = fileId,
            };

            var handler = eth.MyWeb3.Eth.GetContractQueryHandler<GetFileAddressGivenIdFunction>();
            return await handler.QueryAsync<string>(eth.RegistryContract.Address, message);
        }
    }
}
