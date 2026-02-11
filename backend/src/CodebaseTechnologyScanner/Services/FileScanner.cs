using CodebaseTechnologyScanner.Models;

namespace CodebaseTechnologyScanner.Services;

public class FileScanner : IFileScanner
{
    public async Task<ScanResult> ScanDirectoryAsync(string directoryPath)
    {
        var result = new ScanResult();

        try
        {
            if (!Directory.Exists(directoryPath))
            {
                result.Error = "Directory not found";
                return result;
            }

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
            });
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
        }

        return result;
    }
}
