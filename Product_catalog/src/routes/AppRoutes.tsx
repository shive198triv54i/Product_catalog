import { Route, Routes, Navigate } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import HomePage from "../pages/HomePage";
import LoginPage from "../pages/LoginPage";
import ProductPage from "../pages/ProductPage";
import PublicRoute from "./PublicRoute";

const AppRoutes = () => (
  <Routes>
    <Route path="/" element={<ProtectedRoute element={<HomePage />} />} />
    <Route path="/login" element={<PublicRoute element={<LoginPage />} />} />
    <Route
      path="/products"
      element={<ProtectedRoute element={<ProductPage />} />}
    />
    <Route path="*" element={<Navigate to="/" replace />} />
  </Routes>
);

export default AppRoutes;
