import axiosInstance from "./axiosInstance";
import type {
  LoginRequest,
  RegisterRequest,
  AuthResponse,
} from "../types/userTypes";

export const loginUser = async (data: LoginRequest): Promise<AuthResponse> => {
  const response = await axiosInstance.post("/Auth/login", data);
  return response.data;
};

export const registerUser = async (
  data: RegisterRequest
): Promise<AuthResponse> => {
  const response = await axiosInstance.post("/Auth/register", data);
  return response.data;
};
