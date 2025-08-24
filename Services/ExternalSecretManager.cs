using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ExternalSecretManager : ISecretManager
{
    private readonly HttpClient _http;

    public ExternalSecretManager()
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri(Environment.GetEnvironmentVariable("EXTERNAL_WALLET_URL") ?? "https://your-wallet-api/")
        };
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var response = await _http.GetAsync($"secrets/{secretName}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task PutSecretAsync(string secretName, string secretValue)
    {
        var content = JsonContent.Create(new { value = secretValue });
        var response = await _http.PostAsync($"secrets/{secretName}", content);
        response.EnsureSuccessStatusCode();
    }
}
