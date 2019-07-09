using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public interface IApiConsumer
    {
        Task<T> GetDataAsync<T>(string endpoint);
        Task<bool> PostDataAsync<T>(T dataToSend, string endpoint);
    }
}
