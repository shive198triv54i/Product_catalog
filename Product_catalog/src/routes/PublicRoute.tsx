import React, { type JSX } from "react";
import { Navigate } from "react-router-dom";

interface PublicRouteProps {
  element: JSX.Element;
}


const hasValidToken = (): boolean => {
  const token = localStorage.getItem("token");
  if (!token) return false;
  const t = token.trim();
  
  if (t === "" || t.length < 8) return false;
  if (t.toLowerCase().includes("undefined") || t.toLowerCase().includes("null"))
    return false;
  return true;
};

const PublicRoute: React.FC<PublicRouteProps> = ({ element }) => {
  const valid = hasValidToken();
  
  try {
    if (typeof console !== "undefined" && typeof console.debug === "function") {
      console.debug(
        "[PublicRoute] token=",
        localStorage.getItem("token"),
        "valid=",
        valid
      );
    }
  } catch (err) {
    
    void err;
  }
  return valid ? <Navigate to="/home" replace /> : element;
};

export default PublicRoute;
