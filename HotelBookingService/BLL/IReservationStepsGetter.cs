using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public interface IReservationStepsGetter
    {
        Task<List<IReservationStep>> GetDataAsync(Guid hotelGuid);
    }
}
