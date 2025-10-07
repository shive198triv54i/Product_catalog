import axiosInstance from "./axiosInstance";
import type { Product } from "../types/productTypes";

export const getAllProducts = async (): Promise<Product[]> => {
  const response = await axiosInstance.get("/Products");
  return response.data;
};

export const getProductById = async (id: string): Promise<Product> => {
  const response = await axiosInstance.get(`/Products/${id}`);
  return response.data;
};
