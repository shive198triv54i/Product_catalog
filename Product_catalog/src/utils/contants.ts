export const API_BASE_URL =
  import.meta.env.VITE_API_URL || "https://localhost:7097/api";

export const USER_ROLES = {
  ADMIN: "Admin",
  USER: "User",
} as const;

export const LOCAL_STORAGE_KEYS = {
  USER: "user",
  TOKEN: "token",
};

export const COOKIE_KEYS = {
  CART: "cart_data",
  CHECKOUT: "checkout_info",
};
