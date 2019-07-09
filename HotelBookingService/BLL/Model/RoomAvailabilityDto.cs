using System;

namespace HotelBookingService.BLL.Model
{
    public class RoomAvailabilityDto
    {
        public Guid Guid { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
