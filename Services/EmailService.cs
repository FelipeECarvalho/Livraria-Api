using System.Net;
using System.Net.Mail;

namespace Livraria.Services
{
    public class EmailService
    {
        public bool Send(
            string toName,
            string toEmail,
            string subject,
            string body,
            string fromName = "Emanuel's Softwares",
            string fromEmail = ""
            ) 
        {
            var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.Username, Configuration.Smtp.Password);
            smtpClient.EnableSsl = true;

            var mail = new MailMessage();

            mail.From = new MailAddress(fromEmail, fromName);
            mail.To.Add(new MailAddress(toEmail, toName));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            try 
            {
                smtpClient.Send(mail);
                return true;
            }
            catch(Exception)
            {
                return false;            
            }
        }
    }
}
