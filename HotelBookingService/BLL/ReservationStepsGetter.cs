using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public class ReservationStepsGetter : IReservationStepsGetter
    {
        private const string _endpoint1 = "Hotel";
        private const string _endpoint2 = "Steps";
        private readonly IApiConsumer _apiConsumer;
        private readonly IMailSender _mailSender;

        public ReservationStepsGetter(IApiConsumer apiConsumer, IMailSender mailSender)
        {
            _apiConsumer = apiConsumer;
            _mailSender = mailSender;
        }

        public async Task<List<IReservationStep>> GetDataAsync(Guid hotelGuid)
        {
            var endpoint = $"{_endpoint1}/{hotelGuid.ToString()}/{_endpoint2}";
            try
            {
                var stringSteps = await _apiConsumer.GetDataAsync<List<string>>(endpoint);
                var result = new List<IReservationStep>();
                foreach (var stringStep in stringSteps)
                {
                    switch(stringStep)
                    {
                        case "MakePayment":
                            result.Add(new MakePayment(_apiConsumer));
                            break;
                        case "MakeReservation":
                            result.Add(new MakeReservation(_apiConsumer));
                            break;
                        case "SendConfirmationMail":
                            result.Add(new SendConfirmationMail(_mailSender));
                            break;
                        case "VerifyRoomAvailability":
                            result.Add(new VerifyRoomAvailability(_apiConsumer));
                            break;
                        case "VerifyRoomPrice":
                            result.Add(new VerifyRoomPrice(_apiConsumer));
                            break;
                        default:
                            throw new ArgumentException($"Unexpected step name.");
                    }
                }
                return result;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
    }
}
