import { createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";
import type {
  LoginRequest,
  RegisterRequest,
  AuthResponse,
} from "../../types/userTypes";

const API_URL =
  import.meta.env.VITE_API_URL || "https://localhost:7097/api/auth";

export const loginUser = createAsyncThunk<
  AuthResponse,
  LoginRequest,
  { rejectValue: string }
>("Auth/loginUser", async (credentials, { rejectWithValue }) => {
  try {
    const response = await axios.post<AuthResponse>(
      `${API_URL}/login`,
      credentials
    );
    const data = response.data;

    localStorage.setItem("token", data.token);
    localStorage.setItem("user", JSON.stringify(data.user));
    return data;
  } catch (error) {
    return rejectWithValue(
      error.response?.data?.message || "Failed to login. Please try again."
    );
  }
});

export const registerUser = createAsyncThunk<
  AuthResponse,
  RegisterRequest,
  { rejectValue: string }
>("auth/registerUser", async (formData, { rejectWithValue }) => {
  try {
    const response = await axios.post<AuthResponse>(
      `${API_URL}/register`,
      formData
    );
    const data = response.data;
    return data;
  } catch (error) {
    return rejectWithValue(
      error.response?.data?.message || "Registration failed. Please try again."
    );
  }
});

export const logoutUser = createAsyncThunk("auth/logoutUser", async () => {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  return null;
});
