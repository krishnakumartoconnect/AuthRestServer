using System.Collections.Generic;
using EWS.Models;

namespace EWS.Services
{
    public static class UserStore
    {
        // Simulated in-memory user store (used for demo/testing)
        public static Dictionary<string, User> Users = new Dictionary<string, User>
        {
            {
                "user123", new User
                {
                    Id = "user123",
                    Email = "demo@example.com",
                    Password = "password123", // ⚠️ Store hashes in real apps
                    TOTPSecret = "",
                    Is2FAEnabled = true,
                    Preferred2FAMethod = "totp", // or "email", "slack"
                    EmailOTP = null,
                    SlackOTP = null,
                    SlackUserId = "U12345678" // Your Slack user ID here
                }
            }
        };
    }
}
