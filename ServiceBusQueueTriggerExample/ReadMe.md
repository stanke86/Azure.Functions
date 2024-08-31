# WORK IN PROGRESS


# Queue Example
This Azure Function has two functions
1. QueueFunction function hooks up to an existing service bus.  Currently it will receive the Deal.cs class (under QueueExample.Model library) from the service bus queue specified in the local.settings.json file.
2. ShowSecretFunction is an HttpTrigger used for pulling secrets.  It can pull any secret, but if you want to configure this 
   Azure Function to pull secrets from either Azure Key Vault or Azure App Configuration, this will let you view the secrets.

# ServiceBusQueueLocalSettings Setup
This sample uses the local.settings.json file and the secrets.json file to hold information.

## local.settings.json changes you need to make
1. Update ServiceBus:QueueName to match your Service Bus queue.

## In your secrets.json file add the following data
1. Update ServiceBus:ConnectionString with your Service Bus connection string 
```json
{
  "ServiceBus:ConnectionString": "your-service-bus-connection-string",
}
```

# ServiceBusQueueKeyVault setup
This sample uses the key vault to hold the connection string for the service bus.

## Create a Key Vault
1. Create a key vault
2. Grant yourself the following permissions to the key vault:
    - Key Vault Reader 
	- Key Vault Administrator (so that you can add stuff to the key vault...you can remove it afterwards)
3. Add a secret (Objects > Secrets) to the key vault with the name "ServiceBusConnectionString" and the value of your service bus connection string.

## local.settings.json changes you need to make
1. Modify the key vault reference to point to your key vault


## local.settings.json changes you need to make
1. Update ServiceBusConnectionString with your Service Bus connection string 
2. Update ServiceBusQueueName with the name of your Service Bus queue.
3. Update AZURE_TENANT_ID to your tenant id.  You'll find that under "Microsoft Entra ID" and the "Overview" tab.
4. If you want configuration to come from Azure Key vault, uncomment and update AzureKeyVaultEndpoint (see Key Vault below for more info) 
5. If you want configuration to come from Azure App Configuration, uncomment and update AzureAppConfigurationEndpoint (see App Configuration below for more info) 

## Warnings
The host evaluates special bindings like ServiceBusTrigger, EventHubTrigger, etc. before the program.cs file is even run, so with connections and other information used by the binding there are limitations:
- When testing/debugging locally, this information MUST come from your local.settings.json file!
- In the cloud, you CAN use the key vault REFERENCE syntax: @Microsoft.KeyVault(SecretUri=https://myvault.vault.azure.net/secrets/mysecret/)  
- You CANNOT use Azure App Configuration to configure these values.

## Program.cs changes you need to make 
You should do one of the following 
1. Comment out both the **Azure Key Vault** or **Azure App Configuration** because if you only intend to use the settings in local.settings.json.
2. Comment out **Azure Key Vault** code ife you intent to access everything via **Azure App Configuration**
3. Comment out **Azure App Configuration** code if you intent to access everything via **Azure Key Vault**

## Azure Key Vault changes you need to make
1. You'll need to give yourself permission to access the Azure Key Vault for this to work.  This is done under "Access Policies"
2. You'll need to add some secrets if you want to pull them out via the HttpTrigger.

## Azure App Configuration changes you need to make
1. You'll need to create a managed identity for your App Configuration.
2. You'll have to give that managed identity permission to access your key vault
3. You'll need to give yourself "App Configuration Data Reader" permission to access the App Configuration for this to work.  
   This is done under "Access Policies"
4. You'll have to add secrets from your key vault via the "Configuration Explorer" Create button (you can create a ""key-value"" or "key vault reference")



