using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HungryPizzaAPI;
using Xunit;

namespace HungryPizzaAPITest.Integration.API
{
    public class OrderTest : IClassFixture<ApplicationFactory<Startup>>
    {
        public OrderTest(ApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        private readonly HttpClient _httpClient;

        [Fact]
        public async Task GetOrders()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/order");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PlaceOrder()
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/order");
            var order = File.ReadAllText(@"Domain/Fixtures/order.json");
            request.Content = new StringContent(order.Replace("\r\n", ""));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}