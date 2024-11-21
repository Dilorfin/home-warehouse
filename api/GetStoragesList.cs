using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace api;

public static class GetStoragesList
{
    [FunctionName("GetStoragesList")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        var repo = new StorageRepository();

        log.LogInformation("C# HTTP trigger function processed a request.");

        var result = await repo.GetAllAsync();
        return new OkObjectResult(result);
    }
}
