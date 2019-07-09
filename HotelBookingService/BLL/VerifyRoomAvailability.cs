using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HotelBookingService.BLL.Model;
using Newtonsoft.Json;

namespace HotelBookingService.BLL
{
    public class VerifyRoomAvailability : ReservationStepBase, IReservationStep
    {
        private readonly IApiConsumer _apiConsumer;
        private RoomAvailabilityDto _dataToSend;
        private const string _endpoint = "VerifyRoomAvailability";

        public VerifyRoomAvailability(IApiConsumer apiConsumer)
        {
            _dataToSend = new RoomAvailabilityDto();
            _apiConsumer = apiConsumer;
        }

        public Task<bool> SendDataAsync()
        {
            var endpoint = $"{_endpoint}/{_dataToSend.Guid.ToString()}";
            try
            {
                return _apiConsumer.PostDataAsync(_dataToSend, endpoint);
            }
            catch(HttpRequestException)
            {
                return Task.FromResult(false);
            }
        }

        public override List<string> AskForData(out string stepName)
        {
            stepName = this.GetType().Name;

            List<string> propertiesList = new List<string>();

            var properties = _dataToSend.GetType().GetProperties();
            foreach (var property in properties)
            {
                propertiesList.Add(property.Name);
            }
            return propertiesList;
        }

        public override void UpdateData(string data)
        {
            _dataToSend = JsonConvert.DeserializeObject<RoomAvailabilityDto>(data);
        }

        public override string GetStepName()
        {
            return this.GetType().Name;
        }
    }
}
