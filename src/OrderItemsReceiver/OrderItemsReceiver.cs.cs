using Azure.Storage.Blobs;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OrdersItemsReceiver
{
    public static class OrderItemsReceiver
    {
        [FunctionName("OrderItemsReserver")]
        public static async Task Run(
            [ServiceBusTrigger("orders", Connection = "ServiceBusConnectionKey")] string myQueueItem)
        {
            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var containerClient = blobServiceClient.GetBlobContainerClient("items-orders");
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}.json");
            await using var ms = new MemoryStream(Encoding.UTF8.GetBytes(myQueueItem));
            await blobClient.UploadAsync(ms);
        }
    }
}