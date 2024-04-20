using FirstStep.Data;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services.BackgroundServices
{
    public class CleanOTPCacheService 
    {
        private readonly DataContext _context;

        public CleanOTPCacheService(DataContext context)
        {
            _context = context;
        }

        public async Task CleanOTPCache()
        {
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
    }
}
