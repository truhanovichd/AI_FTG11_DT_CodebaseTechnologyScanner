using CodebaseTechnologyScanner.Models;
using CodebaseTechnologyScanner.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CodebaseTechnologyScanner.Tests.Services;

/// <summary>
/// Unit tests for FileScanner service
/// </summary>
public class FileScannerTests
{
    private readonly Mock<ILogger<FileScanner>> _mockLogger;
    private readonly FileScanner _fileScanner;

    public FileScannerTests()
    {
        _mockLogger = new Mock<ILogger<FileScanner>>();
        _fileScanner = new FileScanner(_mockLogger.Object);
    }

    [Fact]
    public async Task ScanAsync_WithValidPath_ReturnsScanResult()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        
        try
        {
            var request = new ScanRequest { Path = tempDir };

            // Act
            var result = await _fileScanner.ScanAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.StartedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
            result.FilesScanned.Should().BeGreaterThanOrEqualTo(0);
            result.Items.Should().BeEmpty();
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task ScanAsync_WithNonexistentPath_ReturnsEmptyResult()
    {
        // Arrange
        var nonexistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "_nonexist");
        var request = new ScanRequest { Path = nonexistentPath };

        // Act
        var result = await _fileScanner.ScanAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.FilesScanned.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task ScanAsync_WithCsProjFile_DetectsCSharpProject()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        
        try
        {
            var csprojFile = Path.Combine(tempDir, "TestProject.csproj");
            File.WriteAllText(csprojFile, "<Project></Project>");

            var request = new ScanRequest { Path = tempDir };

            // Act
            var result = await _fileScanner.ScanAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Kind.Should().Be("CSharp");
            result.Items[0].Name.Should().Be("TestProject");
            result.Items[0].Evidence.Should().Contain("TestProject.csproj");
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task ScanAsync_WithPackageJsonFile_DetectsNodeJs()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        
        try
        {
            var packageFile = Path.Combine(tempDir, "package.json");
            File.WriteAllText(packageFile, "{}");

            var request = new ScanRequest { Path = tempDir };

            // Act
            var result = await _fileScanner.ScanAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Kind.Should().Be("Node.js");
            result.Items[0].Evidence.Should().Contain("package.json");
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task ScanAsync_WithDockerfile_DetectsDocker()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        
        try
        {
            var dockerFile = Path.Combine(tempDir, "Dockerfile");
            File.WriteAllText(dockerFile, "FROM ubuntu");

            var request = new ScanRequest { Path = tempDir };

            // Act
            var result = await _fileScanner.ScanAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Kind.Should().Be("Docker");
            result.Items[0].Evidence.Should().Contain("Dockerfile");
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task ScanAsync_WithMultipleFiles_DetectsAll()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        
        try
        {
            File.WriteAllText(Path.Combine(tempDir, "Project.csproj"), "<Project></Project>");
            File.WriteAllText(Path.Combine(tempDir, "package.json"), "{}");
            File.WriteAllText(Path.Combine(tempDir, "Dockerfile"), "FROM ubuntu");

            var request = new ScanRequest { Path = tempDir };

            // Act
            var result = await _fileScanner.ScanAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3);
            result.Items.Should().Contain(i => i.Kind == "CSharp");
            result.Items.Should().Contain(i => i.Kind == "Node.js");
            result.Items.Should().Contain(i => i.Kind == "Docker");
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public async Task ScanAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _fileScanner.ScanAsync(null));
    }

    [Fact]
    public async Task ScanAsync_WithNullPath_ThrowsArgumentNullException()
    {
        // Arrange
        var request = new ScanRequest { Path = null };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _fileScanner.ScanAsync(request));
    }
}
