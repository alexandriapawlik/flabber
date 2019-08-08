//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetFileAddressFivenId static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;

namespace Flabber.Eth.Functions.Registry
{
    public static class GetFileAddressGivenId
    {
        // returns: address of file contract with matching file id
        public static async Task<string> SendRequestAsync(Web3 web3, string registryAddress, string fileId)
        {
            var message = new GetFileAddressGivenIdFunction()
            {
                FileId = fileId,
            };

            var handler = web3.Eth.GetContractQueryHandler<GetFileAddressGivenIdFunction>();
            return await handler.QueryAsync<string>(registryAddress, message);
        }
    }
}
