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
    public class GetApiTendAjax
    {
        public async Task<BaseResponse> GetCallAsync(BaseClient clientbase)
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
}
