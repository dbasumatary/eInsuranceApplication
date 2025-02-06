using E_Insurance_App.EmailService.Interface;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text;

namespace E_Insurance_App.EmailService.Implementation
{

    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(IConnection connection, ILogger<RabbitMqService> logger)
        {
            _connection = connection;
            _logger = logger;

            _channel = _connection.CreateModel();
        }

        public void SendMessage(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: properties,
                                 body: body);

            _logger.LogInformation("Message sent to RabbitMQ: {Message}", message);
        }

        //public void Dispose()
        //{
        //    _channel?.Close();
        //    _connection?.Close();
        //}
    }
}
