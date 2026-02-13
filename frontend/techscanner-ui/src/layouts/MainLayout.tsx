import { Outlet, Link } from 'react-router-dom';
import './MainLayout.css';

/**
 * Main layout component with primary navigation
 * Displays navigation for Home, Scan, and Results pages
 */
export function MainLayout() {
  return (
    <div className="main-layout">
      <nav className="main-nav">
        <div className="nav-brand">
          <h1>Tech Scanner</h1>
        </div>
        <ul className="nav-links">
          <li>
            <Link to="/">Home</Link>
          </li>
          <li>
            <Link to="/scan">Scan</Link>
          </li>
          <li>
            <Link to="/results">Results</Link>
          </li>
        </ul>
      </nav>
      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
}
