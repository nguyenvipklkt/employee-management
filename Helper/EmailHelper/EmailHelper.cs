using Helper.NLog;
using MailKit.Net.Smtp;
using MimeKit;
using Object.Setting;
using Microsoft.Extensions.Options;

namespace Helper.EmailHelper;

public class EmailHelper
{
    private readonly EmailSetting _settings;

    public EmailHelper(IOptions<EmailSetting> options)
    {
        _settings = options.Value;
    }

    public async Task<bool> SendVerificationEmail(string recipientEmail, string verificationCode)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(
                "Nhà hàng Uyên Khanh",
                _settings.SenderEmail));

            email.To.Add(new MailboxAddress(string.Empty, recipientEmail));
            email.Subject = "Xác thực tài khoản người dùng";
            email.Body = new TextPart("plain")
            {
                Text = $"Mã code của bạn là: {verificationCode} " +
                       "(Vui lòng không chia sẻ mã xác thực cho người khác)"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _settings.SmtpServer,
                _settings.SmtpPort,
                false);

            await smtp.AuthenticateAsync(
                _settings.SenderEmail,
                _settings.SenderPassword);

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
