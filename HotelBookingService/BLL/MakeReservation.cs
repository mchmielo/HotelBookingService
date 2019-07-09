using HotelBookingService.BLL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public class MakeReservation : ReservationStepBase, IReservationStep
    {
        private ReservationDto _dataToSend;
        private readonly IApiConsumer _apiConsumer;
        private const string _endpoint = "MakeReservation";

        public MakeReservation(IApiConsumer apiConsumer)
        {
            _dataToSend = new ReservationDto();
            _apiConsumer = apiConsumer;
        }

        public Task<bool> SendDataAsync()
        {
            try
            {
                return _apiConsumer.PostDataAsync(_dataToSend, _endpoint);
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
            _dataToSend = JsonConvert.DeserializeObject<ReservationDto>(data);
        }

        public override string GetStepName()
        {
            return this.GetType().Name;
        }
    }
}
