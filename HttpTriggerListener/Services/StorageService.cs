using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HttpTriggerListener.Services
{
    public static class StorageService
    {
        private static string _storageConnectionString;
        private static string _containerName;

        public static void init()
        {
            _storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _containerName = Environment.GetEnvironmentVariable("ClaimsReportsBlobContainerName");
        }

        public static async Task UploadFileAsync(string fileName, Stream pdfStream)
        {
            var blobContainerClient = await GetContainerClientAsync();
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            var s = await blobClient.UploadAsync(pdfStream); 
            var len = s.GetRawResponse().ContentStream.Length;
        }

        private static async Task<BlobContainerClient> GetContainerClientAsync()
        {
            var blobContainerClient = new BlobContainerClient(_storageConnectionString, _containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            return blobContainerClient;
        } 
    }
}
