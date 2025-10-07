import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import type {
  LoginRequest,
  RegisterRequest,
  AuthState,
} from "../../types/userTypes";
import axios from "axios";

const API_URL =
  import.meta.env.VITE_API_URL || "https://localhost:7097/api/auth";

export const login = createAsyncThunk(
  "auth/login",
  async (data: LoginRequest, thunkAPI) => {
    try {
      const res = await axios.post(`${API_URL}/login`, data);
      if (res.data.token) localStorage.setItem("token", res.data.token);
      if (res.data.user)
        localStorage.setItem("user", JSON.stringify(res.data.user));

      return res.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(
        error.response?.data?.message || "Login failed"
      );
    }
  }
);


export const register = createAsyncThunk(
  "auth/register",
  async (data: RegisterRequest, thunkAPI) => {
    try {
      const res = await axios.post(`${API_URL}/register`, data);

      if (res.data.token) localStorage.setItem("token", res.data.token);
      if (res.data.user)
        localStorage.setItem("user", JSON.stringify(res.data.user));

      return res.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(
        error.response?.data?.message || "Registration failed"
      );
    }
  }
);


export const logout = createAsyncThunk("auth/logout", async () => {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  return null;
});


let storedUser = null;
try {
  const userData = localStorage.getItem("user");
  if (userData && userData !== "undefined" && userData !== "null") {
    storedUser = JSON.parse(userData);
  }
} catch {
  storedUser = null;
}

const initialState: AuthState = {
  user: storedUser,
  token: localStorage.getItem("token") || "",
  loading: false,
  error: null,
};


const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(login.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload.user;
        state.token = action.payload.token;
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(register.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(register.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload.user;
        state.token = action.payload.token;
      })
      .addCase(register.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(logout.fulfilled, (state) => {
        state.user = null;
        state.token = "";
      });
  },
});

export const { clearError } = authSlice.actions;
export default authSlice.reducer;
