using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingService.BLL.Model
{
    public class ReservationDto
    {
        public Guid Guid { get; set; }
        public Guid UserGuid { get; set; }
        public Guid RoomGuid { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
