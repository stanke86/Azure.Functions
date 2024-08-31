using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueExample.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder =>
    {
        if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
        {
            builder.AddUserSecrets<Program>();
        }


        // You might need this depending on your local dev environment
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true });
        //var credential = new DefaultAzureCredential();

        // Microsoft's current guidance for using Key Vault in an Azure Functions is to use the reference notation
        // https://docs.microsoft.com/en-us/azure/app-service/app-service-key-vault-references?toc=%2Fazure%2Fazure-functions%2Ftoc.json&tabs=azure-cli#reference-syntax
        // which looks like this (note how I did NOT include the version):
        // @Microsoft.KeyVault(SecretUri=https://myvault.vault.azure.net/secrets/mysecret/)
        // Note: This reference notation does NOT work locally.  It only works in the cloud configuration!!!

        // Required NuGet packages for direct key vault connection:
        // 1. Azure.Identity
        // 2. Azure.Extensions.AspNetCore.Configuration.Secrets

        var keyVaultEndpoint = Environment.GetEnvironmentVariable("AzureKeyVaultEndpoint");

        if (string.IsNullOrWhiteSpace(keyVaultEndpoint) == false)
        {
            builder.AddAzureKeyVault(new Uri(keyVaultEndpoint), credential);
        }
    })
    .ConfigureServices((builder, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddQueueExampleServices(builder.Configuration);
    })
    .Build();

host.Run();
