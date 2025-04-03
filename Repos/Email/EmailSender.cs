using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


public class EmailSender : IEmailSender
{
    private readonly string _smtpServer = "smtp.gmail.com"; 
    private readonly int _smtpPort = 587;
    private readonly string _smtpUsername = "ahmadmostafaa02@gmail.com"; 
    private readonly string _smtpPassword = "fobf hhaa oyvu ehps"; 

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using (var client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}
