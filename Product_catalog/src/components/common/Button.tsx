import React from 'react';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  label: string;
}

const Button: React.FC<ButtonProps> = ({ label, ...props }) => {
  return (
    <button
      {...props}
      className={`px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition ${props.className || ''}`}
    >
      {label}
    </button>
  );
};

export default Button;