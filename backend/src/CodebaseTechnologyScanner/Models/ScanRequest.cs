namespace CodebaseTechnologyScanner.Models;

/// <summary>
/// Represents a scan request with the directory path to scan.
/// </summary>
public class ScanRequest
{
    /// <summary>
    /// Gets or sets the directory path to scan.
    /// </summary>
    public string Path { get; set; } = string.Empty;
}
