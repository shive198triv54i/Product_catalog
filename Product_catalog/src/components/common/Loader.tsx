import React from 'react';

const Loader: React.FC = () => (
  <div className="flex justify-center items-center py-10">
    <div className="loader border-4 border-blue-400 border-t-transparent rounded-full w-12 h-12 animate-spin"></div>
  </div>
);

export default Loader;