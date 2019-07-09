using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingService.BLL
{
    public abstract class ReservationStepBase
    {
        public abstract List<string> AskForData(out string stepName);
        public abstract void UpdateData(string data);
        public abstract string GetStepName();
    }
}
