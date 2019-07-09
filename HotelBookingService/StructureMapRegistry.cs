using HotelBookingService.BLL;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingService
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            //For<IReservationStep>().LifecycleIs(Lifecycles.Container)
            //                        .Use<StructuremapMessagingService>();
        }
    }
}
