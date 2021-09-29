using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using MongoDB.Bson;
using OrganicStoreFunctionApp.Models;

namespace OrganicStoreFunctionApp
{
    public static class ProductDetails
    {
        [FunctionName("ProductDetails")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            OkObjectResult responseMessage =null ;
            log.LogInformation("C# HTTP trigger function processed a request.");

            if ((req.Method == HttpMethods.Get))
            {

                var client = new MongoClient(System.Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING"));
                var database = client.GetDatabase("organic");
                var collection = database.GetCollection<Products>("products");

                //var filterBuilder1 = Builders<Products>.Filter;
                //var filter1 = filterBuilder1.Eq("id", Id);
                //var searchResult1 = collection.Find(filter1).ToList();

                var searchResult1 = collection.AsQueryable();
                
                responseMessage = new OkObjectResult(searchResult1);
            }
            if (req.Method == HttpMethods.Post)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Products data = JsonConvert.DeserializeObject<Products>(requestBody);

                var client = new MongoClient(System.Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING"));
                var database = client.GetDatabase("organic");
                var collection = database.GetCollection<Products>("products");

                if (data != null)
                {
                    data._id = ObjectId.GenerateNewId().ToString();
                    await collection.InsertOneAsync(data);
                    
                    await UploadFileToBlobStorageAsync(data?.image, data?.ImagePath);
                    responseMessage = new OkObjectResult($"Product {data.name} created Successfully and Image Uplaoded Successfully to blob storage {data.image}");
                }
                
            }
            return responseMessage;

        }


        private static async Task<bool> UploadFileToBlobStorageAsync(string imageName, string imagePath)
        {
            bool result = false;
            string storageConnectionString = System.Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            CloudStorageAccount storageAccount;
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("organicstoreproducts");
                cloudBlobContainer.CreateIfNotExistsAsync();

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                //await cloudBlockBlob.UploadFromFileAsync(path: imagePath);
                await cloudBlockBlob.UploadFromFileAsync(path:imagePath.ToString());
                result = true;
            }
            return result;
        }
    }
}
