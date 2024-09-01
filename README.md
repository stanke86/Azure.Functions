# Azure Functions

# Work in Progress converting .NET 6 to .NET 8  (see the [DotNet6](https://github.com/madcodemonkey/Azure.Functions/tree/DotNet6) branch if something is broken.)

## Projects List
### .NET 8
- HttpTriggerExample01 - HttpTrigger binding example.
- ServiceBusQueueTriggerExample - A Service Bus queue example 
   - It also demonstrates how to hook up Key Vault locally
   - It has some code, still broken, to hook up app configuration locally.  There is an issue with the local.setting.json syntax.
   
### Still .NET 6  (in progress.....)
- Common
   - AppInsightsEnhancedWithSerilogSink - A Serilog example that I was using for common logging (TODO: it needs work)
- Durable - durable function examples
- Event Hub - Azure functions with event hub trigger
- Powershell - PowerShell in Azure Function examples

## Branching scheme
- [DotNet8](https://github.com/madcodemonkey/Azure.Functions/tree/DotNet8): .NET 8.0 examples
- [DotNet6](https://github.com/madcodemonkey/Azure.Functions/tree/DotNet6): .NET 6.0 examples (with the current exception of AppInsightsEnhancedWithSerilogSink and PowerShell items)
