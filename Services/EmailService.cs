using System.Net;
using System.Net.Mail;

namespace EWS.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) => _config = config;

        public async Task SendOtp(string toEmail, string otp)
        {
            var smtp = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
                EnableSsl = true
            };

            var message = new MailMessage(_config["Smtp:From"], toEmail, "Your OTP", $"Your OTP is: {otp}");
            await smtp.SendMailAsync(message);
        }
    }
}
