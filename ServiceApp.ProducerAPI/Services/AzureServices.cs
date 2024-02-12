using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;
using ServiceBusApp.Common;
using System.Text;

namespace ServiceApp.ProducerAPI.Services
{
    public class AzureServices
    {
        ManagementClient managementClient;

        public AzureServices(ManagementClient managementClient)
        {
            this.managementClient = managementClient;
        }

        public async Task SendMessageToQueue(string queueName,object messageContent)
        {
            await CreateQueueIfNotExist(queueName);
            IQueueClient client = new QueueClient(Constans.ConnectionString, queueName);
           var byteArr=Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
            var message = new Message(byteArr);
           await client.SendAsync(message);
        }

        public async Task CreateQueueIfNotExist(string queueName)
        {
            if (!await managementClient.QueueExistsAsync(queueName))
            {
                await managementClient.CreateQueueAsync(queueName);
            }
        }

       
    }
}
