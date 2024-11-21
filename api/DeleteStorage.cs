using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api;

public static class DeleteStorage
{
    [FunctionName("DeleteStorage")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
        ILogger log)
    {
        var repo = new StorageRepository();

        log.LogInformation("C# HTTP trigger function processed a request.");
        
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<StorageModel>(requestBody);

        return new OkObjectResult(await repo.DeleteAsync(data));
    }
}
