using System.Net;
using System.Net.Mail;
public class EmailSender
{
    public void SendEmailViaSmtp(string toEmail, string subject, string body)
    {
        var fromAddress = new MailAddress("pharmacare.network@gmail.com", "Pharma Care");
        var toAddress = new MailAddress(toEmail);
        const string fromPassword = "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"; // Be cautious with hardcoding passwords

        var smtp = new SmtpClient
        {
            Host = "smtp.sendgrid.net", // Replace with your SMTP host
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("apikey", "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") // "apikey" is the username for SendGrid SMTP
        };

        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        })
        {
            smtp.Send(message);
        }
    }
}