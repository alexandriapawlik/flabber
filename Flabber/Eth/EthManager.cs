//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		EthManager class
// Purpose:		Initiates all interactions with private Ethereum blockchain
//              Uses function static classes to make calls to Web3 API

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using Nethereum.Web3;
using Nethereum.Web3.Accounts;

using Flabber.Infrastructure;
using Flabber.Eth.Functions.Library;
using Flabber.Eth.Functions.Registry;
using Flabber.Eth.Functions.File;
using Flabber.Eth.Functions.Receipt;
using Flabber.Objects;

namespace Flabber.Eth
{
    public class EthManager
    {
        public Web3 MyWeb3 { get; set; }

        private readonly Account MyAccount;


        ////////////////////////////////////////////////////////


        public EthManager()
        {
            // open admin account to use for transactions
            var privateKey = Constants.Key;
            MyAccount = new Account(privateKey);

            // create connection to network
            MyWeb3 = new Web3(MyAccount, Constants.BlockchainURL);
        }


        ////////////////////////////////////////////////////////


        // create a new file registry
        // returns the address of the deployed contract
        public async Task<string> NewRegistry(string name, string description)
        {
            // deploy new file registry smart contract to blockchain
            return await DeployRegistry.SendRequestAsync(MyWeb3, name, description);
        }

        // mark a file registry as inactive
        public async Task RemoveRegistry(string registryAddress)
        {
            await DeactivateRegistry.SendRequestAsync(MyWeb3, registryAddress);
        }

        // adds new file contract for file with provided data
        public async Task NewFile(string registryAddress, File file, string fileHash, string metadataHash)
        {
            // check if it worked
            if (await NewFilePrivate(registryAddress, file, fileHash, metadataHash))
            {
                return;
            }
            else
            {
                // check if it worked second time
                if (await NewFilePrivate(registryAddress, file, fileHash, metadataHash))
                {
                    return;
                }
                else
                {
                    throw new Exception("File could not be registered");
                }
            } 
        }

        // adds new receipt contract for file with fileid and provided hashes
        // returns state of new receipt
        public async Task<string> NewReceipt(string registryAddress, string fileId, string fileHash,
            string metadataHash, string verifiedBy)
        {
            int state = await NewReceiptPrivate(registryAddress, fileId, fileHash,
                metadataHash, verifiedBy);

            // check if it worked
            if (state != 0)
            {
                return (StateConvert.IntToString(state));
            }
            else
            {
                state = await NewReceiptPrivate(registryAddress, fileId, fileHash,
                metadataHash, verifiedBy);

                // check if it worked second time
                if (state != 0)
                {
                    return (StateConvert.IntToString(state));
                }
                else
                {
                    throw new Exception("Receipt could not be created");
                }
            }
        }

        // gets receipt datetimes, states, and verifiers for file with fileid
        public async Task<History> GetHistory(string registryAddress, string fileId)
        {
            History history = new History();

            // get address of file contract with corresponding fileid
            string fileAddress = await GetFileAddressGivenId.SendRequestAsync(MyWeb3, registryAddress, fileId);

            // call number of receipts function on file contract
            int numReceipts = await GetNumberOfReceipts.SendRequestAsync(MyWeb3, fileAddress);

            // for each receipt
            for (int i = 0; i < numReceipts; ++i)
            {
                // get receipt address at index
                string receiptAddress = await GetReceiptAddressAtIndex.SendRequestAsync(MyWeb3, fileAddress, i);

                // get datetime
                string dateTime = await GetVerificationDateTime.SendRequestAsync(MyWeb3, receiptAddress);

                // get state of receipt
                string state = StateConvert.IntToString(await GetState.SendRequestAsync(MyWeb3, receiptAddress));

                // get user that created receipt
                string verifiedBy = await GetVerifiedBy.SendRequestAsync(MyWeb3, receiptAddress);

                // add values to History object
                history.AddLine(dateTime, state, verifiedBy);
            }

            return history;
        }

        // gets active registry names and addresses
        public async Task<RegistryList> GetRegistryList()
        {
            RegistryList list = new RegistryList();

            // call number of registries function on library contract
            int count = await GetNumberOfRegistries.SendRequestAsync(MyWeb3);

            // for each receipt
            for (int i = 0; i < count; ++i)
            {
                // check if registry is active
                if (await IsRegistryActiveAtIndex.SendRequestAsync(MyWeb3, i))
                {
                    // get registry address at index
                    string address = await GetRegistryAddressAtIndex.SendRequestAsync(MyWeb3, i);

                    // get registry name at index
                    string name = await GetRegistryNameAtIndex.SendRequestAsync(MyWeb3, i);

                    // add values to RegistryList object
                    list.AddLine(name, address);
                }
            }

            return list;
        }

        // filter file list down to only unregistered files
        public async Task<FileList> OnlyUnregistered(string registryAddress, FileList list)
        {
            FileList filtered = new FileList();

            // for each file
            for (int i = 0; i < list.NumFiles; ++i)
            {
                // check if the file has been registered
                bool registered = await IsRegisteredFileId.SendRequestAsync(MyWeb3, registryAddress,
                    list.Files[i].FileId);

                // if it hasn't been registered, add to new list
                if (!registered)
                {
                    filtered.AddLine(list.Files[i].Name, list.Files[i].FileId, list.Files[i].Type,
                        list.Files[i].Etag, list.Files[i].Size);
                }
            }

            return filtered;
        }

        // filter file list down to only unregistered files
        public async Task<FileList> OnlyRegistered(string registryAddress, FileList list)
        {
            FileList filtered = new FileList();

            // for each file
            for (int i = 0; i < list.NumFiles; ++i)
            {
                // check if the file has been registered
                bool registered = await IsRegisteredFileId.SendRequestAsync(MyWeb3, registryAddress,
                    list.Files[i].FileId);

                // if it has been registered, add to new list
                if (registered)
                {
                    filtered.AddLine(list.Files[i].Name, list.Files[i].FileId, list.Files[i].Type,
                        list.Files[i].Etag, list.Files[i].Size);
                }
            }

            return filtered;
        }


        ////////////////////////////////////////////////////////


        // returns true if success
        private async Task<bool> NewFilePrivate(string registryAddress, File file, string fileHash, string metadataHash)
        {
            // get number of files
            int count1 = await GetNumberOfFiles.SendRequestAsync(MyWeb3, registryAddress);

            // deploy new file contract to blockchain
            await DeployFile.SendRequestAsync(MyWeb3, registryAddress, file.Name, file.FileId,
                fileHash, metadataHash, file.Type, file.Etag);

            // check that number of files increased
            int count2 = await GetNumberOfFiles.SendRequestAsync(MyWeb3, registryAddress);

            return ((count2 - count1) == 1);
        }

        // returns state
        private async Task<int> NewReceiptPrivate(string registryAddress, string fileId, string fileHash,
            string metadataHash, string verifiedBy)
        {
            // deploy new receipt contract to blockchain
            string receiptAddress = await DeployReceipt.SendRequestAsync(MyWeb3, registryAddress, fileId,
                fileHash, metadataHash, verifiedBy);

            // get state of receipt
            return await GetState.SendRequestAsync(MyWeb3, receiptAddress);
        }
    }
}
