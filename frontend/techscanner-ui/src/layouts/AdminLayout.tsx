import { Outlet, Link } from 'react-router-dom';
import './AdminLayout.css';

/**
 * Admin layout component with administrative navigation
 * Displays navigation for Dashboard, Logs, and Settings pages
 */
export function AdminLayout() {
  return (
    <div className="admin-layout">
      <nav className="admin-nav">
        <div className="nav-brand">
          <h1>Admin Panel</h1>
        </div>
        <ul className="nav-links">
          <li>
            <Link to="/admin/dashboard">Dashboard</Link>
          </li>
          <li>
            <Link to="/admin/logs">Logs</Link>
          </li>
          <li>
            <Link to="/admin/settings">Settings</Link>
          </li>
        </ul>
      </nav>
      <main className="admin-content">
        <Outlet />
      </main>
    </div>
  );
}
