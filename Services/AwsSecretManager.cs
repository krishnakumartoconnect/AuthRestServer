using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using System.Threading.Tasks;

public class AwsSecretManager : ISecretManager
{
    private readonly IAmazonSecretsManager _client;

    public AwsSecretManager()
    {
        _client = new AmazonSecretsManagerClient();
    }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    public async Task<string?> GetSecretAsync(string secretName)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    {
        try
        {
            var response = await _client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = secretName });
            return response.SecretString;
        }
        catch
        {
            return null;
        }
    }

    public async Task PutSecretAsync(string secretName, string secretValue)
    {
        try
        {
            await _client.PutSecretValueAsync(new PutSecretValueRequest
            {
                SecretId = secretName,
                SecretString = secretValue
            });
        }
        catch (ResourceNotFoundException)
        {
            await _client.CreateSecretAsync(new CreateSecretRequest
            {
                Name = secretName,
                SecretString = secretValue
            });
        }
    }
}
