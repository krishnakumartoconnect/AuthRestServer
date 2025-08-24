using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Google.Protobuf;
using System;
using System.Threading.Tasks;

public class GoogleSecretManager : ISecretManager
{
    private readonly SecretManagerServiceClient _client;
    private readonly string _projectId;

    public GoogleSecretManager(string projectId)
    {
        _client = SecretManagerServiceClient.Create();
        _projectId = projectId;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var secretVersion = new SecretVersionName(_projectId, secretName, "latest");
        var result = await _client.AccessSecretVersionAsync(secretVersion);
        return result.Payload.Data.ToStringUtf8();
    }

    public async Task PutSecretAsync(string secretName, string secretValue)
    {
        var parent = new ProjectName(_projectId);

        try
        {
            await _client.GetSecretAsync(new SecretName(_projectId, secretName));
        }
        catch
        {
            await _client.CreateSecretAsync(parent, secretName, new Secret
            {
                Replication = new Replication { Automatic = new Replication.Types.Automatic() }
            });
        }

        await _client.AddSecretVersionAsync(new SecretName(_projectId, secretName),
            new SecretPayload { Data = ByteString.CopyFromUtf8(secretValue) });
    }
}
