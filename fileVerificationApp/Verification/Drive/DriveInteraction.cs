//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		DriveInteraction class
// Purpose:		Initiates all interactions with OneDrive
//              Uses ApiCallHelper to make Microsoft Graph API calls

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using Microsoft.Identity.Client;

using FV.Verification.Drive.Objects;
using FV.Infrastructure;
using System.Linq;

namespace FV.Verification.Drive
{
    public class DriveInteraction
    {
        protected ApiCallHelper ApiCaller;

        // scopes to request access to the Microsoft Graph API
        private static string[] Scopes { get; set; } = new string[] { "User.ReadBasic.All", "Files.Read.All" };
        // base endpoint for Microsoft Graph
        private string MicrosoftGraphBaseEndpoint { get; set; }
        // address of registry contract on blockchain
        private string Registry { get; set; }
				// access token
        private string Token { get; set; }


        ////////////////////////////////////////////////////////


        public DriveInteraction(string registry)
        {
            ApiCaller = new ApiCallHelper();
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
        public async Task<string> GetFileHash(string fileId)
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
        public async Task<string> GetMetadataHash(string fileId)
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

        // make a call to Microsoft Graph until it succeeds or runs 3 attempts
        // to get name of current user
        public async Task<string> GetMe()
        {
            string me = "";
            int loops = 0;

            bool again = true;
            while ((again) && (loops < 3))
            {
                again = false;
                try
                {
                    me = await GetMeAsync();
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

            return me;
        }


        ////////////////////////////////////////////////////////


        // use API call helper function to retrieve file list
        private async Task<FileList> GetFileListAsync()
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.FileList(Registry)}";

            FileList list = await ApiCaller.CallWebApiFileListAndProcessResultAsync(Url, Token);

            return list;
        }

        // use API call helper function to retrieve file hash
        private async Task<string> GetFileHashAsync(string fileId)
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.FileHash(Registry, fileId)}";

            string fileHash = await ApiCaller.CallWebApiAndProcessResultAsync(Url, Token);

            return fileHash;
        }

        // call API call helper function to retrieve file metadata hash
        // currently retrieves size rather than metadata hash
        private async Task<string> GetMetadataHashAsync(string fileId)
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.MetadataHash(Registry, fileId)}";

            string metadataHash = await ApiCaller.CallWebApiAndProcessResultAsync(Url, Token);

            return metadataHash;
        }

        // use API call helper function to retrieve data about a file
        private async Task<File> GetFileAsync(string fileId)
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.File(Registry, fileId)}";

            File file = await ApiCaller.CallWebApiFileAndProcessResultAsync(Url, Token);

            return file;
        }

        // use API call helper function to retrieve current user's name
        private async Task<string> GetMeAsync()
        {
            string Url = $"{MicrosoftGraphBaseEndpoint}{DriveUrls.Me()}";

            string me = await ApiCaller.CallWebApiAndProcessResultAsync(Url, Token);

            return me;
        }

        private void GetToken()
        {
			// identify this app
            var app = PublicClientApplicationBuilder
                            .Create(Constants.ClientId)
                            .WithRedirectUri("http://localhost")
                            .WithTenantId(Constants.TenantId)
                            .Build();

            AuthenticationResult authenticationResult = null;
            var accounts = app.GetAccountsAsync().GetAwaiter().GetResult();

            // if the app already has an account
            if (accounts.Any())
            {
                try
                {
                    // attempt to get a token from the cache (or refresh it silently if needed)
                    authenticationResult = (app as PublicClientApplication).AcquireTokenSilent(Scopes, accounts.FirstOrDefault())
                        .ExecuteAsync().GetAwaiter().GetResult();
                }
                catch (MsalUiRequiredException)
                {
                    // no token for the account
                }
            }

            // cache empty or no token for account in the cache, attempt interactively
            if (authenticationResult == null)
            {
                // aquire an access token for this app interactively
                authenticationResult = app.AcquireTokenInteractive(Scopes).ExecuteAsync().GetAwaiter().GetResult();

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
}
