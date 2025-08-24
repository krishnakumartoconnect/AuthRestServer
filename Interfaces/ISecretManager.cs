public interface ISecretManager
{
    Task<string> GetSecretAsync(string secretName);
    Task PutSecretAsync(string secretName, string secretValue);
}
