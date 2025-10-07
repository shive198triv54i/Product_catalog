import type { CartItem } from "./cartTypes";
import type { User } from "./userTypes";

export interface Order {
  id: number;
  userId: number;
  user?: User;
  orderDate: string;
  totalAmount: number;
  items: CartItem[];
  shippingAddress: ShippingAddress;
}

export interface ShippingAddress {
  name: string;
  phone: string;
  pincode: string;
  city: string;
  state: string;
}

export interface OrderState {
  orders: Order[];
  currentOrder: Order | null;
  loading: boolean;
  error: string | null;
}
