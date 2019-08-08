//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		EthInteraction class
// Purpose:		Initiates all interactions with private Ethereum blockchain
//              Uses function static classes to make calls to Web3 API

//////////////////////////////////////////////


using System.Threading.Tasks;

using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Web3.Accounts;

using FV.Eth.Functions;
using FV.Eth.Objects;
using FV.Drive.Objects;
using FV.Infrastructure;

namespace FV.Eth
{
    public class EthInteraction
    {
        public Contract RegistryContract { get; set; }
        public Web3 MyWeb3 { get; set; }

        private readonly string RegistryAddress;
        private readonly Account MyAccount;


        ////////////////////////////////////////////////////////


        // constructor that takes address of registry contract
        public EthInteraction(string registryAddress)
        {
            // open admin account to use for transactions
            var privateKey = Constants.Key;
            MyAccount = new Account(privateKey);

            // initialize
            RegistryAddress = registryAddress;
            MyWeb3 = MakeConnection();
            RegistryContract = GetRegistryObj();
        }


        ////////////////////////////////////////////////////////


        // adds new receipt contract for file with fileid and provided hashes
        // returns state of new receipt
        public async Task<string> NewReceipt(string fileId, string fileHash, string metadataHash)
        {
            // deploy new receipt contract to blockchain
            // must run sync
            string receiptAddress = DeployReceipt.SendRequestAsync(this, fileId, fileHash, metadataHash).GetAwaiter().GetResult();

            // get state of receipt
            // must run sync
            int state = await GetState.SendRequestAsync(this, receiptAddress);

            return (StateConvert.IntToString(state));
        }

        // gets receipt datetimes and states for file with fileid
        public async Task<History> GetHistory(string fileId)
        {
            History history = new History();

            // get address of file contract with corresponding fileid
            string fileAddress = await GetFileAddressGivenId.SendRequestAsync(this, fileId);

            // call number of receipts function on file contract
            int numReceipts = await GetNumberOfReceipts.SendRequestAsync(this, fileAddress);

            // for each receipt
            for (int i = 0; i < numReceipts; ++i)
            {
                // get receipt address at index
                string receiptAddress = await GetReceiptAddressAtIndex.SendRequestAsync(this, i, fileAddress);

                // get datetime
                string dateTime = await GetVerificationDateTime.SendRequestAsync(this, receiptAddress);

                // get state of receipt
                string state = StateConvert.IntToString(await GetState.SendRequestAsync(this, receiptAddress));

                // add values to History object
                history.AddLine(dateTime, state);
            }

            return history;
        }

        // filter file list down to only unregistered files
        public async Task<FileList> OnlyUnregistered(FileList list)
        {
            FileList filtered = new FileList();
            //Console.WriteLine(await GetNumberOfFiles.SendRequestAsync(this));

            // for each file
            for (int i = 0; i < list.NumFiles; ++i)
            {
                // check if the file has been registered
                bool registered = await IsRegisteredFileId.SendRequestAsync(this, list.Files[i].FileId);

                // if it hasn't been registered, add to new list
                if (!registered)
                {
                    filtered.AddLine(list.Files[i].Name, list.Files[i].FileId, list.Files[i].Type, list.Files[i].Etag, list.Files[i].Size);
                }
            }

            return filtered;
        }

        // filter file list down to only unregistered files
        public async Task<FileList> OnlyRegistered(FileList list)
        {
            FileList filtered = new FileList();

            // for each file
            for (int i = 0; i < list.NumFiles; ++i)
            {
                // check if the file has been registered
                bool registered = await IsRegisteredFileId.SendRequestAsync(this, list.Files[i].FileId);

                // if it has been registered, add to new list
                if (registered)
                {
                    filtered.AddLine(list.Files[i].Name, list.Files[i].FileId, list.Files[i].Type, list.Files[i].Etag, list.Files[i].Size);
                }
            }

            return filtered;
        }

        // adds new file contract for file with provided data
        public async Task NewFile(File file, string fileHash, string metadataHash)
        {
            // deploy new file contract to blockchain
            await DeployFile.SendRequestAsync(this, file.Name, file.FileId, fileHash, metadataHash, file.Type, file.Etag);
        }


        ////////////////////////////////////////////////////////


        // connect to RPC endpoint and return MyWeb3 object
        private Web3 MakeConnection()
        {
            // URL for RPC endpoint of Ethereum POA blockchain
            string url = Constants.URL;

            // create connection to network
            MyWeb3 = new Web3(MyAccount, url);
            return MyWeb3;
        }

        // create a contract object for registry contract at registry address
        private Contract GetRegistryObj()
        {
            // receipt contract ABI
            string registryABI = Constants.RegistryABI;

            Contract registryContract = MyWeb3.Eth.GetContract(registryABI, RegistryAddress);
            return registryContract;
        }

        //// create a contract object for file contract at given address
        //private Contract GetFileObj(string address)
        //{
        //    // file contract ABI
        //    string fileABI = Constants.FileABI;

        //    Contract fileContract = MyWeb3.Eth.GetContract(fileABI, address);
        //    return fileContract;
        //}

        //// create a contract object for receipt contract at given address
        //private Contract GetReceiptObj(string address)
        //{
        //    // receipt contract ABI
        //    string receiptABI = Constants.ReceiptABI; 

        //    Contract receiptContract = MyWeb3.Eth.GetContract(receiptABI, address);
        //    return receiptContract;
        //}
    }
}
