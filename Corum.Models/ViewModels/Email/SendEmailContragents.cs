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
                        SendToContragents(item as ListInfoAfterChange);
                    }
                }
                if (model.listLosersInfoAfterChange != null)
                {
                    foreach (var item in model.listLosersInfoAfterChange)
                    {
                        SendToContragents(item as ListInfoAfterChange);
                    }
                }

            }
            catch (Exception e)
            {

            }
        }

        private void SendToContragents(ListInfoAfterChange model)
        {
            try
            {
                string from = ConfigurationManager.AppSettings["SmtpAccountLogin"];
                using (MailMessage mail = new MailMessage(from, "corumsourcetest@gmail.com"))
                {
                    mail.Subject = (model.count > 1) ? $"{model.expeditorName}(предложения {model.count})" : $"{model.expeditorName}(предложений {model.count})";
                    mail.Body = $"{model.upperartOfTheMessage}<br>{model.dataTable}<br>{model.messageFooter}";
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
                    smtp.EnableSsl = false;
                    NetworkCredential networkCredential = new NetworkCredential(from, ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]);
                    smtp.Send(mail);
                }
            }

            catch (Exception e)
            {

            }
        }
    }
}
