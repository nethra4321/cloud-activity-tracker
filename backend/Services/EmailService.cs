using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    public async Task SendNotificationEmailAsync(string subject, string body)
    {
        var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
        var smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
        var senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
        var senderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD");
        var recipientEmail = Environment.GetEnvironmentVariable("RECIPIENT_EMAIL");

        Console.WriteLine(senderEmail);

        var smtpClient = new SmtpClient(smtpHost)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(senderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        mail.To.Add(recipientEmail);

        await smtpClient.SendMailAsync(mail);
    }
}
