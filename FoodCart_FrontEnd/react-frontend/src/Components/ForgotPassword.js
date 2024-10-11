import React, { useState } from 'react';
import { requestPasswordReset } from '../ApiService'; 
import Notification from '../Notification';
import './ForgotPassword.css'; 

const ForgotPassword = () => {
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      
      const response = await requestPasswordReset({ email });
      setMessage(response.message); 
      setError('');
    } catch (error) {
     
      setError(error.data); 
      console.log(error)
      setMessage('');
    }
  };

  return (
    <div className="forgot-password-container">
      <h2 className='font-sans font-medium'>Forgot Password</h2>
      {message && <Notification message={message} type="success" />}
      {error && <Notification message={error} type="error" />}
      <form  onSubmit={handleSubmit}>
        <input
          type="email"
          placeholder="Enter your email"
          className='font-sans mt-4 rounded-sm p-2'
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <button type="submit" className='font-sans bg-green-600 mt-4 rounded-md p-1'>Send Reset Link</button>
      </form>
    </div>
  );
};

export default ForgotPassword;
