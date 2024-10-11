import React from 'react';
import './Footer.css'; // Add your footer styles here
import { FaFacebookF, FaInstagram, FaTwitter } from 'react-icons/fa'; // Social media icons
import { useNavigate } from 'react-router-dom';

const Footer = () => {
  const navigate = useNavigate();

  return (
    <footer className="footer">
      {/* Quick Links Section */}
      <div className="footer-section">
        <div className="footer-column">
          <h4>Quick Links</h4>
          <ul>
            <li><a href="#" onClick={() => navigate('/')}>Home</a></li>
            <li><a href="#">Offers</a></li>
            <li><a href="#">Restaurants</a></li>
            <li><a href="#">Help</a></li>
            <li><a href="#" onClick={() => navigate('/contact')}>Contact Us</a></li> {/* Contact Us link */}
          </ul>
        </div>

        {/* Company Info Section */}
        <div className="footer-column">
          <h4>Company</h4>
          <ul>
            <li><a href="#">About Us</a></li>
            <li><a href="#">Careers</a></li>
            <li><a href="#" onClick={() => navigate('/contact')}>Contact Us</a></li> {/* Contact Us link */}
            <li><a href="#">Blog</a></li>
          </ul>
        </div>

        {/* Legal Section */}
        <div className="footer-column">
          <h4>Legal</h4>
          <ul>
            <li><a href="#">Terms of Service</a></li>
            <li><a href="#">Privacy Policy</a></li>
            <li><a href="#">Cookie Policy</a></li>
          </ul>
        </div>

        {/* App Download Section */}
        <div className="footer-column">
          <h4>Get the App</h4>
          <a href="#">
            <img src="/play_store.avif" alt="Google Play" className="app-badge" />
          </a>
          <a href="#">
            <img src="/app_store.avif" alt="App Store" className="app-badge" />
          </a>
        </div>
      </div>

      

      {/* Copyright Section */}
      <p className="footer-copyright">
        &copy; 2024 FoodCart. All rights reserved.
      </p>
    </footer>
  );
};

export default Footer;
