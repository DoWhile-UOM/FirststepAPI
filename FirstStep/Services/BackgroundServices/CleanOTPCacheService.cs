using FirstStep.Data;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services.BackgroundServices
{
    public class CleanOTPCacheService : BackgroundService
    {
        private readonly DataContext _context;
        private readonly ILogger<CleanOTPCacheService> _logger;

        public CleanOTPCacheService(DataContext context, ILogger<CleanOTPCacheService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ICleanOTPCache()
        {
            _logger.LogInformation("CleanOTPCacheService: CleanOTPCache() called");

            var otpRequests = await _context.OTPRequests.ToListAsync();

            foreach (var otpRequest in otpRequests)
            {
                if (DateTime.Now > otpRequest.expiry_date_time)
                {
                    _context.OTPRequests.Remove(otpRequest);
                }
            }

            await _context.SaveChangesAsync();
        }

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CleanOTPCacheService: ExecuteAsync() called");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ICleanOTPCache();

                await Task.Delay(86400000, stoppingToken); // 24 hours
            }

            return Task.CompletedTask;
        }
    }
}
