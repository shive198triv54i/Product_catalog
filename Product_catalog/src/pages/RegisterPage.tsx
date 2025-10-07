import React, { useEffect, useState } from "react";
import { useAuth } from "../hooks/useAuth";
import { useNavigate } from "react-router-dom";

const RegisterPage: React.FC = () => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { registerUser, loading, error, token } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    registerUser(name, email, password);
  };

  useEffect(() => {
    if (token) {
      navigate("/home", { replace: true });
    }
  }, [token, navigate]);

  return (
    <form onSubmit={handleSubmit} className="form">
      <h1>Register</h1>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <div className="field">
        <label htmlFor="name">Name</label>
        <input
          id="name"
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
      </div>
      <div className="field">
        <label htmlFor="email">Email</label>
        <input
          id="email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </div>
      <div className="field">
        <label htmlFor="password">Password</label>
        <input
          id="password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>
      <div className="actions">
        <button type="submit" className="btn btn-primary" disabled={loading}>
          Register
        </button>
        <button
          type="button"
          className="btn btn-outline"
          onClick={() => {
            setName("");
            setEmail("");
            setPassword("");
          }}
        >
          Clear
        </button>
      </div>
    </form>
  );
};

export default RegisterPage;
