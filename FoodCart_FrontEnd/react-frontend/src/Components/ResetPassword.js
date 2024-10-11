import React, { useState } from 'react';
import { useLocation , useNavigate } from 'react-router-dom';
import { resetPassword } from '../ApiService'; 
import Notification from '../Notification';
import './ResetPassword.css'; 


const ResetPassword = () => {
    const { search } = useLocation();
    const query = new URLSearchParams(search);
    const token = query.get('token');
    const navigate = useNavigate();

   
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    // Handle the form submission
    const handleSubmit = async (e) => {
        e.preventDefault();
    
        if (newPassword !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }
    
        try {
           
            const response = await resetPassword({ token, newPassword, confirmPassword });
            
            
            if (response && response.message) {
                setMessage(response.message); 
                setError(''); 

                setTimeout(() => {
                    navigate('/login');
                }, 2000);
            }
        } catch (err) {
            
            if (err && err.message) {
                setError(err.message); 
            } else {
                setError('An unexpected error occurred.'); 
            }
            setMessage('');
        }
    };
    
    

    return (
        <div className="reset-password-container">
            <h2 className='font-sans font-bold'>Reset Password</h2>
            {message && <Notification message={message} type="success" />}
            {error && <Notification message={error} type="error" />}
            <form onSubmit={handleSubmit}>
                <input
                    type="password"
                    placeholder="New Password"
                    className='font-sans mt-6 rounded-md p-2'
                    value={newPassword}
                    onChange={(e) => setNewPassword(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Confirm Password"
                     className='font-sans mt-6 rounded-md p-2'
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                />
                <button type="submit" className='font-sans bg-green-600 mt-4 p-2 text-white'>Reset Password</button>
            </form>
        </div>
    );
};

export default ResetPassword;
