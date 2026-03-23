namespace WebApplication4.ConfigurationClass
{
    // Strongly types Configuration class
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";

        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderEmail { get; set; } = string.Empty;

        public string SenderName {  get; set; } = string.Empty;

    }
}
