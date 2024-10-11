import React from 'react';
import './ConfirmDialog.css'; // Import your CSS for styling

const ConfirmDialog = ({ isOpen, onClose, onConfirm }) => {
  if (!isOpen) return null;

  return (
    <div className="confirm-dialog-overlay">
      <div className="confirm-dialog">
        <h2 className='font-sans  font-semibold'>Confirm Logout</h2>
        <p className='mt-2 font-sans'>Are you sure you want to log out?</p>
        <div className="confirm-dialog-buttons">
          <button className='font-sans bg-red-500 p-2' onClick={onClose}>Cancel</button>
          <button className='font-sans bg-green-500 p-2' onClick={onConfirm}>Logout</button>
        </div>
      </div>
    </div>
  );
};

export default ConfirmDialog;
