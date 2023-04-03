using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit.Abstractions;

namespace BasicTemplate.Api.Unit.Test
{
    public class HealthCheckTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public HealthCheckTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Returns_Expected_Status_Message()
        {
            // ARRANGE
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddOptions();
            services.AddHealthChecks()
                .AddCheck("health", () =>
                {
                    return HealthCheckResult.Healthy("I am helthy");
                });

            ServiceProvider provider = services.BuildServiceProvider();
            var healthCheckService = provider.GetRequiredService<HealthCheckService>();

            // ACT
            HealthReport healthReport = await healthCheckService.CheckHealthAsync();
            _testOutputHelper.WriteLine(healthReport.Status.ToString());

            // ASSERT
            Assert.Equal(HealthStatus.Healthy, healthReport.Status);
            Assert.Equal("I am helthy", healthReport.Entries["health"].Description);
        }
    }
}