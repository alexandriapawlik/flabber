//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		ProtectedApiCallHelper class
// Purpose:		Calls protected Microsoft Graph API and processes result

//////////////////////////////////////////////


using Newtonsoft.Json.Linq;

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FV.Drive.Objects
{
    public class ProtectedApiCallHelper
    {
        protected HttpClient MyHttpClient { get; private set; }

        public ProtectedApiCallHelper(HttpClient httpClient)
        {
            MyHttpClient = httpClient;
        }

        // calls the protected Web API and processes the result to get a list of files
        public async Task<FileList> CallWebApiFileListAndProcessResultAsync(string webApiUrl, string accessToken)
        {
            FileList list = new FileList();

            if (!string.IsNullOrEmpty(accessToken))
            {
                HttpResponseMessage response = await MakeHttpRequestAsync(webApiUrl, accessToken);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    // convert entire json response to object
                    JObject result = JObject.Parse(json);
                    // convert useful part of json reponse to array of file data
                    JArray files = (JArray)result["value"];

                    // for each file
                    for (int i = 0; i < files.Count; ++i)
                    {
                        JObject file = (JObject)files[i];

                        // pull each required piece of data
                        string name = (string)file["name"];
                        string id = (string)file["id"];
                        string type = (string)file["file"]["mimeType"];
                        string eTag = (string)file["eTag"];
                        long size = (long)file["size"];
                        
                        // process result into file list object
                        list.AddLine(name, id, type, eTag, size);
                    }
                }
                else
                {
                    //string content = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($"Content: {content}");

                    throw new Exception($"File list API call failed: {response.StatusCode}");
                }
            }
            return list;
        }

        // calls the protected Web API to get data at specified url
        public async Task<string> CallWebApiAndProcessResultAsync(string webApiUrl, string accessToken)
        {
            string hash = "";

            if (!string.IsNullOrEmpty(accessToken))
            {
                HttpResponseMessage response = await MakeHttpRequestAsync(webApiUrl, accessToken);

                if (response.IsSuccessStatusCode)
                {
                    hash = await ParseValueFromJson(response);
                }
                else
                {
                    //string content = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($"Content: {content}");

                    throw new Exception($"API call failed: {response.StatusCode}");
                }
            }
            return hash;
        }

        // calls the protected Web API and processes the result to get data about a file
        public async Task<File> CallWebApiFileAndProcessResultAsync(string webApiUrl, string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
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

                //string content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"Content: {content}");

                throw new Exception($"File API call failed: {response.StatusCode}");
            }
            return new File();
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
