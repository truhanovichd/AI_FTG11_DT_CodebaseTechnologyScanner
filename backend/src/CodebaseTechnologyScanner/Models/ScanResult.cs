namespace CodebaseTechnologyScanner.Models;

/// <summary>
/// Represents the results of scanning a directory for technology-related files.
/// </summary>
public class ScanResult
{
    /// <summary>
    /// Gets or sets the list of C# project files (.csproj) found during the scan.
    /// </summary>
    public List<string> CsProjFiles { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of package.json files found during the scan.
    /// </summary>
    public List<string> PackageJsonFiles { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of Dockerfile files found during the scan.
    /// </summary>
    public List<string> Dockerfiles { get; set; } = new();

    /// <summary>
    /// Gets or sets the total number of files found in the scanned directory.
    /// </summary>
    public int TotalFiles { get; set; }

    /// <summary>
    /// Gets or sets the error message if the scan encountered any errors.
    /// </summary>
    public string? Error { get; set; }
}
