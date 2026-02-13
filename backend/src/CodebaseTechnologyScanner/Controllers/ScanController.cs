using CodebaseTechnologyScanner.Models;
using CodebaseTechnologyScanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodebaseTechnologyScanner.Controllers;

/// <summary>
/// Provides endpoints for scanning directories to detect technology-related files.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ScanController(IFileScanner fileScanner, ILogger<ScanController> logger) : ControllerBase
{
    private readonly IFileScanner _fileScanner = fileScanner ?? throw new ArgumentNullException(nameof(fileScanner));
    private readonly ILogger<ScanController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Scans a directory for technology-related files such as .csproj, package.json, and Dockerfile.
    /// </summary>
    /// <param name=\"request\">The scan request containing the directory path.</param>
    /// <returns>A ScanResult object containing the detected technologies and file count.</returns>
    /// <response code=\"200\">Scan completed successfully</response>
    /// <response code=\"400\">Invalid request or path</response>
    /// <response code=\"500\">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScanResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ScanResult>> Scan([FromBody] ScanRequest request)
    {
        // Validate request
        if (request is null)
        {
            _logger.LogWarning(\"Scan request is null\");
            return BadRequest(new ProblemDetails
            {
                Title = \"Invalid Request\",
                Detail = \"Request body cannot be empty.\",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        // Validate path
        if (string.IsNullOrWhiteSpace(request.Path))
        {
            _logger.LogWarning(\"Scan request has empty or whitespace path\");
            return BadRequest(new ProblemDetails
            {
                Title = \"Invalid Path\",
                Detail = \"The path parameter is required and cannot be empty.\",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            _logger.LogInformation(\"Scan request received for path: {Path}\", request.Path);
            
            var result = await _fileScanner.ScanAsync(request).ConfigureAwait(false);
            
            _logger.LogInformation(
                \"Scan completed. Files scanned: {FilesScanned}, Items detected: {ItemsCount}\",
                result.FilesScanned,
                result.Items.Count);
            
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, \"Argument error during scan\");
            return BadRequest(new ProblemDetails
            {
                Title = \"Invalid Argument\",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, \"Unexpected error during scan for path: {Path}\", request.Path);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = \"Internal Server Error\",
                Detail = \"An unexpected error occurred while scanning the directory.\",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }
}
