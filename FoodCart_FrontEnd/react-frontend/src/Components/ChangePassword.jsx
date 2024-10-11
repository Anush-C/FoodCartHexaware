import React, { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import { FaLock } from 'react-icons/fa'; // Icon for password

const ChangePassword = () => {
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState(false); // State for input validation

  const handleChangePassword = () => {
    if (!currentPassword || !newPassword) {
      setError(true);
      setMessage('Both fields are required.');
      return;
    }

    const jwtToken = localStorage.getItem('token');
    const userId = jwtDecode(jwtToken).UserId;

    fetch(`https://localhost:7263/api/UserProfile/ChangePassword/${userId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${jwtToken}`,
      },
      body: JSON.stringify({ CurrentPassword: currentPassword, NewPassword: newPassword }),
    })
      .then((response) => response.json())
      .then((data) => setMessage(data.message))
      .catch(() => setMessage('Error changing password'));
  };

  return (
    <div className="min-h-screenbg-gradient-to-br from-white to-black flex justify-center items-center p-8 font-sans">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-3xl font-bold text-blue-700 mb-6 text-center">Change Password</h2>

        {/* Current Password Input */}
        <div className="mb-6">
          <label htmlFor="current-password" className="block text-sm font-semibold text-gray-600 mb-1">Current Password</label>
          <div className="relative">
            <input
              id="current-password"
              type="password"
              value={currentPassword}
              onChange={(e) => setCurrentPassword(e.target.value)}
              placeholder="Current password"
              className={`w-full px-10 py-2 border ${error && !currentPassword ? 'border-red-500' : 'border-gray-300'} rounded-lg focus:ring-2 focus:ring-blue-400 focus:outline-none transition duration-300 ease-in-out`}
            />
            <FaLock className="absolute top-1/2 transform -translate-y-1/2 left-3 text-gray-400" size={18} />
          </div>
        </div>

        {/* New Password Input */}
        <div className="mb-6">
          <label htmlFor="new-password" className="block text-sm font-semibold text-gray-600 mb-1">New Password</label>
          <div className="relative">
            <input
              id="new-password"
              type="password"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              placeholder="New password"
              className={`w-full px-10 py-2 border ${error && !newPassword ? 'border-red-500' : 'border-gray-300'} rounded-lg focus:ring-2 focus:ring-blue-400 focus:outline-none transition duration-300 ease-in-out`}
            />
            <FaLock className="absolute top-1/2 transform -translate-y-1/2 left-3 text-gray-400" size={18} />
          </div>
        </div>

        {/* Submit Button */}
        <button
          onClick={handleChangePassword}
          className="w-full py-2 bg-gradient-to-r from-blue-500 to-green-500 text-white rounded-lg font-semibold hover:bg-blue-700 transition duration-300 ease-in-out focus:ring-4 focus:ring-green-300 focus:outline-none"
        >
          Change Password
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

export default ChangePassword;
