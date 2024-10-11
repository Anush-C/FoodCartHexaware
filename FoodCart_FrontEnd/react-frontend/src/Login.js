import { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { loginUser } from './ApiService';
import { AuthContext } from './AuthContext';
import { useUser } from './UserContext'; 
import Notification from './Notification';
import './Login.css'; 

const Login = () => {
  const { setAuthData } = useContext(AuthContext);
  const { setUser } = useUser(); 
  const [loginData, setLoginData] = useState({ email: '', password: '' });
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [isSuccess, setIsSuccess] = useState(false); 
  const navigate = useNavigate(); 

  const handleChange = (e) => {
    setLoginData({ ...loginData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await loginUser(loginData); 
      
      if (response.token) { 
        // Set token in localStorage
        localStorage.setItem('token', response.token);

        // Set the user context
        setUser({ userName: response.userName }); 
        
        // Set auth data, including restaurantId for Hotel Owners
        setAuthData({
          token: response.token,
          role: response.role, 
          restaurantId: response.restaurantId // Save the restaurantId
        });

        // Show success message
        setMessage('Login successful!');
        setError('');
        setIsSuccess(true);

        // Redirect based on user role
        if (response.role === 'Admin') {
          navigate('/admin'); // Redirect to Admin Dashboard
        } 
        else if (response.role === 'Hotel Owner') {
          // Navigate to Hotel Owner dashboard, passing the restaurantId if needed
          navigate(`/hotelowner?restaurantId=${response.restaurantId}`);
        } else {
          navigate('/welcome'); // Redirect to User Dashboard
        }
      } else {
        setError('Login failed: Missing data in response');
      }
    } catch (error) {
      console.error("Login failed: Incorrect email or password");
      setMessage('');
      setError(error.response?.data?.message || 'Incorrect email or password'); 
    }
  };

  return (
    <div className={`login-container ${isSuccess ? 'fade-out' : ''}`}>
      <h2 className='font-sans font-bold mb-2'>Login</h2>
      {message && <Notification message={message} type="success" />}
      {error && <Notification message={error} type="error" />}
      {!isSuccess && (
        <form onSubmit={handleSubmit}>
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={loginData.email}
            onChange={handleChange}
            className='font-sans'
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            className="font-sans"
            value={loginData.password}
            onChange={handleChange}
            required
          />
          <button type="submit" className="login-button font-sans">Login</button>
        </form>
      )}
      <div className='font-sans text-left -mt-3 text-blue-400 hover:text-blue-600 text-xs font-medium'>
        <p>
          <Link to="/forgot-password">Forgot Password?</Link>
        </p>
      </div>
      <div className='font-sans text-xs mt-5 font-medium hover:text-red-600'>
        <p>
          Don't have an account? <Link to="/register">Register</Link>
        </p>
      </div>
    </div>
  );
};

export default Login;
