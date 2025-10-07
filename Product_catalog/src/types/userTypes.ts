export interface User {
  id: number;
  name: string;
  email: string;
  role: "Admin" | "User";
  createdAt?: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface AuthState {
  user: User | null;
  token: string;
  loading: boolean;
  error: string | null;
}
