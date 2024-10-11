import React, { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import { FaPhoneAlt } from 'react-icons/fa'; // Icon for phone number

const AlternativePhoneNumber = () => {
  const [newPhoneNumber, setNewPhoneNumber] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState(false); // State for input validation

  const handlePhoneChange = (e) => {
    setNewPhoneNumber(e.target.value);
    setError(false); // Reset error when input changes
  };

  const handleSubmit = () => {
    // Simple phone validation (can be enhanced)
    if (!newPhoneNumber || !/^\d{10}$/.test(newPhoneNumber)) {
      setError(true);
      setMessage('Please enter a valid 10-digit phone number.');
      return;
    }

    const jwtToken = localStorage.getItem('token');
    const userId = jwtDecode(jwtToken).UserId;

    fetch(`https://localhost:7263/api/UserProfile/UpdateAlternativePhone/${userId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${jwtToken}`,
      },
      body: JSON.stringify(newPhoneNumber),
    })
      .then((response) => response.json())
      .then((data) => setMessage(data.message))
      .catch((error) => setMessage('Error updating phone number'));
  };

  return (
    <div className="min-h-screenbg-gradient-to-br from-white to-black flex justify-center items-center p-8 font-sans">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-3xl font-bold text-blue-700 mb-6 text-center">Update Phone Number</h2>

        {/* Phone Number Input */}
        <div className="mb-6">
          <label htmlFor="phone" className="block text-sm font-semibold text-gray-600 mb-1">New Phone Number</label>
          <div className="relative">
            <input
              id="phone"
              type="text"
              value={newPhoneNumber}
              onChange={handlePhoneChange}
              placeholder="Enter your new phone number"
              className={`w-full px-10 py-2 border ${error ? 'border-red-500' : 'border-gray-300'} rounded-lg focus:ring-2 focus:ring-blue-400 focus:outline-none transition duration-300 ease-in-out`}
            />
            <FaPhoneAlt className="absolute top-1/2 transform -translate-y-1/2 left-3 text-gray-400" size={18} />
          </div>
          {error && <p className="text-red-500 mt-2 text-sm">Invalid phone number format.</p>}
        </div>

        {/* Submit Button */}
        <button
          onClick={handleSubmit}
          className="w-full py-2 bg-gradient-to-r from-blue-500 to-green-500 text-white rounded-lg font-semibold hover:bg-blue-700 transition duration-300 ease-in-out focus:ring-4 focus:ring-green-300 focus:outline-none"
        >
          Update Phone Number
        </button>

        {/* Message Display */}
        {message && (
          <p className={`mt-4 text-center ${message.includes('Error') ? 'text-red-500' : 'text-green-500'} font-medium`}>
            {message}
          </p>
        )}
      </div>
    </div>
  );
};

export default AlternativePhoneNumber;
