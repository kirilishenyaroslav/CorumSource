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

namespace Corum.Models.ViewModels.Tender
{
    public class PostApiTender
    {
        public async Task<BaseResponse> GetCallAsync(BaseClient clientbase, TenderForma postValues)
        {
            HttpClient client = clientbase.client;
            TenderForma postContent = postValues;
            BaseResponse baseresponse = clientbase.baseresponse;
            try
            {
                baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postContent).Result;
                if (baseresponse.response.IsSuccessStatusCode)
                {
                    baseresponse.ResponseMessage = await baseresponse.response.Content.ReadAsStringAsync();
                    baseresponse.StatusCode = (int)baseresponse.response.StatusCode;
                    string content = string.Empty;
                    using (StreamReader stream = new StreamReader(baseresponse.response.Content.ReadAsStreamAsync().Result, System.Text.Encoding.GetEncoding(Encoding.UTF8.WebName)))
                    {
                        content = stream.ReadToEnd();
                        string path = @"C:\Users\Work\Dropbox\Стажировка\Corum project\CorumSource-master\Corum.AdminUI\bin\client-server_Api.json";
                        File.WriteAllText(path, content);
                    }
                }
                else
                {
                    baseresponse.ResponseMessage = await baseresponse.response.Content.ReadAsStringAsync();
                    baseresponse.StatusCode = (int)baseresponse.response.StatusCode;
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
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public HttpResponseMessage response;
    }

    public class BaseClient
    {
        public readonly HttpClient client;
        public readonly BaseResponse baseresponse;

        public BaseClient(string baseAddress, string username, string password)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("http://127.0.0.1:8888"),
                UseProxy = false,
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            var val = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            baseresponse = new BaseResponse();
        }
    }
}
