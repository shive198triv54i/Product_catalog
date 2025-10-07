import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import logo from "../../assets/logo.png";

const Header: React.FC = () => {
  const { user, token, logoutUser } = useAuth();
  const navigate = useNavigate();

  const [theme, setTheme] = useState<string>(() => {
    try {
      return localStorage.getItem("theme") || "light";
    } catch {
      return "light";
    }
  });

  useEffect(() => {
    try {
      document.documentElement.setAttribute("data-theme", theme);
      localStorage.setItem("theme", theme);
    } catch {
      // 
    }
  }, [theme]);

  const handleLogout = () => {
    logoutUser();
    navigate("/", { replace: true });
  };

  const toggleTheme = () => setTheme((t) => (t === "light" ? "dark" : "light"));

  return (
    <header className="app-header">
      <Link to="/" className="brand">
        <img src={logo} alt="logo" className="logo-small" />
        <span>Product Catalog</span>
      </Link>
      <nav className="nav">
        <button
          className="theme-toggle"
          onClick={toggleTheme}
          aria-label="Toggle theme"
        >
          {theme === "light" ? "🌞" : "🌙"}
        </button>
        {!token ? (
          <>
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
          </>
        ) : (
          <>
            <span className="nav-user">{user?.name ?? "User"}</span>
            <button onClick={handleLogout} className="btn-link">
              Logout
            </button>
          </>
        )}
      </nav>
    </header>
  );
};

export default Header;
