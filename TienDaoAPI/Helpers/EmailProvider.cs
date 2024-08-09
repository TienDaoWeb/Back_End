using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Helpers
{
    public class EmailProvider : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        private readonly ILogger<EmailProvider> _logger;

        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public EmailProvider(IOptions<MailSettings> mailSettings, ILogger<EmailProvider> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
            _logger.LogInformation("Create SendMailService");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            _logger.LogInformation("send mail to: " + email);

        }

        public async Task SendEmailWithTemplateAsync(string email, string subject, string templatePath, object data)
        {
            var template = File.ReadAllText(templatePath);

            foreach (var prop in data.GetType().GetProperties())
            {
                template = template.Replace($"{{{prop.Name}}}", prop.GetValue(data)?.ToString());
            }

            await SendEmailAsync(email, subject, template);
        }
    }
}
