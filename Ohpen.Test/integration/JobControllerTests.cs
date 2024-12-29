using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Ohpen.job;
using Ohpen.job.info;
using OhpenExt.info;
using Xunit;

namespace Ohpen.Tests.Controllers
{
    public class JobControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly HttpClient client;

        public JobControllerTests(WebApplicationFactory<Program> _factory)
        {
            factory = _factory;
            client = _factory.CreateClient();
        }

        [Fact]
        public async Task StartJob_BatchJob_ValidRequest_ReturnsAcceptedWithGuid()
        {
            // Arrange
            var type = nameof(BatchJob);
            var items = new List<object> { "item1", 123, true };

            // Act
            var response = await client.PostAsJsonAsync($"/api/jobs?_type={type}", items);

            // Assert status
            Xunit.Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            // Assert guid
            var content = await response.Content.ReadAsStringAsync();
            Xunit.Assert.True(Guid.TryParse(content.Trim('"'), out var guid));
            Xunit.Assert.NotEqual(guid, Guid.Empty);

            // status
            response = await client.GetAsync($"/api/jobs/{guid}/status");
            Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert status
            content = await response.Content.ReadAsStringAsync();
            Xunit.Assert.Equal(
                JsonSerializer.Serialize(new Status() { ItemCount = 3, ProcessedCount = 0, FailedCount = 0 }).ToLower(),
                content.Trim('"').ToLower()
            );
        }

        [Fact]
        public async Task StartJob_BulkJob_ValidRequest_ReturnsAcceptedWithGuid()
        {
            // Arrange
            var type = nameof(BulkJob);
            var items = new List<object> { "item1", 123, true };

            // Act
            var response = await client.PostAsJsonAsync($"/api/jobs?_type={type}", items);

            // Assert status
            Xunit.Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            // Assert guid
            var content = await response.Content.ReadAsStringAsync();
            Xunit.Assert.True(Guid.TryParse(content.Trim('"'), out var guid));
            Xunit.Assert.NotEqual(guid, Guid.Empty);

            // status
            response = await client.GetAsync($"/api/jobs/{guid}/status");
            Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert status
            content = await response.Content.ReadAsStringAsync();
            Xunit.Assert.Equal(
                JsonSerializer.Serialize(new Status() { ItemCount = 3, ProcessedCount = 0, FailedCount = 0 }).ToLower(),
                content.Trim('"').ToLower()
            );
        }
    }
}