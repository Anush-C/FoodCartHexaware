import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { AuthProvider } from './AuthContext';
import { UserProvider } from './UserContext';

import { Elements } from '@stripe/react-stripe-js';
import { loadStripe } from '@stripe/stripe-js';

// Load your Stripe public key
const stripePromise = loadStripe('pk_test_51Q4b8SGHqUhGupcxEqoHXbRmqh55yusheftY3dHB1ANN1SEbJzi2jEGKhJdWC7V07XHIUoOOXwRqHlP8x7vqdzqh00qJUmOrG0');

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    
      <AuthProvider>
        <UserProvider>
          <Elements stripe={stripePromise}> 
            <App />
          </Elements>
        </UserProvider>
      </AuthProvider>
  </React.StrictMode>
);

reportWebVitals();
