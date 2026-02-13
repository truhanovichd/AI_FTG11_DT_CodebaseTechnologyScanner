namespace CodebaseTechnologyScanner.Models;

/// <summary>
/// Represents the results of a completed scan operation.
/// </summary>
public class ScanResult
{
    /// <summary>
    /// Gets or sets the timestamp when the scan started.
    /// </summary>
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the total number of files scanned during the operation.
    /// </summary>
    public int FilesScanned { get; set; }

    /// <summary>
    /// Gets or sets the list of detected technology items found during the scan.
    /// </summary>
    public List<DetectedItem> Items { get; set; } = new();
}
