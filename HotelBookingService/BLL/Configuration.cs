using Microsoft.Extensions.Configuration;
using System.IO;

namespace HotelBookingService.BLL
{
    public static class Configuration
    {
        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
