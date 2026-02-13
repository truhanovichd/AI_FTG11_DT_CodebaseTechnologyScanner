import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { MainLayout } from './layouts/MainLayout';
import { AdminLayout } from './layouts/AdminLayout';
import { Home } from './pages/main/Home';
import { Scan } from './pages/main/Scan';
import { Results } from './pages/main/Results';
import { Dashboard } from './pages/admin/Dashboard';
import { Logs } from './pages/admin/Logs';
import { Settings } from './pages/admin/Settings';
import './App.css';

/**
 * Main App component with routing configuration
 * Sets up two layout routes: MainLayout and AdminLayout with their respective child routes
 */
function App() {
  return (
    <Router>
      <Routes>
        {/* Main Layout Routes */}
        <Route element={<MainLayout />}>
          <Route path="/" element={<Home />} />
          <Route path="/scan" element={<Scan />} />
          <Route path="/results" element={<Results />} />
        </Route>

        {/* Admin Layout Routes */}
        <Route path="/admin" element={<AdminLayout />}>
          <Route path="dashboard" element={<Dashboard />} />
          <Route path="logs" element={<Logs />} />
          <Route path="settings" element={<Settings />} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
