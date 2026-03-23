using Microsoft.Extensions.Options;
using WebApplication4.ConfigurationClass;

namespace WebApplication4.Service
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)

        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine($"Send Email via {_settings.SmtpServer}{_settings.SenderName} " +
                $"{_settings.SenderEmail}");
        }
    }
}
