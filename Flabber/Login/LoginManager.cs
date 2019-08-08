//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		LoginManager class
// Purpose:		Retrieves access token for OneDrive
//              Manages allowed app users

//////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Identity.Client;

using Flabber.Infrastructure;

namespace Flabber.Login
{
    public class LoginManager
    {
        private readonly List<string> Users;


        ////////////////////////////////////////////////////////


        public LoginManager()
        {
            Users = new List<string>
            {
                "Alexandria.Pawlik@accidentfund.com",
                "Craig.Bilinski@accidentfund.com"
				// ADD NEW USERS HERE
            };
        }


        ////////////////////////////////////////////////////////


		// opens login window and aquires authentication token
        public string GetToken()
        {
            string token = "";

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
                    authenticationResult = (app as PublicClientApplication)
                        .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
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
                // define cancellation token
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken cancelToken = source.Token;

                try
                {
                    // aquire an access token for this app interactively
                    authenticationResult = app.AcquireTokenInteractive(Constants.Scopes)
                        .ExecuteAsync(cancelToken).GetAwaiter().GetResult();

                    if (authenticationResult != null)
                    {
                        token = authenticationResult.AccessToken;
                    }
                    else
                    {
                        throw new Exception("Authentication failed");
                    }

                    if (token == null)
                    {
                        throw new Exception("Unable to aquire authentication token");
                    }
                }
                catch (AggregateException ae) {
                    foreach (Exception e in ae.InnerExceptions)
                    {
                        if (e is TaskCanceledException)
                            throw new TaskCanceledException("Login timeout", e);
                        else
                            throw e;
                    }
                }
                finally
                {
                    source.Dispose();
                } 
            }

            return token;
        }

        public bool IsAllowed(string user)
        {
            return Users.Contains(user);
        }
    }
}
