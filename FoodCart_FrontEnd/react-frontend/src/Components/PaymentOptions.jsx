import React, { useState } from 'react';

const PaymentOptions = () => {
  const [selectedPaymentMethod, setSelectedPaymentMethod] = useState('');

  // Handle payment method selection
  const handlePaymentMethodChange = (event) => {
    setSelectedPaymentMethod(event.target.value);
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-4">
      <div className="bg-white shadow-lg rounded-lg p-6 w-full max-w-lg">
        <h2 className="text-2xl font-bold mb-4 text-center">Choose Payment Method</h2>

        {/* Payment Options List */}
        <div className="flex flex-col space-y-4">
          {/* Credit/Debit Card */}
          <div className="flex items-center justify-between bg-gray-50 border border-gray-300 rounded-lg p-4 cursor-pointer hover:bg-gray-200 transition duration-200">
            <div className="flex items-center">
              <img src="/creditcard.png" alt="Card Icon" className="w-8 h-8 mr-2" />
              <span className="text-lg">Credit/Debit Card</span>
            </div>
            <input
              type="radio"
              name="payment"
              value="card"
              onChange={handlePaymentMethodChange}
              className="form-radio h-5 w-5 text-blue-600"
            />
          </div>

          {/* Cash on Delivery */}
          <div className="flex items-center justify-between bg-gray-50 border border-gray-300 rounded-lg p-4 cursor-pointer hover:bg-gray-200 transition duration-200">
            <div className="flex items-center">
              <img src="/cod.png" alt="Cash Icon" className="w-8 h-8 mr-2" />
              <span className="text-lg">Cash on Delivery</span>
            </div>
            <input
              type="radio"
              name="payment"
              value="cod"
              onChange={handlePaymentMethodChange}
              className="form-radio h-5 w-5 text-blue-600"
            />
          </div>
        </div>

        {/* Proceed Button */}
        <button className="mt-6 w-full bg-blue-600 text-white font-bold py-2 rounded-lg hover:bg-blue-700 transition duration-200">
          Proceed to Pay
        </button>
      </div>
    </div>
  );
};

export default PaymentOptions;
