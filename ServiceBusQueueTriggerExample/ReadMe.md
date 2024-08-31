# WORK IN PROGRESS

# Queue Example
This Azure Function has two functions
1. QueueFunction function hooks up to an existing service bus.  Currently it will receive the Deal.cs class (under QueueExample.Model library) from the service bus queue specified in the local.settings.json file.
2. ShowSecretFunction is an HttpTrigger used for pulling secrets.  It can pull any secret, but if you want to configure this 
   Azure Function to pull secrets from either Azure Key Vault or Azure App Configuration, this will let you view the secrets.

---
---
---

# ServiceBusQueueLocalSettings Setup
This sample uses the local.settings.json file and the secrets.json file to hold information.

## local.settings.json changes you need to make
1. ServiceBus:QueueName - update it to match your Service Bus queue.

## In your secrets.json file add the following data
1. Update ServiceBus:ConnectionString with your Service Bus connection string 
```json
{
  "ServiceBus:ConnectionString": "your-service-bus-connection-string",
}
```
---
---
---
# ServiceBusQueueKeyVault setup
This sample uses the key vault to hold the connection string for the service bus.

## Create a Key Vault
1. Create a key vault
2. Grant yourself the following permissions to the key vault:
    - Key Vault Reader 
	- Key Vault Administrator (so that you can add stuff to the key vault...you can remove it afterwards)
3. Add a secret (Objects > Secrets) to the key vault with the name "ServiceBusConnectionString" and the value of your service bus connection string.

## local.settings.json changes you need to make
1. ServiceBus:ConnectionString - update with the key vault reference syntax to point to your key vault
   - You can use either the syntax mentioned [here](https://learn.microsoft.com/en-us/azure/app-service/app-service-key-vault-references?tabs=azure-cli#source-app-settings-from-key-vault)
   - I prefer the non-versioned syntax:  @Microsoft.KeyVault(SecretUri=https://myvault.vault.azure.net/secrets/mysecret/)
      - replace "myvault" with your key vault name and "mysecret" with the name of the secret you created in the key vault.
2. ServiceBus:QueueName - update to match your Service Bus queue name.
3. AZURE_TENANT_ID - update to your tenant id.  You'll find that under "Microsoft Entra ID" and the "Overview" tab.
   - This is mainly for the cases where you have access to more than one tenant.  If this is the case, also make sure that you are logged 
	 into that tenant in Visual Studio as well; otherwise, you will get an error in the program.cs about credentials.

Notes
   - Warning! Do not use vault syntax in your secrets.json thinking it will override your local.settings.json setting and pull values from vault.
	 What it appears to do is just a string replacement and NOT a retrieval from vault.  See https://stackoverflow.com/a/76778244/97803 
---
---
---
# ServiceBusQueueAppConfiguration setup
This sample uses the App Configuration locally and key vault to hold the connection string for the service bus.

## Create a Key Vault
1. Create a key vault
2. Grant yourself the following permissions to the key vault:
    - Key Vault Reader 
	- Key Vault Administrator (so that you can add stuff to the key vault...you can remove it afterwards)
3. Add a secret (Objects > Secrets) to the key vault with the name "ServiceBusConnectionString" and the value of your service bus connection string.

## Azure App Configuration changes you need to make
1. You'll need to create a managed identity for your App Configuration.
2. You'll have to give that managed identity permission to access your key vault
3. You'll need to give yourself "App Configuration Data Reader" permission to access the App Configuration for this to work.  
   This is done under "Access Policies"
4. You'll have to add secrets from your key vault via the "Configuration Explorer" Create button (you can create a ""key-value"" or "key vault reference")


## local.settings.json changes you need to make
1. ServiceBus:ConnectionString - update with the ???? syntax to point to your app configuration
2. ServiceBus:QueueName - update to match your Service Bus queue name.
3. AZURE_TENANT_ID - update to your tenant id.  You'll find that under "Microsoft Entra ID" and the "Overview" tab.
   - This is mainly for the cases where you have access to more than one tenant.  If this is the case, also make sure that you are logged 
	 into that tenant in Visual Studio as well; otherwise, you will get an error in the program.cs about credentials.




