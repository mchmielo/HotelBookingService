using HotelBookingService.BLL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public class SendConfirmationMail : ReservationStepBase, IReservationStep
    {
        private readonly IMailSender _mailSender;
        private UserMailDto _dataToSend;

        public SendConfirmationMail(IMailSender mailSender)
        {
            _dataToSend = new UserMailDto();
            _mailSender = mailSender;
        }

        public Task<bool> SendDataAsync()
        {
            try
            {
               _mailSender.SendMail(_dataToSend);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
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
            _dataToSend = JsonConvert.DeserializeObject<UserMailDto>(data);
        }

        public override string GetStepName()
        {
            return this.GetType().Name;
        }
    }
    
}
