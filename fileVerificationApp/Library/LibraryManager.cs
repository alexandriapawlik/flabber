//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		LibraryManager class
// Purpose:		Accesses methods of Library smart contract

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;
using Nethereum.Web3.Accounts;

using FV.Infrastructure;
using FV.Library.Functions;
using FV.Library.Objects;

namespace FV.Library
{
    public class LibraryManager
    {
        public Web3 MyWeb3 { get; set; }
        public string LibraryAddress = Constants.Library;

        public LibraryManager()
        {
            // open admin account to use for transactions
            var privateKey = Constants.Key;
            Account MyAccount = new Account(privateKey);

            // URL for RPC endpoint of Ethereum POA blockchain
            string url = Constants.BlockchainURL;

            // create connection
            MyWeb3 = new Web3(MyAccount, url);
        }

        // create a new file registry
        // returns the address of the deployed contract
        public async Task<string> NewRegistry(string name, string description)
        {
            // deploy new file registry smart contract to blockchain
            return await DeployRegistry.SendRequestAsync(this, name, description);
        }

        // gets registry names and addresses
        public async Task<RegistryList> GetList()
        {
            RegistryList list = new RegistryList();

            // call number of registries function on library contract
            int count = await GetNumberOfRegistries.SendRequestAsync(this);

            // for each receipt
            for (int i = 0; i < count; ++i)
            {
                // get registry address at index
                string address = await GetRegistryAddressAtIndex.SendRequestAsync(this, i);

                // get registry name at index
                string name = await GetRegistryNameAtIndex.SendRequestAsync(this, i);

                // add values to RegistryList object
                list.AddLine(name, address);
            }

            return list;
        }
    }
}