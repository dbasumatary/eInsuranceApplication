using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace E_Insurance_App.EmailService.Implementation
{
    public class EmailConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<EmailConsumer> _logger;
        private readonly IConfiguration _configuration;

        public EmailConsumer(IConfiguration configuration, ILogger<EmailConsumer> logger)
        {
            _logger = logger;
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void StartConsuming()
        {
            _channel.QueueDeclare(queue: "emailQueue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {Message}", message);

                try
                {
                    var emailMessage = JsonConvert.DeserializeObject<dynamic>(message);
                    SendEmail(emailMessage.Email.ToString(), emailMessage.Subject.ToString(), emailMessage.Body.ToString());

                    // Acknowledge message only after successful processing
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error processing email message: {Error}", ex.Message);
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue message on failure
                }
            };

            _channel.BasicConsume(queue: "emailQueue",
                                 autoAck: false,
                                 consumer: consumer);
        }

        
        private void SendEmail(string email, string subject, string body)
        {
            try
            {
                string smtpServer = _configuration["EmailSettings:SmtpServer"];
                int smtpPort = _configuration.GetValue<int>("EmailSettings:SmtpPort", 587);
                string senderEmail = _configuration["EmailSettings:SenderEmail"];
                string senderPassword = _configuration["EmailSettings:SenderPassword"];

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true 
                };

                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
                _logger.LogInformation("Email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email to {Email}. Error: {Error}", email, ex.Message);
            }
        }

        //public void Dispose()
        //{
        //    _channel?.Close();
        //    _connection?.Close();
        //}
    }
}
