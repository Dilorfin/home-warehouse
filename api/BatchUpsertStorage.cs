using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api;

public static class BatchUpsertStorage
{
    [FunctionName("BatchUpsertStorage")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        var repo = new StorageRepository();

        log.LogInformation("C# HTTP trigger function processed a request.");
        
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<List<StorageModel>>(requestBody);

        await repo.BatchUpsertAsync(data);
        return new OkResult();
    }
}
