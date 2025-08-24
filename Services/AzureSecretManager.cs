using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

public class AzureSecretManager : ISecretManager
{
    private readonly SecretClient _client;

    public AzureSecretManager(string keyVaultUrl)
    {
        _client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var secret = await _client.GetSecretAsync(secretName);
        return secret.Value.Value;
    }

    public async Task PutSecretAsync(string secretName, string secretValue)
    {
        await _client.SetSecretAsync(secretName, secretValue);
    }
}
