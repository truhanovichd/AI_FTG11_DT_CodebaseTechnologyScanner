import { useState } from 'react';

/**
 * Scan page - Input directory path for scanning
 */
export function Scan() {
  const [directoryPath, setDirectoryPath] = useState('');
  const [loading, setLoading] = useState(false);

  const handleScan = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);
    try {
      const response = await fetch('/api/scan', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ directoryPath }),
      });
      const data = await response.json();
      console.log('Scan results:', data);
    } catch (error) {
      console.error('Scan failed:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Scan Directory</h2>
      <form onSubmit={handleScan}>
        <div>
          <label htmlFor="dirname">Directory Path:</label>
          <input
            id="dirname"
            type="text"
            value={directoryPath}
            onChange={(e) => setDirectoryPath(e.target.value)}
            placeholder="Enter directory path"
            required
          />
        </div>
        <button type="submit" disabled={loading}>
          {loading ? 'Scanning...' : 'Scan'}
        </button>
      </form>
    </div>
  );
}
