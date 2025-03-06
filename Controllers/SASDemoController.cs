using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AzureStorageSASDemo.Controllers
{
    public class SASDemoController : Controller
    {
        private readonly BlobServiceClient _blobServiceClient;

        public SASDemoController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        [Route("SAS/getblobsasurl")]
        [HttpGet]
        public string GetBlobSASURL()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("mycontainer");
            var blobClient = blobContainerClient.GetBlobClient("Car_1.jpg");

            var sasBuilder = new BlobSasBuilder()
            {
                BlobName = "Car_1.jpg",
                BlobContainerName = "mycontainer",
                ExpiresOn = DateTime.Now.AddSeconds(300),
                Resource = "b"
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            var sasURI = blobClient.GenerateSasUri(sasBuilder);
            var blobUri = sasURI.AbsoluteUri;
            return blobUri;
        }

        [Route("SAS/getblobsasurlatcontainerlevel")]
        [HttpGet]
        public string GetBlobSASUrlAtContainerLevel()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("mycontainer");
            var blobClient = blobContainerClient.GetBlobClient("Car_1.jpg");

            var sasBuilder = new BlobSasBuilder()
            {                
                BlobContainerName = "mycontainer",
                ExpiresOn = DateTime.Now.AddSeconds(300),
                Resource = "c"
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sarContainerURI = blobContainerClient.GenerateSasUri(sasBuilder);
            var containerUri = sarContainerURI.AbsoluteUri;

            var token = containerUri.Split("?")[1];
            var blobUri = blobClient.Uri.AbsoluteUri + "?" + token;

            return blobUri;
        }

    }
}
