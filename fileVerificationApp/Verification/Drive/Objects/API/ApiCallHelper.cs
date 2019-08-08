//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		ApiCallHelper class
// Purpose:		Calls protected Microsoft Graph API and processes result

//////////////////////////////////////////////


using Newtonsoft.Json.Linq;

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FV.Verification.Drive.Objects
{
    public class ApiCallHelper
    {
        protected HttpClient MyHttpClient { get; private set; }

        public ApiCallHelper()
        {
            MyHttpClient = new HttpClient();
        }

        // calls the protected Web API to get data at specified url
        public async Task<string> CallWebApiAndProcessResultAsync(string webApiUrl, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
			    throw new Exception($"Invalid access token");
            }

            HttpResponseMessage response = await MakeHttpRequestAsync(webApiUrl, accessToken);

            if (response.IsSuccessStatusCode)
            {
                return await ParseValueFromJson(response);
            }

            // throw an exception if the response has a failure status code
            throw new Exception($"API call failed: {response.StatusCode}");
        }

        // calls the protected Web API and processes the result to get a list of files
        public async Task<FileList> CallWebApiFileListAndProcessResultAsync(string webApiUrl, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception($"Invalid access token");
            }

            HttpResponseMessage response = await MakeHttpRequestAsync(webApiUrl, accessToken);

            if (response.IsSuccessStatusCode)
            {
                FileList list = new FileList();
                string json = await response.Content.ReadAsStringAsync();

                // convert entire json response to object
                JObject result = JObject.Parse(json);
                // convert useful part of json reponse to array of file data
                JArray children = (JArray)result["value"];

                // for each file
                for (int i = 0; i < children.Count; ++i)
                {
                    JObject child = (JObject)children[i];

                    // make sure child is not actually a folder
                    if (!child.ContainsKey("folder"))
                    {
                        // pull each required piece of data
                        string name = (string)child["name"];
                        string id = (string)child["id"];
                        string type = (string)child["file"]["mimeType"];
                        string eTag = (string)child["eTag"];
                        long size = (long)child["size"];

                        // process result into file list object
                        list.AddLine(name, id, type, eTag, size);
                    } 
                }
                return list;
            }

            // throw an exception if the response has a failure status code
            throw new Exception($"File list API call failed: {response.StatusCode}");
        }

        // calls the protected Web API and processes the result to get data about a file
        public async Task<File> CallWebApiFileAndProcessResultAsync(string webApiUrl, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception($"Invalid access token");
            }

            HttpResponseMessage response = await MakeHttpRequestAsync(webApiUrl, accessToken);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                // convert entire json response to object
                JObject result = JObject.Parse(json);

                // process result into file object
                return new File(
                    (string)result["name"],
                    (string)result["id"],
                    (string)result["file"]["mimeType"],
                    (string)result["eTag"],
                    (long)result["size"]);
            }

            // throw an exception if the response has a failure status code
            throw new Exception($"File API call failed: {response.StatusCode}");
        }


        ///////////////////////////////////////////////////////////


        private async Task<HttpResponseMessage> MakeHttpRequestAsync(string webApiUrl, string accessToken)
        {
            var defaultRequetHeaders = MyHttpClient.DefaultRequestHeaders;

            if (defaultRequetHeaders.Accept == null || !defaultRequetHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                MyHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            defaultRequetHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            HttpResponseMessage response = await MyHttpClient.GetAsync(webApiUrl);
            return response;
        }

        // pulls value from json object and returns it
        private async Task<string> ParseValueFromJson(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            // convert json response to object
            JObject result = JObject.Parse(json);

            // parse value out of json object
            return (string)result["value"];
        }
    }
}
