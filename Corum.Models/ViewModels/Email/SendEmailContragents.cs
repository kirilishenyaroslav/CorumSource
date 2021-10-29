using Corum.Models.Tender;
using Corum.Models.ViewModels.Tender;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Corum.Models.ViewModels.Email
{
    public class SendEmailContragents
    {
        //protected Corum.Models.ICorumDataProvider context;
        //public void SendMessage(InfoToContragentsAfterChange model)
        //{
        //    try
        //    {
        //        if (model.listWinnersInfoAfterChange != null)
        //        {
        //            context = DependencyResolver.Current.GetService<ICorumDataProvider>();
        //            foreach (var item in model.listWinnersInfoAfterChange)
        //            {
        //                DataToAndFromContragent instance = new DataToAndFromContragent();
        //                context.NewUsedCarExcel(item.formUuid, ref instance);
        //                int orderId = (int)item.orderId;
        //                SendToWinnerContragentsAsync(item as ListInfoAfterChange).GetAwaiter();
        //            }
        //        }
        //        if (model.listLosersInfoAfterChange != null)
        //        {
        //            foreach (var item in model.listLosersInfoAfterChange)
        //            {
        //                SendToLoserContragentsAsync(item as ListInfoAfterChange).GetAwaiter();
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //private async Task SendToLoserContragentsAsync(ListInfoAfterChange model)
        //{
        //    try
        //    {
        //        MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
        //        MailAddress to = new MailAddress("corumsourcetest@gmail.com");
        //        using (MailMessage mail = new MailMessage(from, to))
        //        {
        //            mail.Subject = $"№{model.tenderNumber}, ({model.orderId}) {model.routeShort}, погрузка {model.dataDownload.ToString("yyyy-MM-dd")}";
        //            mail.Body = $"{model.bodyHTML}";
        //            mail.IsBodyHtml = true;
        //            SmtpClient smtp = new SmtpClient();
        //            smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
        //            smtp.EnableSsl = false;
        //            NetworkCredential networkCredential = new NetworkCredential(from.Address, ConfigurationManager.AppSettings["SmtpAccountPassw"]);
        //            smtp.UseDefaultCredentials = false;
        //            smtp.Credentials = networkCredential;
        //            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]);
        //            await Task.Run(() => { smtp.Send(mail); });
        //        }
        //    }

        //    catch (Exception e)
        //    {

        //    }
        //}
        //private async Task SendToWinnerContragentsAsync(ListInfoAfterChange model)
        //{
        //    try
        //    {
        //        MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
        //        MailAddress to = new MailAddress("corumsourcetest@gmail.com");
        //        using (MailMessage mail = new MailMessage(from, to))
        //        {
        //            mail.Subject = $"№{model.tenderNumber}, ({model.orderId}) {model.routeShort}, погрузка {model.dataDownload.ToString("yyyy-MM-dd")}";
        //            mail.Body = $"{model.bodyHTML}";
        //            mail.IsBodyHtml = true;
        //            MemoryStream stream = new MemoryStream();
        //            byte[] mBArray = new ExportToExcelController().OrderAsExcelFromContragent(orderId, Request.UserLanguages[0], data);
        //            stream = new MemoryStream(mBArray, false);
        //            var reportName = "OrderReport " + orderId.ToString() + ".xlsx";
        //            var attacment = new Attachment(stream, reportName);
        //            attacment.ContentType = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //            mail.Attachments.Add(attacment);
        //            SmtpClient smtp = new SmtpClient();
        //            smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
        //            smtp.EnableSsl = false;
        //            NetworkCredential networkCredential = new NetworkCredential(from.Address, ConfigurationManager.AppSettings["SmtpAccountPassw"]);
        //            smtp.UseDefaultCredentials = false;
        //            smtp.Credentials = networkCredential;
        //            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]);
        //            await Task.Run(() => { smtp.Send(mail); });
        //        }
        //    }

        //    catch (Exception e)
        //    {

        //    }
        //}
    }
}
