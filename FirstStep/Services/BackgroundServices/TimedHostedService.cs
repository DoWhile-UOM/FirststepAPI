using FirstStep.Data;

namespace FirstStep.Services.BackgroundServices
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private Timer? _timer;

        private readonly IServiceScopeFactory _scopeFactory;

        public TimedHostedService(
            ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            // SeedData().Wait();

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private Task SeedData()
        {
            DataSeeder seeder = 
                new DataSeeder(
                    _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>(), 
                    _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IAdvertisementService>());

            return seeder.SeedAdvertisements(10);
        }

        private async void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var advertisementService = scope.ServiceProvider.GetRequiredService<IAdvertisementService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                // Close expired advertisements
                await advertisementService.CloseExpiredAdvertisements();

                // remove expired advertisements from seeker's saved list
                await advertisementService.RemoveSavedExpiredAdvertisements();

                // remove expired otps
                emailService.RemoveExpiredOTP();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
