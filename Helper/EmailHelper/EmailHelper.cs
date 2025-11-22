using Helper.NLog;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Object.Model;
using Object.Setting;

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
                "Hệ thống quản lý",
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

    public async Task<string> SendEvenEmail(List<Customer> customerList, string title, string content)
    {
        int count = customerList.Count;
        int successCount = 0;
        try
        {
            foreach (Customer customer in customerList)
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(
                    "Thông báo sự kiện",
                    _settings.SenderEmail));

                email.To.Add(new MailboxAddress(string.Empty, customer.Email));
                email.Subject = title;
                email.Body = new TextPart("plain")
                {
                    Text = $"Kính gửi quý khách hàng {customer.Name},{Environment.NewLine}{Environment.NewLine}" +
                           $"{content}{Environment.NewLine}{Environment.NewLine}" +
                           "Trân trọng," + Environment.NewLine +
                           "Hệ thống thông báo khách hàng" + Environment.NewLine +
                           "Hotline: 123456"
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
                successCount++;
            }

        }
        catch (Exception ex)
        {
            BaseNLog.logger.Error($"Error sending email: {ex.Message}");
        }
        return $"Đã gửi email thành công đến {successCount} trên tổng số {count} khách hàng.";
    }
}
