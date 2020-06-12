using System;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;

namespace CorumAdminUI.Helpers
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string text = message.Body;
            string html = message.Body;

            //do whatever you want to the message        
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("not-reply@corumsource.com");
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient("mail.corumsource.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("not-reply@corumsource.com", "5YqEAW9H!");
            smtpClient.Credentials = credentials;
            smtpClient.Send(msg);

            return Task.FromResult(0);
        }
    }
}