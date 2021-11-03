﻿using CorumAdminUI.Common;
using Corum.Models.ViewModels.Cars;
using System;
using Corum.Models.ViewModels;
using Corum.Common;
using Corum.Models.Tender;
using Corum.Models.ViewModels.Tender;
using System.Collections;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using CorumAdminUI;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;
using Corum.ReportsUI;
using System.Threading.Tasks;

namespace CorumAdminUI.Controllers
{
    public class FormContragentsController : CorumBaseController
    {
        private static string htmlBodyForm { get; set; }
        private static string subject { get; set; }
        private static MemoryStream excStreamFile { get; set; }
        private static bool flag { get; set; }

        [HttpGet]
        public ActionResult SendFormToCorumSource(Guid formUuid)
        {
            if (context.CheckFormUuid(formUuid))
            {
                List<RegisterFormFromContragents> listDataToForm = new List<RegisterFormFromContragents>();
                listDataToForm = context.GetRegisterFormFromContragents(formUuid);
                return View(listDataToForm);
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        [HttpPost]
        public ActionResult SendBodyHtml(BodyHtmlForm bodyHtml)
        {
            htmlBodyForm = bodyHtml.body;
            subject = bodyHtml.subject;

            return new JsonpResult
            {
                Data = new { },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonpResult SendDataFromForm()
        {
            flag = false;
            bool error = false;
            try
            {
                List<HttpPostedFileBase> listFiles = new List<HttpPostedFileBase>();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var key in Request.Form.AllKeys)
                {
                    dic[key] = Request.Form[key];
                }

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    listFiles.Add(file);
                }
                if (context.SetRegisterFormFromContragent(listFiles, dic))
                {
                    DataToAndFromContragent data = new DataToAndFromContragent();
                    int orderId = context.NewUsedCar(Guid.Parse(dic["tenderItemUuid"]), ref data);

                    List<string> listEmails = new List<string>()
                        {
                        "Litovchenko.Sergey@corum.com"
                        //"corumsourcetest@gmail.com"
                        };
                    if (data.regmesstocontrag != null && data.regmesstocontrag.emailOperacionist.Contains('@'))
                    {
                        listEmails.Add(data.regmesstocontrag.emailOperacionist);
                    }

                    foreach (var value in listEmails)
                    {
                        Task.WaitAll(Task.Run(() => SendToOperacionistAsync(listFiles, dic, value)));
                    }
                }
            }
            catch (Exception e)
            {
                error = true;
            }

            htmlBodyForm = null;
            return new JsonpResult
            {
                Data = new { flag, error },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private async Task SendToOperacionistAsync(List<HttpPostedFileBase> listFiles, Dictionary<string, string> dic, string emailOperacionist)
        {
            try
            {
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], $"{dic["contragentName"]}", Encoding.UTF8);
                MailAddress to = new MailAddress(emailOperacionist);
                using (MailMessage mail = new MailMessage(from, to))
                {
                    mail.Subject = $"{subject}";
                    mail.Body = $"{htmlBodyForm}";
                    mail.IsBodyHtml = true;
                    if (listFiles.Count != 0)
                    {
                        foreach (var item in listFiles)
                        {
                            string fileName = Path.GetFileName(item.FileName);
                            mail.Attachments.Add(new Attachment(item.InputStream, fileName));
                        }
                    }
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
                    smtp.EnableSsl = false;
                    NetworkCredential networkCredential = new NetworkCredential(from.Address, ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]);
                    if (true)
                    {
                        await Task.Run(() =>
                        {
                            smtp.Send(mail);
                            flag = true;
                        });
                    }
                }
            }

            catch (Exception e)
            {

            }
        }

        private static string SplitEncodedAttachmentName(string encoded)
        {
            const string encodingtoken = "=?UTF-8?B?";
            const string softbreak = "?=";
            const int maxChunkLength = 30;
            int splitLength = maxChunkLength - encodingtoken.Length - (softbreak.Length * 2);
            IEnumerable<string> parts = SplitByLength(encoded, splitLength);
            string encodedAttachmentName = encodingtoken;
            foreach (var part in parts)
            {
                encodedAttachmentName += part + softbreak + encodingtoken;
            }
            encodedAttachmentName = encodedAttachmentName.Remove(encodedAttachmentName.Length - encodingtoken.Length, encodingtoken.Length);
            return encodedAttachmentName;
        }

        private static IEnumerable<string> SplitByLength(string stringToSplit, int length)
        {
            while (stringToSplit.Length > length)
            {
                yield return stringToSplit.Substring(0, length);
                stringToSplit = stringToSplit.Substring(length);
            }
            if (stringToSplit.Length > 0)
            {
                yield return stringToSplit;
            }
        }
    }
}