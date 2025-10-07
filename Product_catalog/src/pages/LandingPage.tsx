import React from "react";
import { Link } from "react-router-dom";

const LandingPage: React.FC = () => {
  return (
    <div className="landing landing-centered">
      <div className="landing-card">
        <h1>Welcome</h1>
        <p>Please choose an option to continue</p>
        <div className="landing-actions">
          <Link className="btn" to="/login">
            Login
          </Link>
          <Link className="btn btn-outline" to="/register">
            Sign Up
          </Link>
        </div>
      </div>
    </div>
  );
};

export default LandingPage;
