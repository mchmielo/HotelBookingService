using HotelBookingService.BLL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace HotelBookingService.API
{
    public class FakeApiResponder : DelegatingHandler
    {
        private delegate HttpResponseMessage GetMessage();
        private readonly Dictionary<string, GetMessage> _knownUrls;

        public FakeApiResponder()
        {
            _knownUrls = new Dictionary<string, GetMessage>();
            _knownUrls.Add("Hotel", GetFakeMessageSteps);
            _knownUrls.Add("MakePayment", GetFakeMessageOkOrBadRequest);
            _knownUrls.Add("MakeReservation", GetFakeMessageOkOrBadRequest);
            _knownUrls.Add("VerifyRoomAvailability", GetFakeMessageOkOrBadRequest);
            _knownUrls.Add("VerifyRoomPrice", GetFakeMessageOkOrBadRequest);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request
            , System.Threading.CancellationToken cancellationToken)
        {
            var configuration = Configuration.GetConfiguration();
            if (!request.Headers.Contains(configuration.GetValue<string>("HotelBookingApiKeyHeader")))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            if (!request.Headers.ToString().Contains(configuration.GetValue<string>("HotelBookingApiKey")))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            if(IsRequestKnown(request, out var key))
            {
                return Task.FromResult(_knownUrls[key]());
            }
            else
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
                { RequestMessage = request });
            }
        }

        private HttpResponseMessage GetFakeMessageSteps()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<string>>(
                new List<string>
                {
                    "VerifyRoomAvailability",
                    "MakePayment",
                    "MakeReservation",
                    "SendConfirmationMail"
                }, new JsonMediaTypeFormatter())
            };
        }

        private HttpResponseMessage GetFakeMessageOkOrBadRequest()
        {
            Random random = new Random();
            var number = random.Next(100);
            if(number > 10)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private bool IsRequestKnown(HttpRequestMessage request, out string key)
        {
            foreach(var url in _knownUrls)
            {
                if(request.RequestUri.ToString().Contains(url.Key))
                {
                    key = url.Key;
                    return true;
                }
            }
            key = null;
            return false;
        }
    }
}
