using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace api;

public static class GetStorage
{
    [FunctionName("GetStorage")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        var repo = new StorageRepository();

        log.LogInformation("C# HTTP trigger function processed a request.");

        string id = req.Query["id"];

        var result = await repo.GetAsync(id);
        if (result is null)
            return new NotFoundResult();
        return new OkObjectResult(result);
    }
}
