export interface Category {
  id: number;
  name: string;
}

export interface CategoryState {
  items: Category[];
  loading: boolean;
  error: string | null;
}
