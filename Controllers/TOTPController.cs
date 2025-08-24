using EWS.Models;
using EWS.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using QRCoder;

/*public class TOTPController : Controller
{
    public IActionResult Setup()
    {
        // Replace with your actual user data
        string email = "user@example.com";
        string appName = "MySecureApp";

        // Generate a 20-byte secret
        byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
        string base32Secret = Base32Encoding.ToString(secretKey); // Save this in the DB

        // Create otpauth URI
        string otpUri = $"otpauth://totp/{appName}:{email}?secret={base32Secret}&issuer={appName}";

        // Generate QR code image
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q);
        Base64QRCode qrCode = new Base64QRCode(qrCodeData);
        string qrCodeImageBase64 = qrCode.GetGraphic(20);

        // Pass data to the view
        ViewBag.QrCodeImage = $"data:image/png;base64,{qrCodeImageBase64}";
        ViewBag.Secret = base32Secret;

        return View();
    }

    [HttpPost]
    public IActionResult VerifyLogin(string email, string password, string mfaCode)
    {
        //var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        var user = "user";
        if (user == null)
            return Unauthorized();

        if (!string.IsNullOrEmpty(user.MfaSecret))
        {
            var totp = new Totp(Base32Encoding.ToBytes(user.MfaSecret));
            if (!totp.VerifyTotp(mfaCode, out _, new VerificationWindow(2, 2)))
            {
                ModelState.AddModelError("", "Invalid MFA code.");
                return View("Login");
            }
        }

        // Log in the user
        // e.g., SignInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult MfaPrompt(string code)
    {
        var user = GetUser(); // Get logged-in user
        var secret = user.MfaSecret;

        var totp = new Totp(Base32Encoding.ToBytes(secret));
        if (totp.VerifyTotp(code, out _, new VerificationWindow(1, 1)))
        {
            // Log the user in fully
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid code.");
        return View();
    }
}*/

[ApiController]
[Route("api/2fa/totp")]
public class TOTPController : ControllerBase
{
    private static Dictionary<string, User> users = EWS.Services.UserStore.Users;
    private readonly OtpService _otpService;

    public TOTPController(OtpService otpService) => _otpService = otpService;

    [HttpGet("setup/{userId}")]
    public IActionResult GetSetup(string userId)
    {
        var user = users[userId];
        var (secret, uri) = _otpService.GenerateTotpSecret(user.Email);
        user.TOTPSecret = secret;
        return Ok(new { secret, uri });
    }

    [HttpPost("verify")]
    public IActionResult VerifySetup([FromBody] dynamic data)
    {
        string userId = data.userId;
        string code = data.code;
        string secret = data.secret;

        var valid = _otpService.VerifyTotp(secret, code);
        if (valid)
        {
            users[userId].TOTPSecret = secret;
            users[userId].Is2FAEnabled = true;
        }
        return Ok(new { success = valid });
    }

    [HttpPost("verify-login")]
    public IActionResult VerifyLogin([FromBody] dynamic data)
    {
        string userId = data.userId;
        string code = data.code;
        var user = users[userId];

        var valid = _otpService.VerifyTotp(user.TOTPSecret, code);
        return Ok(new { success = valid });
    }
}
