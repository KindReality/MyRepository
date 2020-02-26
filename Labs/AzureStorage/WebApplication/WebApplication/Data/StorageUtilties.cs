using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Data
{
    public class StorageUtilties
    {
        private static BlobServiceClient GetBlobServiceClient()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=saftstorage;AccountKey=NU2r+Mk/EiuQoAHVV8vUq+quy3XAzo5SnbamSFwHN9fQRThqXi22A8YUJAX3u9yDp7G/Ar6nfYSJX0eg/YY1Hw==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            return blobServiceClient;
        }
        public static BlobContainerClient GetBlobContainerClient(string containerName)
        {
            BlobServiceClient blobServiceClient = GetBlobServiceClient();
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return containerClient;
        }
        public static async void UploadBlob(string containerName, string blobName, byte[] blob)
        {
            MemoryStream stream = new MemoryStream(blob);
            containerName = containerName.ToLower();
            blobName = blobName.ToLower();
            BlobContainerClient blobContainerClient = GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream );
        }

        public static DownloadBlobResult DownloadBlob(string containerName, string blobName)
        {
            DownloadBlobResult result = new DownloadBlobResult();
            containerName = containerName.ToLower();
            blobName = blobName.ToLower();
            BlobContainerClient blobContainerClient = GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            Response<BlobDownloadInfo> r = blobClient.DownloadAsync().Result;
            result.ContentType = r.Value.ContentType;
            var photoData = new byte[r.Value.ContentLength];
            r.Value.Content.Read(photoData, 0, (int)r.Value.ContentLength);
            result.Blob = photoData;
            return result;
        }
    }

    public class DownloadBlobResult
    {
        public string ContentType { get; set; }
        public byte[] Blob { get; set; }
    }
}
