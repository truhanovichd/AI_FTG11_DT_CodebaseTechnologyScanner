/**
 * Results page - Display scan results
 */
export function Results() {
  return (
    <div>
      <h2>Scan Results</h2>
      <p>Results will be displayed here after running a scan.</p>
      <div style={{ padding: '20px', border: '1px solid #ccc', marginTop: '20px' }}>
        <h3>Summary</h3>
        <p>Total Files: 0</p>
        <p>C# Projects (.csproj): 0</p>
        <p>Node.js Projects (package.json): 0</p>
        <p>Docker Files: 0</p>
      </div>
    </div>
  );
}
