//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		GetNumberOfFiles static class
// Purpose:		Calls corresponding smart contract function
//              on specified contract using Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

namespace FV.Eth.Functions
{
    public static class GetNumberOfFiles
    {
        // returns number of files in the registry
        public static async Task<int> SendRequestAsync(EthInteraction eth)
        {
            var message = new GetNumberOfFilesFunction()
            { };

            var handler = eth.MyWeb3.Eth.GetContractQueryHandler<GetNumberOfFilesFunction>();
            return await handler.QueryAsync<int>(eth.RegistryContract.Address, message);
        }
    }
}
