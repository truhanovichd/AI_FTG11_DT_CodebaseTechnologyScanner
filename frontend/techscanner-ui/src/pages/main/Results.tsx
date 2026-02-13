import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Results.css';

interface ScanResult {
  csProjFiles: string[];
  packageJsonFiles: string[];
  dockerfiles: string[];
  totalFiles: number;
  error?: string;
}

/**
 * Results page - Display scan results
 */
export function Results() {
  const navigate = useNavigate();
  const [results, setResults] = useState<ScanResult | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Retrieve results from sessionStorage
    const storedResults = sessionStorage.getItem('scanResults');
    if (storedResults) {
      try {
        const parsedResults = JSON.parse(storedResults);
        setResults(parsedResults);
      } catch (error) {
        console.error('Failed to parse scan results:', error);
      }
    }
    setLoading(false);
  }, []);

  if (loading) {
    return (
      <div className="results-container">
        <div className="loading">Loading results...</div>
      </div>
    );
  }

  if (!results) {
    return (
      <div className="results-container">
        <div className="no-results">
          <p>No scan results available.</p>
          <button onClick={() => navigate('/scan')}>Go back to Scan</button>
        </div>
      </div>
    );
  }

  return (
    <div className="results-container">
      <h2>Scan Results</h2>

      <div className="results-summary">
        <div className="summary-card">
          <div className="summary-value">{results.totalFiles}</div>
          <div className="summary-label">Total Files</div>
        </div>
        <div className="summary-card">
          <div className="summary-value">{results.csProjFiles.length}</div>
          <div className="summary-label">C# Projects</div>
        </div>
        <div className="summary-card">
          <div className="summary-value">{results.packageJsonFiles.length}</div>
          <div className="summary-label">Node.js Projects</div>
        </div>
        <div className="summary-card">
          <div className="summary-value">{results.dockerfiles.length}</div>
          <div className="summary-label">Docker Files</div>
        </div>
      </div>

      <div className="results-details">
        {results.csProjFiles.length > 0 && (
          <div className="file-section">
            <h3>C# Project Files (.csproj)</h3>
            <ul className="file-list">
              {results.csProjFiles.map((file, index) => (
                <li key={index}>{file}</li>
              ))}
            </ul>
          </div>
        )}

        {results.packageJsonFiles.length > 0 && (
          <div className="file-section">
            <h3>Node.js Package Files (package.json)</h3>
            <ul className="file-list">
              {results.packageJsonFiles.map((file, index) => (
                <li key={index}>{file}</li>
              ))}
            </ul>
          </div>
        )}

        {results.dockerfiles.length > 0 && (
          <div className="file-section">
            <h3>Docker Files (Dockerfile)</h3>
            <ul className="file-list">
              {results.dockerfiles.map((file, index) => (
                <li key={index}>{file}</li>
              ))}
            </ul>
          </div>
        )}

        {results.csProjFiles.length === 0 &&
          results.packageJsonFiles.length === 0 &&
          results.dockerfiles.length === 0 && (
            <div className="no-files">
              <p>No technology files found in the selected directory.</p>
            </div>
          )}
      </div>

      <div className="results-actions">
        <button onClick={() => navigate('/scan')} className="action-button">
          Scan Another Directory
        </button>
        <button onClick={() => navigate('/')} className="action-button secondary">
          Back to Home
        </button>
      </div>
    </div>
  );
}
