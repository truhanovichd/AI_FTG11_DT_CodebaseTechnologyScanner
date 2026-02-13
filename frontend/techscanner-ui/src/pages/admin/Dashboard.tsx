/**
 * Admin Dashboard page - Overview of system statistics
 */
export function Dashboard() {
  return (
    <div>
      <h2>Dashboard</h2>
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px', marginTop: '20px' }}>
        <div style={{ padding: '20px', border: '1px solid #ccc' }}>
          <h3>Total Scans</h3>
          <p style={{ fontSize: '24px', fontWeight: 'bold' }}>0</p>
        </div>
        <div style={{ padding: '20px', border: '1px solid #ccc' }}>
          <h3>Active Users</h3>
          <p style={{ fontSize: '24px', fontWeight: 'bold' }}>0</p>
        </div>
        <div style={{ padding: '20px', border: '1px solid #ccc' }}>
          <h3>System Health</h3>
          <p style={{ fontSize: '24px', fontWeight: 'bold' }}>Good</p>
        </div>
        <div style={{ padding: '20px', border: '1px solid #ccc' }}>
          <h3>Last Update</h3>
          <p>-</p>
        </div>
      </div>
    </div>
  );
}
