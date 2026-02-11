namespace CodebaseTechnologyScanner.Models;

public class ScanResult
{
    public List<string> CsProjFiles { get; set; } = new();
    public List<string> PackageJsonFiles { get; set; } = new();
    public List<string> Dockerfiles { get; set; } = new();
    public int TotalFiles { get; set; }
    public string? Error { get; set; }
}
