import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode'; // Ensure you have jwt-decode installed
import { FaUserCog, FaEnvelope, FaPhone, FaLock, FaClipboardList } from 'react-icons/fa'; // Ensure you have react-icons installed

const ProfilePage = () => {
  const [profileData, setProfileData] = useState(null);
  const [error, setError] = useState(null);

  useEffect(() => {
    const jwtToken = localStorage.getItem('token');
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const userId = decodedToken.UserId;

        // Fetch user profile data
        fetch(`https://localhost:7263/api/UserProfile/${userId}`, {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${jwtToken}`,
            'Accept': 'application/json',
          },
        })
          .then((response) => {
            if (!response.ok) {
              throw new Error('Failed to fetch user profile');
            }
            return response.json();
          })
          .then((data) => {
            setProfileData(data);
          })
          .catch((error) => {
            console.error('Error fetching profile:', error);
            setError('Failed to load profile data');
          });
      } catch (error) {
        console.error('Error decoding JWT:', error);
        setError('Invalid token');
      }
    }
  }, []);

  return (
    <div className="flex flex-col mx-auto my-4 max-w-lg p-6 bg-white text-black rounded-lg shadow-lg border border-gray-200 font-sans">
      <h2 className="text-3xl font-bold text-center mb-4">Profile Options</h2>

      {error ? (
        <div className="text-red-200 text-center">{error}</div>
      ) : profileData ? (
        <>
          <div className="font-semibold text-lg text-center mb-2">Welcome, {profileData.email}!</div>
          <ul className="flex flex-col gap-3">
            <li>
              <Link to="/profile/accounts" className="flex items-center p-3 rounded-lg bg-white text-indigo-600 text-center hover:bg-gray-200 transition duration-200 shadow-md">
                <FaUserCog className="mr-2" /> Accounts
              </Link>
            </li>
            <li>
              <Link to="/profile/update-email" className="flex items-center p-3 rounded-lg bg-white text-indigo-600 text-center hover:bg-gray-200 transition duration-200 shadow-md">
                <FaEnvelope className="mr-2" /> Update Email
              </Link>
            </li>
            <li>
              <Link to="/profile/alternative-phone" className="flex items-center p-3 rounded-lg bg-white text-indigo-600 text-center hover:bg-gray-200 transition duration-200 shadow-md">
                <FaPhone className="mr-2" /> Alternative Phone Number
              </Link>
            </li>
            <li>
              <Link to="/profile/change-password" className="flex items-center p-3 rounded-lg bg-white text-indigo-600 text-center hover:bg-gray-200 transition duration-200 shadow-md">
                <FaLock className="mr-2" /> Change Password
              </Link>
            </li>
            <li>
              <Link to="/profile/orders" className="flex items-center p-3 rounded-lg bg-white text-indigo-600 text-center hover:bg-gray-200 transition duration-200 shadow-md">
                <FaClipboardList className="mr-2" /> View All Orders
              </Link>
            </li>
          </ul>
        </>
      ) : (
        <div className="text-center">Loading...</div>
      )}
    </div>
  );
};

export default ProfilePage;
