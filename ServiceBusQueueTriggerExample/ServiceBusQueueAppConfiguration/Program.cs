using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder =>
    {
        // You might need this depending on your local dev environment
        // var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true });
        var credential = new DefaultAzureCredential();

        // -------------------------------------Start: App Configuration Example
        // Required NuGet packages for Azure App Configuration:
        // 1. Azure.Identity
        // 2. Microsoft.Extensions.Configuration.AzureAppConfiguration
        // Docs: https://docs.microsoft.com/en-us/azure/azure-app-configuration/quickstart-azure-functions-csharp?tabs=isolated-process#connect-to-an-app-configuration-store

        var azureAppConfigurationEndpoint = Environment.GetEnvironmentVariable("AzureAppConfigurationEndpoint");

        if (string.IsNullOrWhiteSpace(azureAppConfigurationEndpoint) == false)
        {
            builder.AddAzureAppConfiguration(options =>
            {
                // Key Vault access: https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-key-vault-references-dotnet-core?tabs=core5x#update-your-code-to-use-a-key-vault-reference
                options.Connect(new Uri(azureAppConfigurationEndpoint), credential);

                options.ConfigureKeyVault(keyVaultOptions =>
                  {
                      keyVaultOptions.SetCredential(credential);
                  });
            });
        }
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
