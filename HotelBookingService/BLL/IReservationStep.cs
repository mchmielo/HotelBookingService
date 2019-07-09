using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public interface IReservationStep
    {
        Task<bool> SendDataAsync();
    }
}
