using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace drunkShop.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Your App Name", _smtpUser));
            email.To.Add(new MailboxAddress("", emailMessage.RecipientEmail));
            email.Subject = emailMessage.Subject;
            email.Body = new TextPart("plain")
            {
                Text = emailMessage.Body
            };

            using var smtpClient = new SmtpClient();
            try
            {
                // Connect to the SMTP server with STARTTLS (recommended for port 587)
                await smtpClient.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_smtpUser, _smtpPass);
                await smtpClient.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while sending email: {ex.Message}");
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
            }
        }
    }
}
