import { useSelector, useDispatch } from "react-redux";
import type { RootState, AppDispatch } from "../app/store";
import {
  login,
  register,
  logout,
  clearError,
} from "../features/auth/authSlice";

export const useAuth = () => {
  const dispatch: AppDispatch = useDispatch();
  const { user, token, loading, error } = useSelector(
    (state: RootState) => state.auth
  );

  const loginUser = (email: string, password: string) =>
    dispatch(login({ email, password }));

  const registerUser = (name: string, email: string, password: string) =>
    dispatch(register({ name, email, password }));

  const logoutUser = () => dispatch(logout());
  const clearAuthError = () => dispatch(clearError());

  return {
    user,
    token,
    loading,
    error,
    loginUser,
    registerUser,
    logoutUser,
    clearAuthError,
  };
};
