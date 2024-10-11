import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom'; 
import { regUser } from './ApiService';
import { useUser } from './UserContext'; 
import './Reg.css'; 
import Notification from './Notification'; 

export default function Register() {
  const { setUser } = useUser(); 
  const [user, setUserState] = useState({
    userName: '',
    email: '',
    password: '',
    phoneNumber: '',
    role: '',
    restaurantID: ''
  });

  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [isSuccess, setIsSuccess] = useState(false); 
  const navigate = useNavigate(); 

  const handleChange = (e) => {
    setUserState({ ...user, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const registerUser = {
      userName: user.userName,
      email: user.email,
      password: user.password,
      phoneNumber: user.phoneNumber,
      role: user.role,
      restaurantID: (user.role !== 'Customer' && user.role !== 'Admin') ? user.restaurantID : null
    };

    try {
      const response = await regUser(registerUser);
      console.log("Registration successful:", response);
      setMessage("Registration successful!");
      setError('');
      setIsSuccess(true); 

      
      setUser({ userName: user.userName }); 

      navigate('/login'); 
      
      setTimeout(() => {
        setUserState({
          userName: '',
          email: '',
          password: '',
          phoneNumber: '',
          role: '',
          restaurantID: ''
        });
        setIsSuccess(false); 
      }, 2000); 
    } catch (error) {
      console.error("Registration failed:", error);
      const errorResponse = error.data || "Registration failed. Please try again.";
      setError(errorResponse);
      setMessage(''); 
    }
  };

  return (
    <div className={`register-container ${isSuccess ? 'fade-out' : ''}`}>
      <h2>Register</h2>
      {message && <Notification message={message} type="success" />}
      {error && <Notification message={error} type="error" />}
      {!isSuccess && (
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="userName"
            placeholder="Username"
            value={user.userName}
            onChange={handleChange}
            required
          />
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={user.email}
            onChange={handleChange}
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Password (8 characters long)"
            value={user.password}
            onChange={handleChange}
            required
          />
          <input
            type="text"
            name="phoneNumber"
            placeholder="Phone Number"
            value={user.phoneNumber}
            onChange={handleChange}
            required
          />
          <select name="role" value={user.role} onChange={handleChange} required>
            <option value="">Select Role</option>
            <option value="Customer">Customer</option>
            <option value="Admin">Admin</option>
            <option value="Hotel Owner">Hotel Owner</option>
          </select>
          {(user.role !== 'Customer' && user.role !== 'Admin') && (
            <input
              type="text"
              name="restaurantID"
              placeholder="Restaurant ID"
              value={user.restaurantID}
              onChange={handleChange}
            />
          )}
          <button type="submit" className="register-button">Register</button>
        </form>
      )}
      <div className='font-sans text-xs mt-3 hover:text-green-400'>
      <p>
        Already have an account? <Link to="/login"> Login</Link>
      </p>
      </div>
    </div>
  );
}
