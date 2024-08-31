using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBusServiceBusQueueKeyVault;

public class ShowSecretFunction
{
    private readonly ILogger<ShowSecretFunction> _logger;
    private readonly IConfiguration _configuration;

    public ShowSecretFunction(ILogger<ShowSecretFunction> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [Function("ShowSecretFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, string keyName)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string? data = _configuration[keyName];

        string responseMessage = data == null ? $"Key named {keyName} was not Found" : $"{keyName}={data}";

        return new OkObjectResult(responseMessage);
    }
}