using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Data;

namespace Corum.Models.ViewModels.Tender
{
    public class GetFileApiTender
    {
        byte[] fileContents;
        public async Task<byte[]> GetCallAsync(BaseClient clientbase)
        {
            HttpClient client = clientbase.client;
            BaseResponse baseresponse = clientbase.baseresponse;
            int count = 0;
            try
            {
                while (count < 10)
                {
                    baseresponse.response = client.GetAsync(client.BaseAddress).Result;
                    if (baseresponse.response.IsSuccessStatusCode)
                    {
                        baseresponse.ResponseMessage = await baseresponse.response.Content.ReadAsStringAsync();
                        baseresponse.StatusCode = (int)baseresponse.response.StatusCode;

                        using (var stream = new StreamContent(baseresponse.response.Content.ReadAsStreamAsync().Result))
                        {
                            fileContents = stream.ReadAsByteArrayAsync().Result;
                        }
                        count = 10;
                    }
                    else
                    {
                        baseresponse.ResponseMessage = await baseresponse.response.Content.ReadAsStringAsync();
                        baseresponse.StatusCode = (int)baseresponse.response.StatusCode;
                    }
                    ++count;
                }
                return fileContents;
            }
            catch (Exception ex)
            {
                baseresponse.StatusCode = 0;
                baseresponse.ResponseMessage = (ex.Message ?? ex.InnerException.ToString());
            }
            return fileContents;
        }
    }
}
