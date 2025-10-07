import React, { type JSX } from "react";
import { Navigate } from "react-router-dom";

interface ProtectedRouteProps {
  element: JSX.Element;
}

const hasValidToken = (): boolean => {
  const token = localStorage.getItem("token");
  if (!token) return false;
  const t = token.trim();
  return t !== "" && t !== "undefined" && t !== "null";
};

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ element }) => {
  const valid = hasValidToken();
  try {
    if (typeof console !== "undefined" && typeof console.debug === "function") {
      console.debug(
        "[ProtectedRoute] token=",
        localStorage.getItem("token"),
        "valid=",
        valid
      );
    }
  } catch (err) {
    void err;
  }
  return valid ? element : <Navigate to="/login" replace />;
};

export default ProtectedRoute;
