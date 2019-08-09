//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DriveManager class
// Purpose:		Initiates all interactions with OneDrive

//////////////////////////////////////////////


using System.Threading.Tasks;

using Flabber.Infrastructure;
using Flabber.Objects;

namespace Flabber.Drive
{
    public class DriveManager
    {
        private ApiCallHelper ApiCaller;
        private string Token { get; set; }  // access token for OneDrive


        ////////////////////////////////////////////////////////


        // requires access token that LoginManager retrieves
        public DriveManager(string token)
        {
            ApiCaller = new ApiCallHelper();
            Token = token;
        }


        ////////////////////////////////////////////////////////


        // use API call helper function to retrieve current user's name
        public async Task<string> GetMe()
        {
            string Url = $"{Constants.MicrosoftGraphBaseEndpoint}{DriveUrls.Me()}";

            string me = await ApiCaller.CallWebApiAndProcessResultAsync(Url, Token);

            return me;
        }

        // use API call helper function to retrieve data about a file
        public async Task<File> GetFile(string registryAddress, string fileId)
        {
            string Url = $"{Constants.MicrosoftGraphBaseEndpoint}{DriveUrls.File(registryAddress, fileId)}";

            File file = await ApiCaller.CallWebApiFileAndProcessResultAsync(Url, Token);

            return file;
        }

        // use API call helper function to retrieve file list
        public async Task<FileList> GetFileList(string registryAddress)
        {
            string Url = $"{Constants.MicrosoftGraphBaseEndpoint}{DriveUrls.FileList(registryAddress)}";

            FileList list = await ApiCaller.CallWebApiFileListAndProcessResultAsync(Url, Token);

            return list;
        }

        // use API call helper function to retrieve file hash
        public async Task<string> GetFileHash(string registryAddress, string fileId)
        {
            string Url = $"{Constants.MicrosoftGraphBaseEndpoint}{DriveUrls.FileHash(registryAddress, fileId)}";

            string fileHash = await ApiCaller.CallWebApiAndProcessResultAsync(Url, Token);

            return fileHash;
        }

        // call API call helper function to retrieve file metadata hash
        // currently retrieves size rather than metadata hash
        public async Task<string> GetMetadataHash(string registryAddress, string fileId)
        {
            string Url = $"{Constants.MicrosoftGraphBaseEndpoint}{DriveUrls.MetadataHash(registryAddress, fileId)}";

            string metadataHash = await ApiCaller.CallWebApiAndProcessResultAsync(Url, Token);

            return metadataHash;
        }
    }
}
