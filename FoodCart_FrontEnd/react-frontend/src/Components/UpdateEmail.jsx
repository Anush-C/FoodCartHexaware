import React, { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import { FaEnvelope } from 'react-icons/fa'; // Importing an email icon

const UpdateEmail = () => {
  const [newEmail, setNewEmail] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState(false); // Error state for validation

  const handleEmailChange = (e) => {
    setNewEmail(e.target.value);
    setError(false); // Reset error when input changes
  };

  const handleSubmit = () => {
    if (!newEmail || !/\S+@\S+\.\S+/.test(newEmail)) {
      setError(true);
      setMessage('Please enter a valid email.');
      return;
    }

    const jwtToken = localStorage.getItem('token');
    const userId = jwtDecode(jwtToken).UserId;

    fetch(`https://localhost:7263/api/UserProfile/ChangeEmail/${userId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${jwtToken}`,
      },
      body: JSON.stringify(newEmail),
    })
      .then((response) => response.json())
      .then((data) => setMessage(data.message))
      .catch((error) => setMessage('Error updating email'));
  };

  return (
    <div className="min-h-screenbg-gradient-to-br from-white to-black flex justify-center items-center p-8 font-sans">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-3xl font-bold text-indigo-700 mb-6 text-center">Update Email</h2>
        
        {/* Email Field */}
        <div className="mb-6 relative">
          <label htmlFor="email" className="block text-sm font-semibold text-gray-600 mb-1">New Email</label>
          <div className="relative">
            <input
              id="email"
              type="email"
              value={newEmail}
              onChange={handleEmailChange}
              placeholder="Enter your new email"
              className={`w-full px-10 py-2 border ${error ? 'border-red-500' : 'border-gray-300'} rounded-lg focus:ring-2 focus:ring-indigo-400 focus:outline-none transition duration-300 ease-in-out`}
            />
            <FaEnvelope className="absolute top-1/2 transform -translate-y-1/2 left-3 text-gray-400" size={18} />
          </div>
          {error && <p className="text-red-500 mt-2 text-sm">Invalid email format.</p>}
        </div>

        {/* Submit Button */}
        <button
          onClick={handleSubmit}
          className="w-full py-2 bg-gradient-to-r from-indigo-500 to-purple-500 text-white rounded-lg font-semibold hover:bg-indigo-700 transition duration-300 ease-in-out focus:ring-4 focus:ring-purple-300 focus:outline-none"
        >
          Update Email
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

export default UpdateEmail;
