namespace E_Insurance_App.EmailService.Implementation
{
    public class EmailConsumerService : BackgroundService
    {
        private readonly EmailConsumer _emailConsumer;

        public EmailConsumerService(EmailConsumer emailConsumer)
        {
            _emailConsumer = emailConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _emailConsumer.StartConsuming();
            return Task.CompletedTask;
        }
    }

}
