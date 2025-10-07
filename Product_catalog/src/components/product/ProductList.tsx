import React from 'react';
import type { Product } from '../../types/productTypes';
import ProductCard from './ProductCard';

interface ProductListProps {
  products: Product[];
}

const ProductList: React.FC<ProductListProps> = ({ products }) => (
  <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
    {products.map((product) => (
      <ProductCard key={product.id} product={product} />
    ))}
  </div>
);

export default ProductList;