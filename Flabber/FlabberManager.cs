//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		FlabberManager class
// Purpose:		Encapsulates all of web app user's options in one object
//              Is the only object that the driver program needs to create

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


        // changes the stored registry address to allow the object to work with a different registry
        // must be called at least once before GetFilesToRegister, GetFilesToVerify, GetHistory,
        // RegisterFile, or VerifyFile can be called
        public void SetRegistry(string registryAddress)
        {
            RegistryAddress = registryAddress;
        }

        // returns a RegistryList object containing the current active registries in the library
        public async Task<RegistryList> GetRegistryList()
        {
            return await MyEthManager.GetRegistryList();
        }

        // creates a new file registry
        // returns the address of the deployed FileRegistry smart contract
        // so that it can be added to the mappings in Constants.cs
        public async Task<string> AddRegistry(string name, string description)
        {
            // add new file registry contract
            return await MyEthManager.NewRegistry(name, description);
        }

        // removes a regsitry from the library by setting its state to deactive
        public async Task RemoveRegistry(string registryAddress)
        {
            if (registryAddress == null)
            {
                throw new Exception("No address specified");
            }

            // mark the registry as deactivated
            await MyEthManager.RemoveRegistry(registryAddress);
        }

        // returns a FileList object containing the files in OneDrive
        // that have not yet been added to the current file registry
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

        // returns a FileList object containing the files in OneDrive
        // that have been added to the current file registry
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

        // returns a History object containing all the existing receipts
        // for the file with the given ID
        public async Task<History> GetHistory(string fileId)
        {
            if (RegistryAddress != null)
            {
                // get history object
                return await MyEthManager.GetHistory(RegistryAddress, fileId);
            }

            throw new Exception("No file registry chosen");
        }

        // adds a new file to the current file registry
        // returns the name of the registered file
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

        // generates a new verification receipt for the file with the given ID
        // returns the verification result of the new receipt
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

        // gets a new login token for the user, which will require them to sign in again
        // returns false if the token is null
        // should only be used if the user needs to re-sign in for some reason
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
