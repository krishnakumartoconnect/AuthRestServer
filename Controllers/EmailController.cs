using EWS.Models;
using EWS.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EWS.Controllers
{
    [ApiController]
    [Route("api/2fa/email")]
    public class EmailController : ControllerBase
    {
        private static Dictionary<string, User> users = Services.UserStore.Users;
        private readonly EmailService _emailService;
        private readonly OtpService _otpService;

        public EmailController(EmailService emailService, OtpService otpService)
        {
            _emailService = emailService;
            _otpService = otpService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] dynamic data)
        {
            string userId = data.userId;
            var otp = _otpService.GenerateRandomOtp();
            users[userId].EmailOTP = otp;
            await _emailService.SendOtp(users[userId].Email, otp);
            return Ok(new { sent = true });
        }

        [HttpPost("verify")]
        public IActionResult Verify([FromBody] dynamic data)
        {
            string userId = data.userId;
            string code = data.code;
            bool valid = users[userId].EmailOTP == code;
            return Ok(new { success = valid });
        }
    }
}
