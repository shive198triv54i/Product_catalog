import type { Product } from "./productTypes";

export interface CartItem {
  product: Product;
  quantity: number;
}

export interface CartState {
  items: CartItem[];
  totalQuantity: number;
  totalPrice: number;
}
