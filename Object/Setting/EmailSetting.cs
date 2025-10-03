namespace Object.Setting
{
    public class EmailSetting
    {
        public string SenderEmail { get; set; } = default!;
        public string SenderPassword { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int SmtpPort { get; set; }
    }
}
