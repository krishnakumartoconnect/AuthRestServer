
public static class SecretManagerFactory
{
    public static async Task<ISecretManager> CreateAsync(IServiceProvider services)
    {
        string environment = await CloudEnvironmentDetector.DetectEnvironmentAsync();
        Console.WriteLine($"[SecretManagerFactory] Detected environment: {environment}");

        return environment switch
        {
            "AWS" => new AwsSecretManager(),
            "Azure" => new AzureSecretManager("https://your-keyvault-name.vault.azure.net/"),
            "GCP" => new GoogleSecretManager("your-gcp-project-id"),
            "External" => new ExternalSecretManager(),
            _ => throw new Exception("Unknown environment")
        };
    }
}
