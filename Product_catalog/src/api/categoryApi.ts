import axiosInstance from "./axiosInstance";
import type { Category } from "../types/categoryTypes";

export const getAllCategories = async (): Promise<Category[]> => {
  const response = await axiosInstance.get("/Categories");
  return response.data;
};

export const getCategoryById = async (id: number): Promise<Category> => {
  const response = await axiosInstance.get(`/Categories/${id}`);
  return response.data;
};

export const createCategory = async (
  data: Partial<Category>
): Promise<Category> => {
  const response = await axiosInstance.post("/Categories", data);
  return response.data;
};

export const updateCategory = async (
  id: number,
  data: Partial<Category>
): Promise<void> => {
  await axiosInstance.put(`/Categories/${id}`, data);
};

export const deleteCategory = async (id: number): Promise<void> => {
  await axiosInstance.delete(`/Categories/${id}`);
};

export default {
  getAllCategories,
  getCategoryById,
  createCategory,
  updateCategory,
  deleteCategory,
};
