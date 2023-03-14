using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
#region smtp
//using System.Net.Mail;
//using System.Net.Mime;
#endregion

namespace OnlineShop.Service.Services.FileExcute
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<SenderSettings> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public SenderSettings Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if(Options.AuthMessageSenderOptions!.Method == "smtp")
            {
                if (string.IsNullOrEmpty(Options.AuthMessageSenderOptions?.SmtpServer))
                {
                    throw new Exception("Null SmtpServer");
                }
                if (string.IsNullOrEmpty(Options.AuthMessageSenderOptions?.SmtpUserID))
                {
                    throw new Exception("Null SmtpUserID");
                }
                if (string.IsNullOrEmpty(Options.AuthMessageSenderOptions?.SmtpPass))
                {
                    throw new Exception("Null SmtpPass");
                }
                if (Options.AuthMessageSenderOptions.SmtpPort < 0)
                {
                    throw new Exception("SmtpPort < 0");
                }
                await SmptExecute(subject, message, toEmail);
            }
            else if (Options.AuthMessageSenderOptions!.Method == "sendgridapi")
            {
                if (string.IsNullOrEmpty(Options.AuthMessageSenderOptions?.SendGridKey))
                {
                    throw new Exception("Null SendGridKey");
                }
                await Execute(Options.AuthMessageSenderOptions.SendGridKey, subject, message, toEmail);
            }      
        }

        #region sendgrid
        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SenderEmail, "Auth Service"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail, "Client Email"));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
        #endregion

        #region smtp

        public async Task SmptExecute(string subject, string message, string toEmail)
        {
            var smtpServer = Options.AuthMessageSenderOptions?.SmtpServer;
            int smtpPort = Options.AuthMessageSenderOptions!.SmtpPort;
            var userID = Options.AuthMessageSenderOptions?.SmtpUserID;
            var userPass = Options.AuthMessageSenderOptions?.SmtpPass;
            var client = new SmtpClient();
            client.Connect(smtpServer, smtpPort, SecureSocketOptions.Auto);
            {
                // MailKit におけるメールの情報
                var mimeMessage = new MimeMessage();

                // 送り元情報  
                //message.From.Add(MailboxAddress.Parse(from));
                mimeMessage.From.Add(MailboxAddress.Parse(userID));

                mimeMessage.To.Add(MailboxAddress.Parse(toEmail));

                // 表題  
                mimeMessage.Subject = subject;

                // 内容  
                var textFormat = TextFormat.Html;
                var textPart = new TextPart(textFormat)
                {
                    Text = message,
                };
                mimeMessage.Body = textPart;

                if (string.IsNullOrEmpty(userID) == false)
                {
                    // SMTPサーバ認証  
                    await client.AuthenticateAsync(userID, userPass);
                }

                // 送信  
                await client.SendAsync(mimeMessage);

                // 切断  
                await client.DisconnectAsync(true);
            }
        }
        #endregion
    }
}
