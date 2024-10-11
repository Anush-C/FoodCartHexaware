import React, { useState, useEffect } from 'react';
import { CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; // Ensure this is imported correctly
import { toast, ToastContainer } from 'react-toastify'; // Import Toastify
import 'react-toastify/dist/ReactToastify.css'; // Import the CSS for Toastify
import 'tailwindcss/tailwind.css';
import { useNavigate } from 'react-router-dom'; // Import useNavigate for navigation

const Checkout = () => {
  const stripe = useStripe();
  const elements = useElements();
  const navigate = useNavigate(); // Hook to programmatically navigate

  const [userId, setUserId] = useState('');
  const [restaurantId, setRestaurantId] = useState('');
  const [selectedAddress, setSelectedAddress] = useState('');
  const [paymentMethod, setPaymentMethod] = useState('card'); // Default to card payment
  const [stripePaymentMethodId, setStripePaymentMethodId] = useState('');
  const [jwtToken] = useState(localStorage.getItem('token'));
  const [userID, setUserID] = useState(null);

  useEffect(() => {
    const storedSelectedAddress = localStorage.getItem('selectedAddress');
    if (storedSelectedAddress) {
      setSelectedAddress(storedSelectedAddress);
    } else {
      console.error('No selected address found in localStorage');
    }
  }, []);

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedUserID = decodedToken.UserId;
        console.log('Decoded User ID:', decodedUserID);
        setUserID(decodedUserID);
      } catch (error) {
        console.error('Error decoding JWT:', error);
      }
    }
  }, [jwtToken]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    let paymentMethodId = '';

    if (paymentMethod === 'card') {
      const cardElement = elements.getElement(CardElement);
      const { error, paymentMethod } = await stripe.createPaymentMethod({
        type: 'card',
        card: cardElement,
      });

      if (error) {
        console.error(error);
        toast.error(error.message); // Use Toast for error
        return;
      }

      paymentMethodId = paymentMethod.id; // Store the payment method ID
    }

    try {
      const response = await axios.post(
        'https://localhost:7263/api/Checkout/checkout',
        {
          userId: userID,
          restaurantId,
          shippingAddress: selectedAddress,
          paymentMethod,
          stripePaymentMethodId: paymentMethodId, // Use the stored payment method ID
        },
        {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );

      // Show success message
      toast.success(response.data.message);

      // Navigate to Order Confirmation page
      navigate(`/order-confirmation/${response.data.orderId}`); // Pass the order ID if available

    } catch (err) {
      console.error(err);
      toast.error('Payment failed: ' + (err.response?.data || err.message)); // Show error message
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-white">
      <ToastContainer /> {/* Add ToastContainer for notifications */}
      <form
        onSubmit={handleSubmit}
        className="bg-white shadow-xl rounded-lg p-8 max-w-lg w-full transition-transform transform hover:scale-105 hover:shadow-xl duration-300"
      >
        {/* Heading with logo */}
        <div className="flex items-center justify-center mb-6">
          <img src='/logofc.png' alt="Logo" className="w-10 h-10 mr-3" />
          <h2 className="text-2xl font-bold text-orange-600 font-sans mt-6 mb-4">Secure Checkout</h2>
        </div>

        <input
          type="text"
          placeholder="User ID (auto-filled)"
          value={userID || userId}
          onChange={(e) => setUserId(e.target.value)}
          required
          className="w-full p-3 mb-4 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 transition duration-300 font-sans"
        />

        <input
          type="text"
          placeholder="Restaurant ID"
          value={restaurantId}
          onChange={(e) => setRestaurantId(e.target.value)}
          required
          className="w-full p-3 mb-4 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 transition duration-300 font-sans"
        />

        <input
          type="text"
          placeholder="Shipping Address"
          value={selectedAddress}
          readOnly
          className="w-full p-3 mb-4 font-sans bg-gray-200 border border-gray-300 rounded-lg focus:outline-none"
        />

        <div className="mb-1">
          <h2 className='font-bold font-sans mt-2'>Choose Payment Method</h2>
        </div>
        <div>
          <label className="flex items-center mb-2 mt-2 font-sans">
            <input
              type="radio"
              value="card"
              checked={paymentMethod === 'card'}
              onChange={() => setPaymentMethod('card')}
              className="mr-2"
            />
            Card Payment (Stripe)
          </label>
          <label className="flex items-center font-sans">
            <input
              type="radio"
              value="cod"
              checked={paymentMethod === 'cod'}
              onChange={() => setPaymentMethod('cod')}
              className="mr-2"
            />
            Cash on Delivery 
          </label>
        </div>

        {paymentMethod === 'card' && (
          <div className="mb-6 mt-4">
            <CardElement className="p-4 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 transition duration-300" />
          </div>
        )}

        <button
          type="submit"
          disabled={!stripe}
          className="w-full p-3 bg-green-600 font-sans text-white font-bold rounded-lg hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 transition-colors duration-300 mt-4"
        >
          Place Order
        </button>
      </form>
    </div>
  );
};

export default Checkout;
