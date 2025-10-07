export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data: T;
}

export interface PaginationMeta {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
}

export interface PagedResponse<T> {
  items: T[];
  meta: PaginationMeta;
}
