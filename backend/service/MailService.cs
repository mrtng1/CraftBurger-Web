using MimeKit;

namespace service;

public class MailService
{
    public void sendMail(string to, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("easv.webshop@gmail.com");
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            // Change here to send HTML email
            email.Body = new TextPart("html") { Text = body };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 465, true);
                smtp.Authenticate("easv.webshop@gmail.com", "wssrjeqiusxdpqra");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while sending email: {ex.Message}");
        }
    }
}