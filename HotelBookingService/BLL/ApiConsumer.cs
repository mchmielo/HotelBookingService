using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public class ApiConsumer : IApiConsumer
    {
        protected const string _httpClientName = "HotelBookingApi";
        protected readonly IHttpClientFactory _httpClientFactory;

        public ApiConsumer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetDataAsync<T>(string endpoint)
        {
            var client = _httpClientFactory.CreateClient(_httpClientName);

            var response = await client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            var responseDto = JsonConvert.DeserializeObject<T>(stringResponse, jsonSerializerSettings);
            return responseDto;
        }

        public async Task<bool> PostDataAsync<T>(T dataToSend, string endpoint)
        {
            var client = _httpClientFactory.CreateClient(_httpClientName);
            string serializedDto = SerializeData(dataToSend);
            var httpContent = new StringContent(serializedDto, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, httpContent);
            response.EnsureSuccessStatusCode();
            return true;
        }

        private string SerializeData<T>(T dataToSend)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };            
            return JsonConvert.SerializeObject(dataToSend, jsonSettings);
        }
    }
}
