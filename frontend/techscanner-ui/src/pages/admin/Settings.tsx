import { useState } from 'react';

/**
 * Admin Settings page - Manage application settings
 */
export function Settings() {
  const [settings, setSettings] = useState({
    maxScanSize: 1000,
    scanTimeout: 60,
    enableLogging: true,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type } = e.target;
    setSettings(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? (e.target as HTMLInputElement).checked : value,
    }));
  };

  const handleSave = () => {
    console.log('Settings saved:', settings);
    alert('Settings saved successfully');
  };

  return (
    <div>
      <h2>Settings</h2>
      <form style={{ marginTop: '20px', maxWidth: '400px' }}>
        <div style={{ marginBottom: '15px' }}>
          <label htmlFor="maxsize">Max Scan Size (MB):</label>
          <input
            id="maxsize"
            type="number"
            name="maxScanSize"
            value={settings.maxScanSize}
            onChange={handleChange}
          />
        </div>
        <div style={{ marginBottom: '15px' }}>
          <label htmlFor="timeout">Scan Timeout (seconds):</label>
          <input
            id="timeout"
            type="number"
            name="scanTimeout"
            value={settings.scanTimeout}
            onChange={handleChange}
          />
        </div>
        <div style={{ marginBottom: '15px' }}>
          <label htmlFor="logging">
            <input
              id="logging"
              type="checkbox"
              name="enableLogging"
              checked={settings.enableLogging}
              onChange={handleChange}
            />
            Enable Logging
          </label>
        </div>
        <button type="button" onClick={handleSave}>Save Settings</button>
      </form>
    </div>
  );
}
