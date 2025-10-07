import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import type { RootState, AppDispatch } from "../app/store";
import { fetchProducts } from "../features/product/productThunks";

const CategoryPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const categoryId = id ? parseInt(id, 10) : null;
  const dispatch: AppDispatch = useDispatch();
  const { items, loading, error } = useSelector((s: RootState) => s.product);

  useEffect(() => {
    if (items.length === 0) dispatch(fetchProducts());
  }, [dispatch, items.length]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  const filtered = categoryId
    ? items.filter((p) => p.categoryId === categoryId)
    : items;

  return (
    <div>
      <h1>Products {categoryId ? `in category ${categoryId}` : ""}</h1>
      <div className="grid">
        {filtered.map((p) => (
          <div key={p.id} className="card">
            <h3>{p.name}</h3>
            <p>₹{p.price}</p>
          </div>
        ))}
        {filtered.length === 0 && <p>No products found for this category.</p>}
      </div>
    </div>
  );
};

export default CategoryPage;
