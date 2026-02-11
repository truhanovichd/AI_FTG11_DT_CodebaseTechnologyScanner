using CodebaseTechnologyScanner.Models;

namespace CodebaseTechnologyScanner.Services;

public interface IFileScanner
{
    Task<ScanResult> ScanDirectoryAsync(string directoryPath);
}
