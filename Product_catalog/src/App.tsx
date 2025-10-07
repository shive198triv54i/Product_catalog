import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  useLocation,
} from "react-router-dom";


import Header from "./components/layout/Header";
import Footer from "./components/layout/Footer";
import Sidebar from "./components/layout/Sidebar";
import ProtectedRoute from "./routes/ProtectedRoute";


import HomePage from "./pages/HomePage";
import ProductPage from "./pages/ProductPage";
import ProductDetailPage from "./pages/ProductDetailPage";
import CategoryPage from "./pages/CategoryPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import PublicRoute from "./routes/PublicRoute";
import NotFoundPage from "./pages/NotFoundPage";
import LandingPage from "./pages/LandingPage";

import "./App.css"; 

const App: React.FC = () => {
  return (
    <Router>
      <AppRouter />
    </Router>
  );
};

const AppRouter: React.FC = () => {
  const location = useLocation();
  
  const publicPaths = ["/login", "/register"];
  const isPublic = publicPaths.includes(location.pathname);


  return (
    <div className="app-shell">
      <Header />
      <div className="app-body">
        {!isPublic && <Sidebar />}
        <main className="main">
          <Routes>
            {/* TEMP: render LandingPage directly to verify routing */}
            <Route path="/" element={<LandingPage />} />

            <Route
              path="/login"
              element={<PublicRoute element={<LoginPage />} />}
            />
            <Route
              path="/register"
              element={<PublicRoute element={<RegisterPage />} />}
            />
            <Route
              path="/home"
              element={<ProtectedRoute element={<HomePage />} />}
            />
            <Route
              path="/products"
              element={<ProtectedRoute element={<ProductPage />} />}
            />
            <Route
              path="/product/:id"
              element={<ProtectedRoute element={<ProductDetailPage />} />}
            />
            <Route
              path="/category/:id"
              element={<ProtectedRoute element={<CategoryPage />} />}
            />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </main>
      </div>
      {!isPublic && <Footer />}
    </div>
  );
};

export default App;
