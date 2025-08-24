using EWS.Models;
using EWS.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EWS.Controllers
{
    [ApiController]
    [Route("api/2fa/slack")]
    public class SlackController : ControllerBase
    {
        private static Dictionary<string, User> users = Services.UserStore.Users;
        private readonly SlackService _slackService;
        private readonly OtpService _otpService;

        public SlackController(SlackService slackService, OtpService otpService)
        {
            _slackService = slackService;
            _otpService = otpService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] dynamic data)
        {
            string userId = data.userId;
            var otp = _otpService.GenerateRandomOtp();
            users[userId].SlackOTP = otp;
            await _slackService.SendOtp(users[userId].SlackUserId, otp);
            return Ok(new { sent = true });
        }

        [HttpPost("verify")]
        public IActionResult Verify([FromBody] dynamic data)
        {
            string userId = data.userId;
            string code = data.code;
            bool valid = users[userId].SlackOTP == code;
            return Ok(new { success = valid });
        }
    }
}
