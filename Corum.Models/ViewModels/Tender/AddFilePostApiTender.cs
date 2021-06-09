using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Corum.Models.ViewModels.Tender
{
    public class AddFilePostApiTender
    {
        public async Task<ResponseApi> GetCallAsync(HttpClientApi clientbase, byte[] data, string filename)
        {
            HttpClient client = clientbase.client;
            byte[] postContent = data;
            ResponseApi baseresponse = clientbase.baseresponse;
            var requestContent = new MultipartFormDataContent();
            var streamContent = new ByteArrayContent(data);
            streamContent.Headers.Add("Content-Type", "application/octet-stream");
            streamContent.Headers.Add("Content-Disposition", "form-data; name=\"Orderfile\"; filename=\"" + filename + "\"");
            requestContent.Add(streamContent, "Orderfile", filename);

            int count = 0;
            try
            {
                while (count < 10)
                {
                    baseresponse.response = client.PostAsync(client.BaseAddress.AbsoluteUri, requestContent).Result;
                    if (baseresponse.response.IsSuccessStatusCode)
                    {
                        baseresponse.ResponseMessage = await baseresponse.response.Content.ReadAsStringAsync();
                        baseresponse.StatusCode = (int)baseresponse.response.StatusCode;
                        count = 10;
                    }
                    else
                    {
                        baseresponse.ResponseMessage = await baseresponse.response.Content.ReadAsStringAsync();
                        baseresponse.StatusCode = (int)baseresponse.response.StatusCode;
                    }
                    ++count;
                }
                return baseresponse;
            }
            catch (Exception ex)
            {
                baseresponse.StatusCode = 0;
                baseresponse.ResponseMessage = (ex.Message ?? ex.InnerException.ToString());
            }
            return baseresponse;
        }
    }
    public class ResponseApi
    {
        public int StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public HttpResponseMessage response;
    }

    public class HttpClientApi
    {
        public readonly HttpClient client;
        public readonly ResponseApi baseresponse;

        public HttpClientApi(string baseAddress, string username, string password)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            var val = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            baseresponse = new ResponseApi();
        }
    }
}
