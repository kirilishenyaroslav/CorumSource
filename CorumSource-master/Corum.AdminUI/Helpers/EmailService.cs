using System;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels;

namespace CorumAdminUI.Helpers
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string text = message.Body;
            string html = message.Body;


            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"], Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpAccountLogin"], ConfigurationManager.AppSettings["SmtpAccountPassw"]);
            smtpClient.Credentials = credentials;
            smtpClient.Send(msg);

            return Task.FromResult(0);
        }
    }

    public class CorumEmailService 
    {
        public Task SendOrderNotificationsAsync (OrderNotificationsMessage message, List<OrderObserverViewModel> recievers)
        {
            foreach(var reciever in recievers)
            {
                string text = message.Body;
                string html = message.Body;

                   
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
                msg.To.Add(new MailAddress(reciever.observerEmail));
                msg.Subject = message.Subject;
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"], Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpAccountLogin"], ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                smtpClient.Credentials = credentials;
                smtpClient.Send(msg);


            }

            return Task.FromResult(0);
        }


        public Task SendRequestToEmailAsync(OrderNotificationsMessage message, List<UserViewModel> recievers)
        {
            foreach (var reciever in recievers)
            {                
                string html = message.Body;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
                msg.To.Add(new MailAddress(reciever.userEmail));
                msg.Subject = message.Subject;
                
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"], Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpAccountLogin"], ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                smtpClient.Credentials = credentials;
                smtpClient.Send(msg);
            }

            return Task.FromResult(0);
        }

        public Task SendRequestToEmailAsync(OrderNotificationsMessage message, List<OrderObserverViewModel> recievers)
        {
            foreach (var reciever in recievers)
            {
                string html = message.Body;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
                msg.To.Add(new MailAddress(reciever.observerEmail));
                msg.Subject = message.Subject;

                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"], Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpAccountLogin"], ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                smtpClient.Credentials = credentials;
                smtpClient.Send(msg);
            }

            return Task.FromResult(0);
        }
    }
}