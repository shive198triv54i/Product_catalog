import React from 'react';

interface ProductFilterProps {
  categories: { id: number; name: string }[];
  selectedCategory: number | null;
  onChange: (id: number | null) => void;
}

const ProductFilter: React.FC<ProductFilterProps> = ({
  categories,
  selectedCategory,
  onChange,
}) => (
  <div className="mb-4">
    <select
      value={selectedCategory ?? ''}
      onChange={(e) =>
        onChange(e.target.value ? Number(e.target.value) : null)
      }
      className="px-3 py-2 border rounded"
    >
      <option value="">All Categories</option>
      {categories.map((cat) => (
        <option key={cat.id} value={cat.id}>
          {cat.name}
        </option>
      ))}
    </select>
  </div>
);

export default ProductFilter;