using CodebaseTechnologyScanner.Models;
using Microsoft.Extensions.Logging;

namespace CodebaseTechnologyScanner.Services;

/// <summary>
/// Provides functionality to scan directories for technology-related files.
/// </summary>
/// <param name="logger">The logger instance for structured logging.</param>
public class FileScanner(ILogger<FileScanner> logger) : IFileScanner
{
    private readonly ILogger<FileScanner> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Asynchronously scans a directory for technology-related files such as .csproj, package.json, and Dockerfile.
    /// </summary>
    /// <param name="directoryPath">The path to the directory to scan.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the scan results.</returns>
    public async Task<ScanResult> ScanDirectoryAsync(string directoryPath)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);

        var result = new ScanResult();

        try
        {
            if (!Directory.Exists(directoryPath))
            {
                _logger.LogWarning("Directory not found: {DirectoryPath}", directoryPath);
                result.Error = "Directory not found";
                return result;
            }

            _logger.LogInformation("Starting directory scan: {DirectoryPath}", directoryPath);

            await Task.Run(() =>
            {
                var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
                result.TotalFiles = files.Length;

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);

                    if (fileName.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
                    {
                        result.CsProjFiles.Add(file);
                    }
                    else if (fileName.Equals("package.json", StringComparison.OrdinalIgnoreCase))
                    {
                        result.PackageJsonFiles.Add(file);
                    }
                    else if (fileName.Equals("Dockerfile", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Dockerfiles.Add(file);
                    }
                }
            }).ConfigureAwait(false);

            _logger.LogInformation(
                "Scan completed. Found {TotalFiles} total files, {CsProjCount} .csproj, {PackageJsonCount} package.json, {DockerfileCount} Dockerfile",
                result.TotalFiles,
                result.CsProjFiles.Count,
                result.PackageJsonFiles.Count,
                result.Dockerfiles.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning directory: {DirectoryPath}", directoryPath);
            result.Error = ex.Message;
        }

        return result;
    }
}
