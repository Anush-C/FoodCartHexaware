// src/components/ConfirmDialog.js
import React from 'react';

const ConfirmBox = ({ isOpen, onClose, onConfirm, message }) => {
  if (!isOpen) return null; // Don't render if not open

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
      <div className="bg-white rounded-lg shadow-lg p-6 max-w-sm w-full">
        <h2 className="text-lg font-bold mb-4">Confirmation</h2>
        <p className="mb-4">{message}</p>
        <div className="flex justify-end">
          <button 
            onClick={onConfirm} 
            className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded mr-2">
            Yes
          </button>
          <button 
            onClick={onClose} 
            className="bg-gray-300 hover:bg-gray-400 text-black px-4 py-2 rounded">
            No
          </button>
        </div>
      </div>
    </div>
  );
};

export default ConfirmBox;
