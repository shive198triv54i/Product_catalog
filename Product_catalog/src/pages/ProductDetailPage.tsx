import React from "react";
import { useParams } from "react-router-dom";
import { useSelector } from "react-redux";
import type { RootState } from "../app/store";

const ProductDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const product = useSelector((state: RootState) =>
    state.product.items.find((p) => p.id === Number(id))
  );

  if (!product) return <p>Product not found.</p>;

  return (
    <div className="card">
      <h1>{product.name}</h1>
      <p>{product.description}</p>
      <p>Price: ₹{product.price}</p>
      <div className="actions">
        <button className="btn btn-primary">Add to Cart</button>
        <button className="btn btn-outline">Buy Now</button>
      </div>
    </div>
  );
};

export default ProductDetailPage;
