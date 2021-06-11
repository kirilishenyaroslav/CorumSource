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
    public class PostApiTender<T> where T: new()
    {
        public async Task<BaseResponse> GetCallAsync(BaseClient clientbase, object postValues, int number)
        {
            HttpClient client = clientbase.client;
            BaseResponse baseresponse = clientbase.baseresponse;
            int count = 0;
            try
            {
                while (count < 10)
                {
                    switch (number)
                    {
                        case 0: baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postValues as TenderForma<PropAliasValuesOne>).Result; break;
                        case 1: baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postValues as TenderForma<PropAliasValuesOne>).Result; break;
                        case 2: baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postValues as TenderForma<PropAliasValuesTwo>).Result; break;
                        case 3: baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postValues as TenderForma<PropAliasValuesThree>).Result; break;
                        case 4: baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postValues as TenderForma<PropAliasValuesFour>).Result; break;
                        case 5: baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postValues as TenderForma<PropAliasValuesFive>).Result; break;
                        default: baseresponse.response = client.PostAsJsonAsync(client.BaseAddress, postValues as TenderForma<PropAliasValuesOne>).Result; break;
                    }
                   
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
