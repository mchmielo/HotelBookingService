using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingService.BLL.Model
{
    public class UserReservationDto
    {
        public Guid Guid { get; set; }
        public Guid ReservationGuid { get; set; }
        public decimal AmountToPay { get; set; }
        public string Credentials { get; set; }
    }
}
