using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit.Abstractions;

namespace BasicTemplate.Api.Integration.Test
{
    public class HealthCheckTests : WebApplicationFactory<Program>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public HealthCheckTests(ITestOutputHelper testOutputHelper)
        {
            _client = CreateClient();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Returns_200Ok_Response()
        {
            // ACT
            HttpResponseMessage responseMessage = await _client.GetAsync(new Uri("/health", UriKind.Relative));
            string contentString = await responseMessage.Content.ReadAsStringAsync();
            _testOutputHelper.WriteLine(contentString);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal("Healthy", contentString);
        }
    }
}