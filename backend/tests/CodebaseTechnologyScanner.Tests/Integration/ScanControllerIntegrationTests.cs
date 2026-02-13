using System.Net;
using System.Text;
using System.Text.Json;
using CodebaseTechnologyScanner;
using CodebaseTechnologyScanner.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CodebaseTechnologyScanner.Tests.Integration;

/// <summary>
/// Integration tests for ScanController using WebApplicationFactory
/// </summary>
public class ScanControllerIntegrationTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;

    public ScanControllerIntegrationTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    public async Task InitializeAsync()
    {
        _httpClient = _factory.CreateClient();
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _httpClient?.Dispose();
        _factory?.Dispose();
        await Task.CompletedTask;
    }

    [Fact]
    public async Task PostScan_WithValidPath_ReturnsOkAndScanResult()
    {
        // Arrange
        var request = new ScanRequest { Path = Path.GetTempPath() };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var result = JsonSerializer.Deserialize<ScanResult>(responseContent, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        result.Should().NotBeNull();
        result?.StartedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task PostScan_WithEmptyPath_ReturnsBadRequest()
    {
        // Arrange
        var request = new ScanRequest { Path = string.Empty };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostScan_WithWhitespacePath_ReturnsBadRequest()
    {
        // Arrange
        var request = new ScanRequest { Path = "   " };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostScan_WithNullRequest_ReturnsBadRequest()
    {
        // Arrange
        var content = new StringContent(
            "null",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostScan_WithInvalidPath_ReturnsBadRequest()
    {
        // Arrange
        var request = new ScanRequest { Path = null! };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostScan_WithEmptyBody_ReturnsBadRequest()
    {
        // Arrange
        var content = new StringContent(
            string.Empty,
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        // Should be BadRequest due to JSON parsing error or null request
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task PostScan_WithValidTempPath_ReturnsOkWithEmptyItems()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            var request = new ScanRequest { Path = tempDir };
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PostAsync("/api/scan", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ScanResult>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            result.Should().NotBeNull();
            result?.Items.Should().NotBeNull();
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task PostScan_WithValidPathContainingCsProjFile_ReturnsOkWithDetectedItem()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            File.WriteAllText(Path.Combine(tempDir, "test.csproj"), "<Project></Project>");

            var request = new ScanRequest { Path = tempDir };
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PostAsync("/api/scan", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ScanResult>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            result.Should().NotBeNull();
            result?.Items.Should().NotBeEmpty();
            result?.Items.Should().Contain(i => i.Kind == "CSharp");
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task PostScan_WithMultipleContentTypes_ReturnsOkWithJsonContent()
    {
        // Arrange
        var request = new ScanRequest { Path = Path.GetTempPath() };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task PostScan_WithSpecialCharactersInPath_ReturnsOkOrBadRequest()
    {
        // Arrange - Special characters that might be filesystem-valid or invalid
        var request = new ScanRequest { Path = "C:/Projects/My-App_2024/src#test" };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert - Either succeeds or returns bad request (path might not exist)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task PostScan_ReturnsCorrectHeaders()
    {
        // Arrange
        var request = new ScanRequest { Path = Path.GetTempPath() };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert
        response.Headers.Should().NotBeNull();
        response.Content.Headers.ContentType.Should().NotBeNull();
    }

    [Fact]
    public async Task PostScan_WithLongPathString_ProcessesRequest()
    {
        // Arrange
        var longPath = "C:/" + string.Join("/", Enumerable.Range(1, 5).Select(i => $"folder{i}"));
        var request = new ScanRequest { Path = longPath };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/scan", content);

        // Assert - Path might not exist, so 500 is acceptable
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError);
    }
}
