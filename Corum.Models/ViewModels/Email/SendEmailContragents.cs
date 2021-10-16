using Corum.Models.Tender;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.ViewModels.Email
{
    public class SendEmailContragents
    {
        public void SendMessage(InfoToContragentsAfterChange model)
        {
            try
            {
                if (model.listWinnersInfoAfterChange != null)
                {
                    foreach (var item in model.listWinnersInfoAfterChange)
                    {
                        SendToContragentsAsync(item as ListInfoAfterChange).GetAwaiter();
                    }
                }
                if (model.listLosersInfoAfterChange != null)
                {
                    foreach (var item in model.listLosersInfoAfterChange)
                    {
                        SendToContragentsAsync(item as ListInfoAfterChange).GetAwaiter();
                    }
                }

            }
            catch (Exception e)
            {

            }
        }

        private async Task SendToContragentsAsync(ListInfoAfterChange model)
        {
            try
            {
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
                MailAddress to = new MailAddress("corumsourcetest@gmail.com");
                using (MailMessage mail = new MailMessage(from, to))
                {
                    mail.Subject = $"№{model.tenderNumber}, ({model.orderId}) {model.routeShort}, погрузка {model.dataDownload.ToString("yyyy-MM-dd")}";
                    mail.Body = $"{model.bodyHTML}";
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
                    smtp.EnableSsl = false;
                    NetworkCredential networkCredential = new NetworkCredential(from.Address, ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]);
                    await Task.Run(() => { smtp.Send(mail); });
                }
            }

            catch (Exception e)
            {

            }
        }
    }
}
