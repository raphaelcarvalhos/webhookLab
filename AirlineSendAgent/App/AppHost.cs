using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using AirlineSendAgent.Client;
using AirlineSendAgent.Context;
using AirlineSendAgent.Dtos;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AirlineSendAgent.App
{
    public class AppHost : IAppHost
    {
        private readonly SendAgentDBContext _context;
        private readonly IWebhookClient _webHookClient;
        public AppHost(SendAgentDBContext context, IWebhookClient webHookClient)
        {
            _context = context;
            _webHookClient = webHookClient;
        }
        public void Run()
        {
            var factory = new ConnectionFactory() {HostName = "localhost", Port = 5672};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()){
                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName, exchange: "trigger", routingKey: "");

                var consumer = new EventingBasicConsumer(channel);
                Console.WriteLine("Ouvindo Message Bus...");

                consumer.Received += async (ModuleHandle, ea) => {
                    var body = ea.Body;
                    var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                    var message = JsonSerializer.Deserialize<NotificationMessageDto>(notificationMessage);

                    var webhookToSend = new FlightDetailChangePayloadDto(){
                        WebhookType = message.WebhookType,
                        OldPrice = message.OldPrice,
                        NewPrice = message.NewPrice,
                        FlightCode = message.FlightCode,
                        WebhookURL = string.Empty,
                        Secret = string.Empty,
                        Publisher = string.Empty
                    };

                    foreach (var whs in _context.WebhookSubscriptions.Where(subs => subs.WebhookType.Equals(message.WebhookType)))
                    {
                        webhookToSend.WebhookURL = whs.WebhookURL;
                        webhookToSend.Secret = whs.Secret;
                        webhookToSend.Publisher = whs.WebhookPublisher;

                        await _webHookClient.SendWebhookNotificationAsync(webhookToSend);
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.ReadLine();
            }
        }
    }
}