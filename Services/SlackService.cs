using System.Net.Http.Headers;

namespace EWS.Services
{
    public class SlackService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient = new();

        public SlackService(IConfiguration config) => _config = config;

        public async Task SendOtp(string slackUserId, string otp)
        {
            var payload = new
            {
                channel = slackUserId,
                text = $"Your OTP is: {otp}"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/chat.postMessage")
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config["Slack:BotToken"]);
            await _httpClient.SendAsync(request);
        }
    }
}
