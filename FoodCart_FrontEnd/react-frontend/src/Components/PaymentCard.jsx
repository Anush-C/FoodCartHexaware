import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ConfirmDialog from './ConfirmDialog2'; 
import DeliveryAddressCart from './DeliveryAddressCart';// Import the custom confirmation dialog component

const PaymentCard = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [selectedAddress, setSelectedAddress] = useState('');
  const navigate = useNavigate();

  // Check if the user is logged in by checking for a token or user state
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      setIsLoggedIn(true);
    }
  }, []);

  // Function to handle opening the confirmation dialog
  const handleCheckoutClick = () => {
    if (isLoggedIn) {
      setIsDialogOpen(true); // Show the confirmation dialog
    } else {
      alert('You must be logged in to proceed to checkout.');
    }
  };

  // Function to handle checkout confirmation
  const confirmCheckout = () => {
    setIsDialogOpen(false); // Close the dialog
    navigate('/checkout'); // Navigate to checkout page
  };

  // Function to cancel the checkout
  const cancelCheckout = () => {
    setIsDialogOpen(false); // Close the dialog without proceeding
  };

  return (
    <div className="flex flex-col justify-center items-center bg-white gap-4 px-8 py-4 mt-5 mr-2">
      <button
        onClick={handleCheckoutClick} // Trigger custom confirmation dialog
        className={`bg-gray-400 text-white px-2 py-2  font-semibold hover:bg-lime-600 cursor-pointer text-center  ${!isLoggedIn ? 'opacity-50 cursor-not-allowed' : ''}`}
        disabled={!isLoggedIn} // Disable button if not logged in
      >
        PROCEED TO CHECKOUT
      </button>

      {/* Render the custom confirmation dialog when isDialogOpen is true */}
      {isDialogOpen && (
        <ConfirmDialog
          message="Are you sure you want to proceed to checkout?"
          onConfirm={confirmCheckout}
          onCancel={cancelCheckout}
        />
      )}
    </div>
  );
};

export default PaymentCard;
