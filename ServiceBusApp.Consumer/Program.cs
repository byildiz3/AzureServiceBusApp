using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusApp.Common;
using ServiceBusApp.Common.Events;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        ConsumeQueue<OrderCreatedEvent>(Constans.OrderCreatedQueueName, i => {

            Console.WriteLine($"Order Created Event Received Message With id : {i.Id} and Name: {i.ProductName}");
        
        
        }).Wait();

        ConsumeQueue<OrderDeletedEvent>(Constans.OrderDeletedQueueName, i => {

            Console.WriteLine($"Order Deleted Event Received Message With id : {i.Id} ");


        }).Wait();

        ConsumeSubs<OrderCreatedEvent>(Constans.OrderCreatedSubscription,Constans.OrderTopic, i => {

            Console.WriteLine($"{Constans.OrderCreatedSubscription} Received Message With id : {i.Id} and Name: {i.ProductName}");


        }).Wait();

        ConsumeSubs<OrderDeletedEvent>(Constans.OrderDeletedSubscription, Constans.OrderTopic, i => {

            Console.WriteLine($"{Constans.OrderDeletedSubscription}  Received Message With id : {i.Id} ");


        }).Wait();



        Console.ReadLine();
    }

    private static async Task ConsumeSubs<T>(string subName, string topicName, Action<T> receivedAction)
    {
        ISubscriptionClient client = new SubscriptionClient(Constans.ConnectionString, topicName,subName);
        client.RegisterMessageHandler(async (message, ct) => {
            var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
            receivedAction(model);
            await Task.CompletedTask;
        }, new MessageHandlerOptions(i => Task.CompletedTask));

        Console.WriteLine($"{typeof(T).Name} is listening");
    }

    private static async Task ConsumeQueue<T>(string queueName, Action<T> receivedAction)
    {
        IQueueClient client = new QueueClient(Constans.ConnectionString, queueName);
        client.RegisterMessageHandler(async (message, ct) => {
            var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
            receivedAction(model);
            await Task.CompletedTask;
        }, new MessageHandlerOptions(i => Task.CompletedTask));

        Console.WriteLine($"{typeof(T).Name} is listening");
    }
}