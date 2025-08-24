using System;

public static class CloudEnvironmentDetector
{
    internal static async Task<string> DetectEnvironmentAsync()
    {
        if (Environment.GetEnvironmentVariable("USE_EXTERNAL_WALLET") == "true")
            return "External";

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_REGION")))
            return "AWS";

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT")))
            return "GCP";

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_TENANT_ID")))
            return "Azure";

        if (await IsAwsMetadata()) return "AWS";
        if (await IsAzureMetadata()) return "Azure";
        if (await IsGcpMetadata()) return "GCP";

        return "Unknown";
    }

    private static async Task<bool> IsAwsMetadata()
    {
        const string awsMetadataUrl = "http://169.254.169.254/latest/meta-data/";

        try
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(1000)
            };

            var response = await client.GetAsync(awsMetadataUrl);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<bool> IsAzureMetadata()
    {
        const string azureMetadataUrl = "http://169.254.169.254/metadata/instance?api-version=2021-02-01";

        try
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(1000)
            };

            // Azure requires a Metadata header
            client.DefaultRequestHeaders.Add("Metadata", "true");

            var response = await client.GetAsync(azureMetadataUrl);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
    
    private static async Task<bool> IsGcpMetadata()
    {
        const string gcpMetadataUrl = "http://169.254.169.254/computeMetadata/v1/";

        try
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(1000)
            };

            // GCP requires this header
            client.DefaultRequestHeaders.Add("Metadata-Flavor", "Google");

            var response = await client.GetAsync(gcpMetadataUrl);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
