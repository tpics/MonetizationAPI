using System.Net;
using FluentAssertions;
using Xunit;

public class RateLimitIntegrationTests : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client;
    private readonly Guid _customerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    public RateLimitIntegrationTests(TestAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Should_Return_Ok_When_Under_Limit()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/LimitTest");
        request.Headers.Add("customerId", _customerId.ToString());

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("API call successful");
    }

    [Fact]
    public async Task Get_Should_Return_429_When_Rate_Limit_Exceeded()
    {
        // Arrange
        for (int i = 0; i < 2; i++)
        {
            var okRequest = new HttpRequestMessage(HttpMethod.Get, "/api/LimitTest");
            okRequest.Headers.Add("customerId", _customerId.ToString());
            await _client.SendAsync(okRequest);
        }

        var limitedRequest = new HttpRequestMessage(HttpMethod.Get, "/api/LimitTest");
        limitedRequest.Headers.Add("customerId", _customerId.ToString());

        // Act
        var response = await _client.SendAsync(limitedRequest);

        // Assert
        response.StatusCode.Should().Be((HttpStatusCode)429);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Rate limit or monthly quota exceeded");
    }
}
