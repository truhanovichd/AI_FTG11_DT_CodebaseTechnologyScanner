using CodebaseTechnologyScanner.Controllers;
using CodebaseTechnologyScanner.Models;
using CodebaseTechnologyScanner.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CodebaseTechnologyScanner.Tests.Controllers;

/// <summary>
/// Unit tests for ScanController
/// </summary>
public class ScanControllerTests
{
    private readonly Mock<IFileScanner> _mockFileScanner;
    private readonly Mock<ILogger<ScanController>> _mockLogger;
    private readonly ScanController _controller;

    public ScanControllerTests()
    {
        _mockFileScanner = new Mock<IFileScanner>();
        _mockLogger = new Mock<ILogger<ScanController>>();
        _controller = new ScanController(_mockFileScanner.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Scan_WithValidRequest_ReturnsOkWithResult()
    {
        // Arrange
        var request = new ScanRequest { Path = "C:/test" };
        var expectedResult = new ScanResult
        {
            StartedAt = DateTimeOffset.UtcNow,
            FilesScanned = 10,
            Items = new List<DetectedItem>
            {
                new() { Kind = "CSharp", Name = "Project", Evidence = "Project.csproj" }
            }
        };

        _mockFileScanner.Setup(fs => fs.ScanAsync(request))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Scan(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result;
        okResult.Value.Should().Be(expectedResult);
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        _mockFileScanner.Verify(fs => fs.ScanAsync(request), Times.Once);
    }

    [Fact]
    public async Task Scan_WithNullRequest_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.Scan(null!);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badResult = (BadRequestObjectResult)result.Result;
        badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badResult.Value.Should().BeOfType<ProblemDetails>();
        var problemDetails = (ProblemDetails)badResult.Value;
        problemDetails?.Title.Should().Be("Invalid Request");
    }

    [Fact]
    public async Task Scan_WithEmptyPath_ReturnsBadRequest()
    {
        // Arrange
        var request = new ScanRequest { Path = string.Empty };

        // Act
        var result = await _controller.Scan(request);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badResult = (BadRequestObjectResult)result.Result;
        badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var problemDetails = (ProblemDetails)badResult.Value!;
        problemDetails?.Title.Should().Be("Invalid Path");
    }

    [Fact]
    public async Task Scan_WithWhitespacePath_ReturnsBadRequest()
    {
        // Arrange
        var request = new ScanRequest { Path = "   " };

        // Act
        var result = await _controller.Scan(request);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badResult = (BadRequestObjectResult)result.Result;
        badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var problemDetails = (ProblemDetails)badResult.Value!;
        problemDetails?.Title.Should().Be("Invalid Path");
    }

    [Fact]
    public async Task Scan_WhenServiceThrowsArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var request = new ScanRequest { Path = "C:/invalid" };
        _mockFileScanner.Setup(fs => fs.ScanAsync(It.IsAny<ScanRequest>()))
            .ThrowsAsync(new ArgumentException("Invalid argument"));

        // Act
        var result = await _controller.Scan(request);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badResult = (BadRequestObjectResult)result.Result;
        badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var problemDetails = (ProblemDetails)badResult.Value!;
        problemDetails?.Title.Should().Be("Invalid Argument");
    }

    [Fact]
    public async Task Scan_WhenServiceThrowsUnexpectedException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ScanRequest { Path = "C:/test" };
        _mockFileScanner.Setup(fs => fs.ScanAsync(It.IsAny<ScanRequest>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.Scan(request);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var errorResult = (ObjectResult)result.Result;
        errorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        var problemDetails = (ProblemDetails)errorResult.Value;
        problemDetails.Title.Should().Be("Internal Server Error");
    }

    [Fact]
    public async Task Scan_WithEmptyResult_ReturnsOkWithEmptyItems()
    {
        // Arrange
        var request = new ScanRequest { Path = "C:/empty" };
        var emptyResult = new ScanResult
        {
            StartedAt = DateTimeOffset.UtcNow,
            FilesScanned = 0,
            Items = new List<DetectedItem>()
        };

        _mockFileScanner.Setup(fs => fs.ScanAsync(request))
            .ReturnsAsync(emptyResult);

        // Act
        var result = await _controller.Scan(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result;
        var returnedResult = (ScanResult)okResult.Value;
        returnedResult.Items.Should().BeEmpty();
        returnedResult.FilesScanned.Should().Be(0);
    }

    [Fact]
    public async Task Scan_WithComplexPath_ProcessesSuccessfully()
    {
        // Arrange
        var complexPath = "C:/Users/Test/Documents/Projects/MyApp";
        var request = new ScanRequest { Path = complexPath };
        var expectedResult = new ScanResult
        {
            StartedAt = DateTimeOffset.UtcNow,
            FilesScanned = 50,
            Items = new List<DetectedItem>
            {
                new() { Kind = "CSharp", Name = "App", Evidence = "App.csproj" },
                new() { Kind = "Node.js", Name = "Node.js Project", Evidence = "package.json" }
            }
        };

        _mockFileScanner.Setup(fs => fs.ScanAsync(It.IsAny<ScanRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Scan(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result;
        var returnedResult = (ScanResult)okResult.Value;
        returnedResult.Items.Should().HaveCount(2);
    }
}
