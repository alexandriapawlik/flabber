//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		IsRegisteredFileId static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using FV.Verification.Eth.Objects;

namespace FV.Verification.Eth.Functions
{
    public static class IsRegisteredFileId
    {
        // returns whether or not a file is already in registry
        public static async Task<bool> SendRequestAsync(EthInteraction eth, string fileId)
        {
            var message = new IsRegisteredFileIdFunction()
            {
                FileId = fileId,
            };

            var handler = eth.MyWeb3.Eth.GetContractQueryHandler<IsRegisteredFileIdFunction>();
            return await handler.QueryAsync<bool>(eth.RegistryContract.Address, message);
        }
    }
}

