using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
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

        public async Task SendMessageToQueue(string queueName, object messageContent)
        {
            await CreateQueueIfNotExist(queueName);
            IQueueClient client = new QueueClient(Constans.ConnectionString, queueName);
            await SendMessage(client, messageContent);
        }

        public async Task CreateQueueIfNotExist(string queueName)
        {
            if (!await managementClient.QueueExistsAsync(queueName))
            {
                await managementClient.CreateQueueAsync(queueName);
            }
        }

        public async Task CreateTopicIfNotExist(string topicName)
        {
            if (!await managementClient.TopicExistsAsync(topicName))
            {
                await managementClient.CreateTopicAsync(topicName);
            }
        }

        public async Task SendMessageToTopic(string topicName, object messageContent, string subs, string messageType, string ruleName = null)
        {
            ITopicClient topicClient = new TopicClient(Constans.ConnectionString, topicName);
            await CreateTopicIfNotExist(topicName);
            await CreateSubscriptionIfNotExist(topicName, subs, messageType, ruleName);
            await SendMessage(topicClient, messageContent, messageType);
        }

        public async Task SendMessage(ISenderClient client, object messageContent, string messageType = null)
        {
            var byteArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
            var message = new Message(byteArr);
            if (messageType != null)
                message.UserProperties["messageType"] = messageType;

            await client.SendAsync(message);
        }


        public async Task CreateSubscriptionIfNotExist(string topicName, string subscriptionName, string messageType, string ruleName = null)
        {
            if (await managementClient.SubscriptionExistsAsync(topicName, subscriptionName))
                return;

            if (messageType != null)
            {
                SubscriptionDescription subscriptionDescription = new(topicName, subscriptionName);
                CorrelationFilter filter = new();
                filter.Properties["messageType"] = messageType;
                RuleDescription rd = new(ruleName ?? messageType + "Rule", filter);
                await managementClient.CreateSubscriptionAsync(subscriptionDescription, rd);

            }
            else
            {
                await managementClient.CreateSubscriptionAsync(topicName, subscriptionName);

            }
        }

    }
}
