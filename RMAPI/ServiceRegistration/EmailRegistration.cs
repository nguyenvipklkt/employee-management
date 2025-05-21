using Helper.NLog;
using MailKit.Net.Smtp;
using MimeKit;

namespace RMAPI.ServiceRegistration
{
    public class EmailRegistration
    {
        private readonly IConfiguration _config;

        public EmailRegistration(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendVerificationEmail(string recipientEmail, string verificationCode)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Nhà hàng Uyên Khanh", _config["EmailSettings:SenderEmail"]));
                email.To.Add(new MailboxAddress("", recipientEmail));
                email.Subject = "Xác thực tài khoản người dùng";

                email.Body = new TextPart("plain")
                {
                    Text = $"Mã code của bạn là: {verificationCode} (Vui lòng không chia sẻ mã xác thực cho người khác)"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:SmtpPort"] ?? "587"), false);
                await smtp.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderPassword"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
