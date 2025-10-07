import { useSelector } from 'react-redux';
import type { RootState } from '../app/store';
import type { Product } from '../types/productTypes';

export const useCategoryFilter = (categoryId: number | null) => {
  const products = useSelector((state: RootState) => state.product.items);

  if (!categoryId) return products;
  return products.filter((product: Product) => product.categoryId === categoryId);
};