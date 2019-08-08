//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		VerificationManager class
// Purpose:		Encapsulates all of web app user's options in one object

//////////////////////////////////////////////


using System.Threading.Tasks;

using FV.Eth;
using FV.Drive;
using FV.Eth.Objects;
using FV.Drive.Objects;

namespace FV
{
    public class VerificationManager
    {
        public EthInteraction EthConnection;
        public DriveInteraction DriveConnection;

        public VerificationManager(string registryAddress)
        {
            DriveConnection = new DriveInteraction(registryAddress);
            EthConnection = new EthInteraction(registryAddress);
        }

        // generate new receipt for file of given id
        // returns state of new receipt
        public async Task<string> VerifyFile(string fileId)
        {
            // get file hash and metadata hash
            string fileHash = await DriveConnection.HashFile(fileId);
            string metadataHash = await DriveConnection.HashMetadata(fileId);

            // add new receipt contract
            return await EthConnection.NewReceipt(fileId, fileHash, metadataHash);
        }

        // returns the history of verifications of the file of given id
        public async Task<History> GetHistory(string fileId)
        {
            // get history object
            return await EthConnection.GetHistory(fileId);
        }

        // returns a list of the files in the onedrive folder
		// that have already been added to registry
        public async Task<FileList> GetFileList()
        {
            // get list of files in Drive folder
            FileList list = await DriveConnection.GetFileList();

            // filter list down to only unregistered files
            return await EthConnection.OnlyRegistered(list);
        }

        // returns a list of files in the onedrive folder
        // that have not yet been added to registry
        public async Task<FileList> GetFilesToRegister()
        {
            // get list of files in Drive folder
            FileList list = await DriveConnection.GetFileList();

            // filter list down to only unregistered files
            return await EthConnection.OnlyUnregistered(list);
        }

        // add a new file to the registry
        // returns the name of registered file
        public async Task<string> RegisterFile(string fileId)
        {
            // get file info
            File file = await DriveConnection.GetFile(fileId);

            // get file hash and metadata hash
            string fileHash = await DriveConnection.HashFile(file.FileId);
            string metadataHash = await DriveConnection.HashMetadata(file.FileId);

            // add new file contract
            await EthConnection.NewFile(file, fileHash, metadataHash);
            return file.Name;
        }
    }
}
