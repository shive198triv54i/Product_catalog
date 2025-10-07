import React from "react";

const Footer: React.FC = () => (
  <footer className="app-footer">
    <p>
      &copy; {new Date().getFullYear()} Product Catalog. All rights reserved.
    </p>
  </footer>
);

export default Footer;
