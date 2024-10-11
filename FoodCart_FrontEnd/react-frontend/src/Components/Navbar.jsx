import React, { useState, useContext, useEffect } from 'react';
import './Navbar.css';
import flogo from '../Components/Design 1.png';
import { Link, useNavigate } from 'react-router-dom';
import { useUser } from '../UserContext'; 
import { AuthContext } from '../AuthContext'; 
import ConfirmDialog from './ConfirmDialog'; 
import { jwtDecode } from 'jwt-decode'; // Use jwt-decode
import { FaUserCircle } from 'react-icons/fa'; // Avatar icon

const Navbar = ({ setIsOtherOpen, setIsProfileOpen }) => {
  const [isDialogOpen, setIsDialogOpen] = useState(false); 
  const navigate = useNavigate(); 
  const { user, setUser } = useUser(); 
  const { setAuthData } = useContext(AuthContext); 
  const jwtToken = localStorage.getItem('token'); // Get JWT from localStorage
  const [userName, setUserName] = useState(''); // State for username

  useEffect(() => {
    // Decode the JWT token to get the username
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        setUserName(decodedToken.userName || ''); 
      } catch (error) {
        console.error('Error decoding JWT:', error);
      }
    }
  }, [jwtToken]);

  const handleLogoutClick = () => {
    setIsDialogOpen(true); // Open confirmation dialog
  };

  const handleConfirmLogout = () => {
    setUser(null); 
    setAuthData({}); 
    localStorage.removeItem('token'); // Remove JWT token
    navigate('/login'); // Redirect to login
    setIsDialogOpen(false); 
  };

  const handleCloseDialog = () => {
    setIsDialogOpen(false); // Close confirmation dialog
  };

  return (
    <>
      {isDialogOpen && (
        <ConfirmDialog 
          isOpen={isDialogOpen}
          onClose={handleCloseDialog}
          onConfirm={handleConfirmLogout}
          message="Are you sure you want to log out?" // Optional message for the dialog
        />
      )}
      <div className='shadow-xl shadow-gray-200 p-3'>
        <nav className="z-10">
          <div className="navbar-container">
            {/* Logo Section */}
            <div className="navbar-logo">
              <Link to="/">
                <img src={flogo} alt="FoodCart Logo" className="logo-image" />
              </Link>
              <div className='font-sans'>
                <button onClick={() => setIsOtherOpen(true)} className="others-button">
                  Other
                </button>
              </div>
            </div>

            {/* Links Section */}
            <ul className="navbar-links">
              <div className='font-sans'>
                <li>
                  <Link to="/search">Search</Link>
                </li>
              </div>
              <div className='font-sans'>
                <li>
                  <Link to="/aboutus">About Us</Link>
                </li>
              </div>
              {jwtToken ? ( // Check if JWT exists
                <>
                  <li>
                    <span className="cursor-pointer" onClick={() => setIsProfileOpen(true)}>
                      {userName || <FaUserCircle size={23} />} 
                    </span>
                  </li>
                  <li>
                    <button onClick={handleLogoutClick} className="logout-button font-sans font-medium bg-orange-500 p-2 text-white">
                      Logout
                    </button>
                  </li>
                </>
              ) : (
                <div className='font-sans'>
                  <li>
                    <Link to="/login">
                      Login
                    </Link>
                  </li>
                </div>
              )}
              <div className='font-sans'>
                <li>
                  <Link to="cart">Cart</Link>
                </li>
              </div>
            </ul>
          </div>
        </nav>
      </div>
    </>
  );
};

export default Navbar;
