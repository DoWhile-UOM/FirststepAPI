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
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var advertisementService = scope.ServiceProvider.GetRequiredService<IAdvertisementService>();
                // var OTPService = scope.ServiceProvider.GetRequiredService<CleanOTPCacheService>();

                // Clean OTP cache
                // await OTPService.CleanOTPCache();

                // Close expired advertisements
                await advertisementService.CloseExpiredAdvertisements();
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
