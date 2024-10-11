import { createContext, useState } from 'react';

// Create Auth Context
export const AuthContext = createContext();

// Create AuthProvider component to wrap around the entire app
export const AuthProvider = ({ children }) => {
  const [authData, setAuthData] = useState(null);

  return (
    <AuthContext.Provider value={{ authData, setAuthData }}>
      {children}
    </AuthContext.Provider>
  );
};
