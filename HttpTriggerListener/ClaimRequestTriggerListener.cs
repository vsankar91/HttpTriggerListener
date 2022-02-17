using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Text.Json;
using HttpTriggerListener.Models;
using HttpTriggerListener.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace HttpTriggerListener
{
    public static class ClaimRequestTriggerListener
    {
        [FunctionName("ClaimRequestTriggerListener")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var claimRequest = JsonSerializer.Deserialize<ClaimRequest>(requestBody);

                var pdfFileName = $"{claimRequest.FirstName}-{claimRequest.LastName}-{claimRequest.Id}.pdf";

                ReportGeneratorService.init(log);

                using var pdfFileStream = ReportGeneratorService.CreateClaimRequestReport(claimRequest);
                pdfFileStream.Position = 0;

                log.LogInformation($"Length = {pdfFileStream.Length}");

                StorageService.init();
                await StorageService.UploadFileAsync(pdfFileName, pdfFileStream);

                log.LogInformation($"File saved at {pdfFileName}");

                return new OkObjectResult(pdfFileName);
            }
            catch(Exception ex)
            {
                log.LogError($"Error {ex.StackTrace}");
                log.LogError($"Error {ex.Message}");
                throw ex;
            }
        }
    }
}
