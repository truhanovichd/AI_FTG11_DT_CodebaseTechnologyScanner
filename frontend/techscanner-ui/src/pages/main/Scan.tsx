import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Scan.css';

interface ScanResult {
  csProjFiles: string[];
  packageJsonFiles: string[];
  dockerfiles: string[];
  totalFiles: number;
  error?: string;
}

/**
 * Scan page - Input directory path for scanning
 * Handles form submission, loading states, error handling, and navigation
 */
export function Scan() {
  const navigate = useNavigate();
  const [directoryPath, setDirectoryPath] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const handleScan = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);

    try {
      if (!directoryPath.trim()) {
        throw new Error('Please enter a directory path');
      }

      const response = await fetch('/api/scan', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ directoryPath }),
      });

      if (!response.ok) {
        throw new Error(`Server error: ${response.statusText}`);
      }

      const data: ScanResult = await response.json();

      if (data.error) {
        throw new Error(data.error);
      }

      // Store results and navigate to Results page
      sessionStorage.setItem('scanResults', JSON.stringify(data));
      setSuccess(true);

      // Navigate after a brief delay to show success message
      setTimeout(() => {
        navigate('/results');
      }, 500);
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to scan directory';
      setError(errorMessage);
      console.error('Scan failed:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="scan-container">
      <h2>Scan Directory</h2>
      <p className="scan-description">Enter the path to a directory to scan for technology files.</p>

      {error && <div className="error-message">{error}</div>}
      {success && <div className="success-message">Scan completed successfully! Redirecting...</div>}

      <form onSubmit={handleScan} className="scan-form">
        <div className="form-group">
          <label htmlFor="dirname">Directory Path:</label>
          <input
            id="dirname"
            type="text"
            value={directoryPath}
            onChange={(e) => setDirectoryPath(e.target.value)}
            placeholder="e.g., C:/projects/myapp or /home/user/projects"
            disabled={loading}
            autoFocus
          />
        </div>

        <button type="submit" disabled={loading} className="submit-button">
          {loading ? (
            <>
              <span className="spinner"></span>
              Scanning...
            </>
          ) : (
            'Start Scan'
          )}
        </button>
      </form>

      <div className="scan-info">
        <h3>Scan Information</h3>
        <p>This tool will scan the directory for:</p>
        <ul>
          <li>C# Project files (.csproj)</li>
          <li>Node.js Package files (package.json)</li>
          <li>Docker configuration (Dockerfile)</li>
        </ul>
      </div>
    </div>
  );
}
