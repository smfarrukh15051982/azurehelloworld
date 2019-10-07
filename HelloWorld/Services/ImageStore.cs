﻿using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HelloWorld.Services
{
    public class ImageStore
    {
        CloudBlobClient blobClient;
        string baseUri = "https://storageaccountbatch90.blob.core.windows.net/";

        public ImageStore()
        {
            var credentials = new StorageCredentials("storageaccountbatch90", "DYGuXDg4vSMlgH1cYsyWSVWsVjRs5EGHGaHHjiq9y8nlkAishzyMvzHvqt+kYipDBWE1HH/g9MM6ZyEFdG/RbQ==");
            blobClient = new CloudBlobClient(new Uri(baseUri), credentials);
        }

        public async Task<string> SaveImage(Stream imageStream)
        {
            var imageId = Guid.NewGuid().ToString();
            var container = blobClient.GetContainerReference("images");
            var blob = container.GetBlockBlobReference(imageId);
            await blob.UploadFromStreamAsync(imageStream);
            return imageId;
        }

        public string UriFor(string imageId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            var container = blobClient.GetContainerReference("images");
            var blob = container.GetBlockBlobReference(imageId);
            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return $"{baseUri}images/{imageId}{sas}";
        }
    }
}
