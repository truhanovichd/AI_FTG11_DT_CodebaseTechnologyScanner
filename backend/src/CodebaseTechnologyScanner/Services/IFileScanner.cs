using CodebaseTechnologyScanner.Models;

namespace CodebaseTechnologyScanner.Services;

/// <summary>
/// Defines the contract for scanning directories to identify technology-related files.
/// </summary>
public interface IFileScanner
{
    /// <summary>
    /// Asynchronously scans a directory for technology-related files such as .csproj, package.json, and Dockerfile.
    /// </summary>
    /// <param name="directoryPath">The path to the directory to scan.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the scan results.</returns>
    Task<ScanResult> ScanDirectoryAsync(string directoryPath);
}
