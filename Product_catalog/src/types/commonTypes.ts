export interface LoadingState {
  loading: boolean;
  error: string | null;
}

export interface FilterOption {
  key: string;
  value: string | number;
}

export interface DropdownOption {
  label: string;
  value: string | number;
}
