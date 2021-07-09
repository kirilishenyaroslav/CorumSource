using Corum.Models;
using Corum.Models.Tender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Threading;
using Corum.Models.ViewModels.Tender;
using System.Collections.Specialized;
using System.Configuration;
using Newtonsoft.Json;

namespace CorumAdminUI.HangFireTasks
{
    public class HangFireTasks
    {
        protected Corum.Models.ICorumDataProvider context;
        List<RegisterTenders> listRegistryTenders;
        ParallelOptions options;
        List<Task> listTask;
        NameValueCollection allAppSettings;

        public HangFireTasks()
        {
            listRegistryTenders = new List<RegisterTenders>();
            context = DependencyResolver.Current.GetService<ICorumDataProvider>();
            options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Environment.ProcessorCount > 3 ? Environment.ProcessorCount - 1 : 1;
            listTask = new List<Task>();
            allAppSettings = ConfigurationManager.AppSettings;
        }
        public async Task AsyncStatusTender(RegisterTenders item)
        {
            int numberTender = item.tenderNumber;
            await Task.Factory.StartNew(() => Parallel.Invoke(options, () => GetStatusTender(numberTender)));
        }

        public async Task AsyncGetInfoTenderId(RegisterTenders item)
        {
            int numberTender = item.tenderNumber;
            await Task.Factory.StartNew(() => Parallel.Invoke(options, () => GetTenderInfoId(numberTender)));
        }

        public void GetStatusTender(int numberTender)
        {
            try
            {
                BaseClient clientbase = new BaseClient($"{allAppSettings["ApiGetStatusTender"]}{numberTender}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var JSONresponse = new GetApiTender().GetCallAsync(clientbase).Result.ResponseMessage;
                ResponseStatusTender myDeserializedClass = JsonConvert.DeserializeObject<ResponseStatusTender>(JSONresponse);
                DateTime dateUpdateStatus = DateTime.Now;
                if (myDeserializedClass.success)
                {
                    if (myDeserializedClass.data.process > 7)
                    {
                        try
                        {
                            BaseClient client = new BaseClient($"{allAppSettings["ApiGetTenderId"]}{numberTender}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                            var response = new GetApiTender().GetCallAsync(client).Result.ResponseMessage;
                            RequestJSONDeserializedToModel resultDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(response);
                            context.UpdateStatusRegisterTender(numberTender, myDeserializedClass.data.process, dateUpdateStatus, resultDeserializedClass);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    else
                    {
                        context.UpdateStatusRegisterTender(numberTender, myDeserializedClass.data.process, dateUpdateStatus, null);
                    }
                }
            }
            catch { }
        }

        public void GetTenderInfoId(int numberTender)
        {
            try
            {
                BaseClient clientbase = new BaseClient($"{allAppSettings["ApiGetTenderId"]}{numberTender}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var JSONresponse = new GetApiTender().GetCallAsync(clientbase).Result.ResponseMessage;
                RequestJSONDeserializedToModel myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(JSONresponse);
                if (myDeserializedClass.success)
                {
                    context.UpdateTimeRemainingTime(myDeserializedClass, numberTender);
                }
            }
            catch { }
        }

        public async Task ListTasks()
        {
            listRegistryTenders = context.GetRegisterTenders();
            foreach (var item in listRegistryTenders)
            {
                if (item.processValue != "Завершен")
                {
                    await Task.Run(() => AsyncGetInfoTenderId(item));
                    await Task.Run(() => AsyncStatusTender(item));
                }
            }

        }

    }
}