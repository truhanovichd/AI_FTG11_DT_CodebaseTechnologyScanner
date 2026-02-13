namespace CodebaseTechnologyScanner.Models;

/// <summary>
/// Represents a detected technology item found during a scan.
/// </summary>
public class DetectedItem
{
    /// <summary>
    /// Gets or sets the kind of technology (e.g., 'CSharp', 'Node.js', 'Docker').
    /// </summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the detected item (e.g., project name or file name).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version of the detected technology, if available.
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Gets or sets the evidence or path that confirms this detection.
    /// </summary>
    public string Evidence { get; set; } = string.Empty;
}
