import React from 'react';
import type { Product } from '../../types/productTypes';
import { Link } from 'react-router-dom';
import { formatPrice } from '../../utils/helpers';

interface ProductCardProps {
  product: Product;
}

const ProductCard: React.FC<ProductCardProps> = ({ product }) => (
  <div className="border p-4 rounded shadow hover:shadow-lg transition">
    <h3 className="font-semibold">{product.name}</h3>
    <p>{formatPrice(product.price)}</p>
    <Link
      to={`/product/${product.id}`}
      className="text-blue-600 mt-2 inline-block"
    >
      View Details
    </Link>
  </div>
);

export default ProductCard;