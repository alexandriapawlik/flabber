//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		VerificationManager class
// Purpose:		Encapsulates all of web app user's options in one object

//////////////////////////////////////////////


using System.Threading.Tasks;

using FV.Verification.Eth;
using FV.Verification.Drive;
using FV.Verification.Eth.Objects;
using FV.Verification.Drive.Objects;

namespace FV.Verification
{
    public class VerificationManager
    {
        public EthInteraction MyEthInteraction;
        public DriveInteraction MyDriveInteraction;

        public VerificationManager(string registryAddress)
        {
            MyDriveInteraction = new DriveInteraction(registryAddress);
            MyEthInteraction = new EthInteraction(registryAddress);
        }

        // generate new receipt for file of given id
        // returns state of new receipt (verification result)
        public async Task<string> VerifyFile(string fileId)
        {
            // get file hash and metadata hash
            string fileHash = await MyDriveInteraction.GetFileHash(fileId);
            string metadataHash = await MyDriveInteraction.GetMetadataHash(fileId);

            // get name of current user
            string user = await MyDriveInteraction.GetMe();

            // make new receipt contract
            return MyEthInteraction.NewReceipt(fileId, fileHash, metadataHash, user);      
        }

        // returns the history of verifications of the file of given id
        public async Task<History> GetHistory(string fileId)
        {
            // get history object
            return await MyEthInteraction.GetHistory(fileId);
        }

        // returns a list of the files in the onedrive folder
		// that have already been added to registry
        public async Task<FileList> GetFilesToVerify()
        {
            // get list of files in Drive folder
            FileList list = await MyDriveInteraction.GetFileList();

            // filter list down to only unregistered files
            return await MyEthInteraction.OnlyRegistered(list);
        }

        // returns a list of files in the onedrive folder
        // that have not yet been added to registry
        public async Task<FileList> GetFilesToRegister()
        {
            // get list of files in Drive folder
            FileList list = await MyDriveInteraction.GetFileList();

            // filter list down to only unregistered files
            return await MyEthInteraction.OnlyUnregistered(list);
        }

        // add a new file to the registry
        // returns the name of registered file
        public async Task<string> RegisterFile(string fileId)
        {
            // get file info
            File file = await MyDriveInteraction.GetFile(fileId);

            // get file hash and metadata hash
            string fileHash = await MyDriveInteraction.GetFileHash(file.FileId);
            string metadataHash = await MyDriveInteraction.GetMetadataHash(file.FileId);

            // add new file contract
            await MyEthInteraction.NewFile(file, fileHash, metadataHash);
            return file.Name;
        }
    }
}
