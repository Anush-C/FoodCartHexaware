import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './WelcomePage.css'; // Import the updated CSS

const WelcomePage = () => {
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setTimeout(() => {
      setLoading(false);
      navigate('/'); // Navigate to the home page after the loading effect
    }, 7000); // 3 seconds delay

    return () => clearTimeout(timer); // Cleanup the timer on component unmount
  }, [navigate]);

  return (
    <div className="welcome-container font-sans">
      {loading ? (
        <div className="loading-content">
          {/* <img src="/logofc.png" alt="Food Cart Logo" className="logo" /> Optional Logo */}
          <h2 className="welcome-text font-sans">Welcome to Food Cart!</h2>
          <p className="subtext">Searching for great deals near you...</p>
          <div className="spinner"></div>
        </div>
      ) : null}
    </div>
  );
};

export default WelcomePage;
