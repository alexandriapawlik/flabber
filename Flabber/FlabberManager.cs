//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		FlabberManager class
// Purpose:		Encapsulates all of web app user's options in one object

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using Flabber.Login;
using Flabber.Eth;
using Flabber.Drive;
using Flabber.Objects;

namespace Flabber
{
    public class FlabberManager
    {
        public string Me { get; }  // current user's username

        private readonly LoginManager MyLoginManager;
        private readonly EthManager MyEthManager;
        private readonly DriveManager MyDriveManager;
        private string Token;
        private string RegistryAddress;  // defaults to null
        

        ////////////////////////////////////////////////////////


        public FlabberManager()
        {
            // get token
            MyLoginManager = new LoginManager();
            Token = MyLoginManager.GetToken();

            // check user
            MyDriveManager = new DriveManager(Token);
            Me = MyDriveManager.GetMe().GetAwaiter().GetResult();
            if (!MyLoginManager.IsAllowed(Me))
            {
                throw new Exception("User not allowed");
            }

            MyEthManager = new EthManager();
        }


        ////////////////////////////////////////////////////////


        // change the registry address to work with a new registry
        public void SetRegistry(string registryAddress)
        {
            RegistryAddress = registryAddress;
        }

        // gets registry names and addresses
        public async Task<RegistryList> GetRegistryList()
        {
            return await MyEthManager.GetRegistryList();
        }

        // create a new file registry
        // returns the address of the deployed contract
        public async Task<string> AddRegistry(string name, string description)
        {
            // add new file registry contract
            return await MyEthManager.NewRegistry(name, description);
        }

        // remove a registry from the library
        public async Task RemoveRegistry(string registryAddress)
        {
            if (registryAddress == null)
            {
                throw new Exception("No address specified");
            }

            // mark the registry as deactivated
            await MyEthManager.RemoveRegistry(registryAddress);
        }

        // returns a list of files in the onedrive folder
        // that have not yet been added to registry
        public async Task<FileList> GetFilesToRegister()
        {
            if (RegistryAddress != null)
            {
                // get list of files in Drive folder
                FileList list = await MyDriveManager.GetFileList(RegistryAddress);

                // filter list down to only unregistered files
                return await MyEthManager.OnlyUnregistered(RegistryAddress, list);
            }

            throw new Exception("No file registry chosen");
        }

        // returns a list of the files in the onedrive folder
        // that have already been added to registry
        public async Task<FileList> GetFilesToVerify()
        {
            if (RegistryAddress != null)
            {
                // get list of files in Drive folder
                FileList list = await MyDriveManager.GetFileList(RegistryAddress);

                // filter list down to only unregistered files
                return await MyEthManager.OnlyRegistered(RegistryAddress, list);
            }

            throw new Exception("No file registry chosen");
        }

        // returns the history of verifications of the file of given id
        public async Task<History> GetHistory(string fileId)
        {
            if (RegistryAddress != null)
            {
                // get history object
                return await MyEthManager.GetHistory(RegistryAddress, fileId);
            }

            throw new Exception("No file registry chosen");
        }

        // add a new file to the registry
        // returns the name of registered file
        public async Task<string> RegisterFile(string fileId)
        {
            if (RegistryAddress != null)
            {
                // get file info
                File file = await MyDriveManager.GetFile(RegistryAddress, fileId);

                // get file hash and metadata hash
                string fileHash = await MyDriveManager.GetFileHash(RegistryAddress, file.FileId);
                string metadataHash = await MyDriveManager.GetMetadataHash(RegistryAddress, file.FileId);

                // add new file contract
                await MyEthManager.NewFile(RegistryAddress, file, fileHash, metadataHash);
                return file.Name;
            }

            throw new Exception("No file registry chosen");
        }

        // generate new receipt for file of given id
        // returns state of new receipt (verification result)
        public async Task<string> VerifyFile(string fileId)
        {
			if (RegistryAddress != null)
            {
                // get file hash and metadata hash
            	string fileHash = await MyDriveManager.GetFileHash(RegistryAddress, fileId);
            	string metadataHash = await MyDriveManager.GetMetadataHash(RegistryAddress, fileId);

            	// get name of current user
            	string user = await MyDriveManager.GetMe();

            	// make new receipt contract
            	return await MyEthManager.NewReceipt(RegistryAddress, fileId, fileHash, metadataHash, user);
            }

            throw new Exception("No file registry chosen");
        }

        // get new user token (only use if token does not work)
        // returns false if token is null
        public bool NewToken()
        {
            Token = MyLoginManager.GetToken();

            if (Token == null)
            {
                return false;
            }

            return true;
        }
    }
}
