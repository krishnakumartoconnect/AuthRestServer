using OtpNet;

namespace EWS.Services
{
    public class OtpService
    {
        public (string Base32Secret, string Uri) GenerateTotpSecret(string email)
        {
            var secretKey = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secretKey);
            var otpUri = new OtpUri(OtpType.Totp, base32Secret, email, "MyApp").ToString();
            return (base32Secret, otpUri);
        }

        public bool VerifyTotp(string secret, string code)
        {
            var bytes = Base32Encoding.ToBytes(secret);
            var totp = new Totp(bytes);
            return totp.VerifyTotp(code, out _);
        }

        public string GenerateRandomOtp() =>
            new Random().Next(100000, 999999).ToString();
    }
}
