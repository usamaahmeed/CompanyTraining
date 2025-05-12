using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace CompanyTraining.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _emailSettings;

        public EmailSender(IOptions<EmailSetting> emailSettings)
        {
            this._emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail,_emailSettings.Password);
                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = message
                };
                mail.To.Add(email);
                await client.SendMailAsync(mail);
            }
             
        }
    }
}
