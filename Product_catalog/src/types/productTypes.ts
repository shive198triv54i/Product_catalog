import type { Category } from "./categoryTypes";

export interface Product {
  id: number;
  name: string;
  description?: string;
  price: number;
  categoryId: number;
  category?: Category;
  imageUrl?: string;
  createdDate?: string;
}

export interface ProductCreateRequest {
  name: string;
  description?: string;
  price: number;
  categoryId: number;
  imageUrl?: string;
}

export interface ProductUpdateRequest extends ProductCreateRequest {
  id: number;
}

export interface ProductState {
  items: Product[];
  selectedProduct: Product | null;
  loading: boolean;
  error: string | null;
}
