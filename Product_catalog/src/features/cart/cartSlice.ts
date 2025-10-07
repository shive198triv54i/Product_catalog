import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { CartItem, CartState } from "../../types/cartTypes";
import { checkoutCart } from "./cartThunks";

const initialState: CartState = {
  items: [],
  totalQuantity: 0,
  totalPrice: 0,
};

const cartSlice = createSlice({
  name: "cart",
  initialState,
  reducers: {
    addToCart: (state, action: PayloadAction<CartItem>) => {
      const existingItem = state.items.find(
        (i) => i.product.id === action.payload.product.id
      );

      if (existingItem) {
        existingItem.quantity += action.payload.quantity;
      } else {
        state.items.push(action.payload);
      }

      state.totalQuantity += action.payload.quantity;
      state.totalPrice +=
        action.payload.product.price * action.payload.quantity;
    },

    removeFromCart: (state, action: PayloadAction<number>) => {
      const item = state.items.find((i) => i.product.id === action.payload);
      if (item) {
        state.totalQuantity -= item.quantity;
        state.totalPrice -= item.product.price * item.quantity;
        state.items = state.items.filter(
          (i) => i.product.id !== action.payload
        );
      }
    },

    clearCart: (state) => {
      state.items = [];
      state.totalQuantity = 0;
      state.totalPrice = 0;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(checkoutCart.fulfilled, (state) => {
      state.items = [];
      state.totalQuantity = 0;
      state.totalPrice = 0;
    });
  },
});

export const { addToCart, removeFromCart, clearCart } = cartSlice.actions;
export default cartSlice.reducer;
