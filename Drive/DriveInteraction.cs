//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DriveInteraction class
// Purpose:		Initiates all interactions with OneDrive
//              Uses ProtectedApiCallHelper to make Microsoft Graph API calls

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Identity.Client;

using FV.Drive.Objects;
using FV.Infrastructure;

namespace FV.Drive
{
    public class DriveInteraction
    {
        protected ProtectedApiCallHelper protectedApiCallHelper;

        // scopes to request access to the Microsoft Graph API
        private static string[] Scopes { get; set; } = new string[] { "User.ReadBasic.All", "Files.Read.All" };
        // base endpoint for Microsoft Graph
        private string MicrosoftGraphBaseEndpoint { get; set; }
        // address of registry contract on blockchain
        // used to pick urls
        private string Registry { get; set; }
        private string Token { get; set; }


        ////////////////////////////////////////////////////////


        public DriveInteraction(string registry)
        {
            var client = new HttpClient();
            protectedApiCallHelper = new ProtectedApiCallHelper(client);
            MicrosoftGraphBaseEndpoint = Constants.MicrosoftGraphBaseEndpoint;
            Registry = registry;
            GetToken();
        }


        ////////////////////////////////////////////////////////


        // make a call to Microsoft Graph until it succeeds or runs 3 attempts
        // to get a list of files currently in the FV directory on OneDrive
        public async Task<FileList> GetFileList()
        {
            FileList list = new FileList();
            int loops = 0;

            bool again = true;
            while ((again) && (loops < 3))
            {
                again = false;
                try
                {
                    list = await GetFileListAsync();
                }
                catch (Exception)
                {
                    if (loops == 2)
                    {
                        throw;
                    }
                    again = true;
                    ++loops;
                }
            }

            return list;
        }

        // make a call to Microsoft Graph until it succeeds or runs 3 attempts
        // to get a hash of the file's content
        public async Task<string> HashFile(string fileId)
        {
            string hash = "";
            int loops = 0;

            bool again = true;
            while ((again) && (loops < 3))
            {
                again = false;
                try
                {
                    hash = await GetFileHashAsync(fileId);
                }
                catch (Exception)
                {
                    if (loops == 2)
                    {
                        throw;
                    }
                    again = true;
                    ++loops;
                }
            }

            return hash;
        }

        // make a call to Microsoft Graph until it succeeds or runs 3 attempts
        // to get a hash of the file's metadata
        public async Task<string> HashMetadata(string fileId)
        {
            string hash = "";
            int loops = 0;

            bool again = true;
            while ((again) && (loops < 3))
            {
                again = false;
                try
                {
                    hash = await GetMetadataHashAsync(fileId);
                }
                catch (Exception)
                {
                    if (loops == 2)
                    {
                        throw;
                    }
                    again = true;
                    ++loops;
                }
            }

            return hash;
        }

        // make a call to Microsoft Graph until it succeeds or runs 3 attempts
        // to get data about a file
        public async Task<File> GetFile(string fileId)
        {
            File file = new File();
            int loops = 0;

            bool again = true;
            while ((again) && (loops < 3))
            {
                again = false;
                try
                {
                    file = await GetFileAsync(fileId);
                }
                catch (Exception)
                {
                    if (loops == 2)
                    {
                        throw;
                    }
                    again = true;
                    ++loops;
                }
            }

            return file;
        }


        ////////////////////////////////////////////////////////


        // get token and use API call helper function to retrieve file list
        private async Task<FileList> GetFileListAsync()
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.FileList(Registry)}";

            FileList list = await protectedApiCallHelper.CallWebApiFileListAndProcessResultAsync(Url, Token);

            return list;
        }

        // get token and use API call helper function to retrieve file hash
        private async Task<string> GetFileHashAsync(string fileId)
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.FileHash(Registry, fileId)}";

            string fileHash = await protectedApiCallHelper.CallWebApiAndProcessResultAsync(Url, Token);

            return fileHash;
        }

        // get token and call API call helper function to retrieve file metadata hash
        // currently retrieves size rather than metadata hash
        private async Task<string> GetMetadataHashAsync(string fileId)
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.MetadataHash(Registry, fileId)}";

            string metadataHash = await protectedApiCallHelper.CallWebApiAndProcessResultAsync(Url, Token);

            return metadataHash;
        }

        // get token and use API call helper function to retrieve data about a file
        private async Task<File> GetFileAsync(string fileId)
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.File(Registry, fileId)}";

            File file = await protectedApiCallHelper.CallWebApiFileAndProcessResultAsync(Url, Token);

            return file;
        }

        private void GetToken()
        {
            var app = PublicClientApplicationBuilder
                            .Create(Constants.ClientId)
                            .WithRedirectUri("http://localhost")
                            .WithTenantId(Constants.TenantId)
                            .Build();

            var authenticationResult = app.AcquireTokenInteractive(Scopes).ExecuteAsync().GetAwaiter().GetResult();

            if (authenticationResult != null)
            {
                Token = authenticationResult.AccessToken;
            }
            else
            {
                throw new Exception("Authentication failed");
            }

            if (Token == null)
            {
                throw new Exception("Unable to aquire authentication token");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Authentication successful");
            Console.WriteLine($"Expires: {authenticationResult.ExpiresOn}"); 
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
