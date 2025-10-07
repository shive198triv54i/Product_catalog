import { createAsyncThunk } from "@reduxjs/toolkit";
import axiosInstance from "../../api/axiosInstance";
import type {
  Product,
  ProductCreateRequest,
  ProductUpdateRequest,
} from "../../types/productTypes";

export const fetchProducts = createAsyncThunk<Product[]>(
  "product/fetchAll",
  async (_, thunkAPI) => {
    try {
      const response = await axiosInstance.get("/Products");
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(
        error.response?.data || "Failed to fetch products"
      );
    }
  }
);

export const addProduct = createAsyncThunk<Product, ProductCreateRequest>(
  "product/add",
  async (data, thunkAPI) => {
    try {
      const response = await axiosInstance.post("/Products", data);
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(
        error.response?.data || "Failed to add product"
      );
    }
  }
);

export const updateProduct = createAsyncThunk<Product, ProductUpdateRequest>(
  "product/update",
  async (data, thunkAPI) => {
    try {
      const response = await axiosInstance.put(`/Products/${data.id}`, data);
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(
        error.response?.data || "Failed to update product"
      );
    }
  }
);

export const deleteProduct = createAsyncThunk<number, number>(
  "product/delete",
  async (id, thunkAPI) => {
    try {
      await axiosInstance.delete(`/Products/${id}`);
      return id;
    } catch (error) {
      return thunkAPI.rejectWithValue(
        error.response?.data || "Failed to delete product"
      );
    }
  }
);
