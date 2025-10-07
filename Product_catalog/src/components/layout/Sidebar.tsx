import React, { useEffect } from "react";
import { Link } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import type { RootState, AppDispatch } from "../../app/store";
import { fetchCategories } from "../../features/category/categoryThunks";

const Sidebar: React.FC = () => {
  const dispatch: AppDispatch = useDispatch();
  const { items, loading } = useSelector((state: RootState) => state.category);

  useEffect(() => {
    dispatch(fetchCategories());
  }, [dispatch]);

  return (
    <aside className="sidebar">
      <h2>Categories</h2>
      {loading && <p>Loading...</p>}
      <ul>
        <li>
          <Link to="/products">All Products</Link>
        </li>
        {items.map((cat) => (
          <li key={cat.id}>
            <Link to={`/category/${cat.id}`}>{cat.name}</Link>
          </li>
        ))}
      </ul>
    </aside>
  );
};

export default Sidebar;
