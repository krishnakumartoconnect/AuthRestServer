namespace EWS.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TOTPSecret { get; set; }
        public bool Is2FAEnabled { get; set; }
        public string Preferred2FAMethod { get; set; } // "totp", "email", "slack"
        public string EmailOTP { get; set; }
        public string SlackOTP { get; set; }
        public string SlackUserId { get; set; }
    }
}
