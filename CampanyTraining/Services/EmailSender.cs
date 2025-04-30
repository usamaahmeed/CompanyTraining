using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CompanyTraining.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _emailFrom = "";
        private readonly string _emailPassword = "";

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailFrom, _emailPassword)
            };

            return client.SendMailAsync(
                new MailMessage(from: _emailFrom, to: email, subject, message)
                {
                    IsBodyHtml = true
                });
        }
    }
}
