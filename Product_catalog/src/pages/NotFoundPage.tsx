import React from "react";
import { Link } from "react-router-dom";

const NotFoundPage: React.FC = () => {
  return (
    <div style={{ textAlign: "center", padding: "5rem" }}>
      <h1 style={{ fontSize: "4rem", marginBottom: "1rem" }}>404</h1>
      <h2 style={{ marginBottom: "1rem" }}>Page Not Found</h2>
      <p style={{ marginBottom: "2rem" }}>
        The page you are looking for does not exist.
      </p>
      <Link
        to="/"
        style={{
          padding: "0.75rem 1.5rem",
          backgroundColor: "#007BFF",
          color: "#fff",
          borderRadius: "0.5rem",
          textDecoration: "none",
          fontWeight: "bold",
        }}
      >
        Go to Home
      </Link>
    </div>
  );
};

export default NotFoundPage;
