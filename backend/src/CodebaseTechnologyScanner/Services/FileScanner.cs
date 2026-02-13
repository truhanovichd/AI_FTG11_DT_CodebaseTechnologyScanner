using CodebaseTechnologyScanner.Models;
using Microsoft.Extensions.Logging;

namespace CodebaseTechnologyScanner.Services;

/// <summary>
/// Implements directory scanning to detect technology-related files.
/// </summary>
/// <param name="logger">The logger instance for structured logging.</param>
public class FileScanner(ILogger<FileScanner> logger) : IFileScanner
{
    private readonly ILogger<FileScanner> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Asynchronously scans a directory for technology-related files such as .csproj, package.json, and Dockerfile.
    /// </summary>
    /// <param name="request">The scan request containing the directory path.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the scan results.</returns>
    public async Task<ScanResult> ScanAsync(ScanRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Path);

        var result = new ScanResult
        {
            StartedAt = DateTimeOffset.UtcNow,
        };

        try
        {
            if (!Directory.Exists(request.Path))
            {
                _logger.LogWarning("Directory not found: {DirectoryPath}", request.Path);
                return result;
            }

            _logger.LogInformation("Starting directory scan: {DirectoryPath}", request.Path);

            await Task.Run(() =>
            {
                var files = Directory.GetFiles(request.Path, "*.*", SearchOption.AllDirectories);
                result.FilesScanned = files.Length;

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);

                    if (fileName.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Items.Add(new DetectedItem
                        {
                            Kind = "CSharp",
                            Name = Path.GetFileNameWithoutExtension(file),
                            Evidence = file,
                        });
                    }
                    else if (fileName.Equals("package.json", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Items.Add(new DetectedItem
                        {
                            Kind = "Node.js",
                            Name = "Node.js Project",
                            Evidence = file,
                        });
                    }
                    else if (fileName.Equals("Dockerfile", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Items.Add(new DetectedItem
                        {
                            Kind = "Docker",
                            Name = "Docker",
                            Evidence = file,
                        });
                    }
                }
            }).ConfigureAwait(false);

            _logger.LogInformation(
                "Scan completed. Found {FilesScanned} total files, {ItemsCount} technology items detected",
                result.FilesScanned,
                result.Items.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning directory: {DirectoryPath}", request.Path);
        }

        return result;
    }
}
