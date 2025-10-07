import { createAsyncThunk } from '@reduxjs/toolkit';
import type { CartItem } from '../../types/cartTypes';
import axiosInstance from '../../api/axiosInstance';

export const checkoutCart = createAsyncThunk(
  'cart/checkout',
  async (items: CartItem[], thunkAPI) => {
    try {
      const response = await axiosInstance.post('/orders/checkout', { items });
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response?.data || 'Checkout failed');
    }
  }
);