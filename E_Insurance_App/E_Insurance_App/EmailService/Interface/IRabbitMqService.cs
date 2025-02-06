namespace E_Insurance_App.EmailService.Interface
{
    public interface IRabbitMqService
    {
        void SendMessage(string queueName, string message);

    }
}
