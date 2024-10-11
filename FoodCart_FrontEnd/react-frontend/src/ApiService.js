    import axios from 'axios'; // To perform HTTP methods efficiently

    const url = 'https://localhost:7263/api/Auth';  //My base URL for authentication

    //Register API call

    export const regUser = async (registerUser) => {
        try {
          const response = await axios.post(`${url}/register`, registerUser);
          return response.data; // return the response data on success
        } catch (error) {
          // Throw an error with the response data if available
          if (error.response) {
            throw error.response; // This will capture the response data
          } else {
            throw new Error('Network error or server not responding');
          }
        }
      };

    //Login API Call

    export const loginUser = async(loginData) =>{
        try{
            const response  = await axios.post(`${url}/login`, loginData) // fetch base url/login
            return response.data
        }catch(error){
            throw error.response ? error.response.data : error;
        }
    } 
    // ApiService.js
export const requestPasswordReset = async ({ email }) => {
  const response = await fetch('https://localhost:7263/api/ForgotPassword/request-reset', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email }),
  });

  if (!response.ok) {
    throw new Error('Failed to send reset link');
  }

  return response.json(); // Adjust as necessary based on your API response structure
};

    
export const resetPassword = async (payload) => {
  try {
      const response = await axios.post('https://localhost:7263/api/ForgotPassword/reset-password', payload);
      return response.data; // Assuming response.data contains the message
  } catch (error) {
      // Check if error.response and error.response.data exist
      if (error.response && error.response.data) {
          throw error.response.data; // Throw the full error response object to handle in catch block
      } else {
          throw new Error('Network error or invalid response'); // Generic error for network issues
      }
  }
};



