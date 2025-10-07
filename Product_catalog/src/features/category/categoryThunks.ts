import { createAsyncThunk } from "@reduxjs/toolkit";
import type { Category } from "../../types/categoryTypes";
import * as categoryApi from "../../api/categoryApi";

export const fetchCategories = createAsyncThunk<Category[]>(
  "category/fetchAll",
  async (_, thunkAPI) => {
    try {
      const data = await categoryApi.getAllCategories();
      return data;
    } catch (error) {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const err: any = error;
      return thunkAPI.rejectWithValue(
        err.response?.data || "Failed to fetch categories"
      );
    }
  }
);
