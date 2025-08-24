public class SecretManagerService
{
    private readonly ISecretManager _manager;

    public SecretManagerService(ISecretManager manager)
    {
        _manager = manager;
    }

    public Task<string> GetSecretAsync(string name) => _manager.GetSecretAsync(name);
    public Task PutSecretAsync(string name, string value) => _manager.PutSecretAsync(name, value);
}
