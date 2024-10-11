import React, { useState, useEffect } from 'react';
import { IoClose } from 'react-icons/io5';
import { Link } from 'react-router-dom'; // Ensure you have react-router-dom installed for navigation
import { jwtDecode } from 'jwt-decode'; // Ensure you import jwtDecode

const UserProfile = ({ setIsProfileOpen }) => {
  const [profileData, setProfileData] = useState({ phoneNumber: '', email: '' });
  const [error, setError] = useState(null);

  const handleClose = () => {
    setIsProfileOpen(false);
  };

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
            setProfileData({
              phoneNumber: data.phoneNumber,
              email: data.email,
            });
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
    <div className='flex flex-col mx-8 my-4 gap-4 font-sans'>
      <div className='flex flex-row gap-4 items-center'>
        <div onClick={handleClose} className='cursor-pointer'>
          <IoClose size={23} />
        </div>
        <div className='text-xl font-semibold'>User Profile</div>
      </div>

      {error ? (
        <div className='text-red-500'>{error}</div>
      ) : (
        <>
          <div className='flex flex-col gap-2 mt-6'>
            <div className='font-semibold text-lg text-gray-700'>Phone Number</div>
            <div>{profileData.phoneNumber || 'Loading...'}</div>
          </div>

          <hr className='border-[0.5px solid] border-gray-300' />

          <div className='flex flex-col gap-2'>
            <div className='font-semibold text-lg text-gray-700'>Email ID</div>
            <div>{profileData.email || 'Loading...'}</div>
          </div>

          <div className='mt-4'>
            <Link to="/profile" className='text-blue-500 underline'>
              Go to Profile Page
            </Link>
          </div>
        </>
      )}
    </div>
  );
};

export default UserProfile;
