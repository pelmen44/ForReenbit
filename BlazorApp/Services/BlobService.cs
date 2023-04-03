using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorApp.Services;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using System.Net.Mail;


namespace BlazorApp.Services
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BlobService> _logger;
        string blobStorageconnection = string.Empty;
        private string blobContainerName = "container1";
        public BlobService(IConfiguration configuration, ILogger<BlobService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            blobStorageconnection = _configuration.GetConnectionString("AzureStorageAccount");
        }
        public async Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream)
        {
            try
            {
                var container = new BlobContainerClient(blobStorageconnection, blobContainerName);
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                var blob = container.GetBlobClient(strFileName);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contecntType });
                var urlString = blob.Uri.ToString();
                return urlString;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                throw;
            }
        }
        public async Task<bool> DeleteFileToBlobAsync(string strFileName)
        {
            try
            {
                var container = new BlobContainerClient(blobStorageconnection, blobContainerName);
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                var blob = container.GetBlobClient(strFileName);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                throw;
            }
        }
    }
}


