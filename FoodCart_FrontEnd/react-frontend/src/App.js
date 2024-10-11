import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import Register from './Register';
import Login from './Login';
import Home from './Pages/Home';
import AboutUs from './Components/AboutUs';
import Navbar from './Components/Navbar';
import Search from './Components/Search';
import ItemDetail from './Components/RestaurantDetail';
import ForgotPassword from './Components/ForgotPassword';
import ResetPassword from './Components/ResetPassword';
import Others from './Components/Others';
import UserProfile from './Components/UserProfile';
import Cart from './Components/Cart';
import Menu from './Components/Menu';
import MenuDetails from './Components/MenuDetails';
import RestaurantItems from './Components/RestaurantItems';
import AdminDashboard from './Components/AdminDashboard';
import OrderConfirmation from './Components/OrderConfirmation';
import HotelOwnerDashboard from './Components/HotelOwnerDashboard';
import Checkout from './Components/Checkout';
import Spinner from './Components/Spinner';
import WelcomePage from './Components/WelcomePage';
import ProfilePage from './Components/Profile';
import UpdateEmail from './Components/UpdateEmail';
import ChangePassword from './Components/ChangePassword';
import AlternativePhoneNumber from './Components/AlternativePhoneNumber';
import OrdersPage from './Components/OrdersPage';
import ContactUs from './Components/ContactUs';

const AppRoutes = ({ isOtherOpen, setIsOtherOpen, isProfileOpen, setIsProfileOpen, loading }) => {
  const location = useLocation(); // Get current route

  // Check if the current route is for Admin or Hotel Owner
  const isAdminOrHotelOwner = location.pathname === '/admin' || location.pathname === '/hotelowner';

  return (
    <>
      {/* Conditionally render Navbar only if not on the Admin or Hotel Owner route */}
      {!isAdminOrHotelOwner && (
        <>
          {isOtherOpen && (
            <div className="bg-white h-screen w-[450px] shadow-xl absolute">
              <Others setIsOpen={setIsOtherOpen} />
            </div>
          )}

          {isProfileOpen && (
            <div className="bg-white h-screen w-[450px] shadow-xl absolute">
              <UserProfile setIsProfileOpen={setIsProfileOpen} />
            </div>
          )}

          <Navbar setIsOtherOpen={setIsOtherOpen} setIsProfileOpen={setIsProfileOpen} />
        </>
      )}

      {/* Show spinner while loading */}
      {loading && <Spinner />} {/* Spinner component while loading */}

      {!loading && ( // Only show routes when loading is false
        <Routes>
          <Route path="/" element={<Home />} exact />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/search" element={<Search />} />
          <Route path="/item/:id" element={<ItemDetail />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/reset-password" element={<ResetPassword />} />
          <Route path="/aboutus" element={<AboutUs />} />
          <Route path="/restaurant/:id" element={<ItemDetail />} />
          <Route path="/profile/:id" element={<UserProfile />} />
          <Route path="/cart" element={<Cart />} />
          <Route path="/menu/:id" element={<Menu />} />
          <Route path="/menus/:id" element={<MenuDetails />} />
          <Route path="/restaurant/:restaurantId" element={<RestaurantItems />} />
          <Route path="/admin" element={<AdminDashboard />} />
          <Route path="/hotelowner" element={<HotelOwnerDashboard />} />
          <Route path="/checkout" element={<Checkout />} />
          <Route path="/order-confirmation/:orderId" element={<OrderConfirmation />} />
          <Route path="/welcome" element={<WelcomePage/>}/>
          <Route path="/profile" element={<ProfilePage/>}/>
          <Route path="/profile/update-email" element={<UpdateEmail/>}/>
          <Route path="/profile/alternative-phone" element={<AlternativePhoneNumber/>}/>
          <Route path="/profile/change-password" element={<ChangePassword/>} />
          <Route path="/profile/orders" element={<OrdersPage/>}/>
          <Route path="/contact" element={<ContactUs/>}/>
        </Routes>
      )}
    </>
  );
};

function App() {
  const [isOtherOpen, setIsOtherOpen] = useState(false);
  const [isProfileOpen, setIsProfileOpen] = useState(false);
  const [loading, setLoading] = useState(false); // State for loading

  // Show spinner during route transitions
  const handleLoading = (location) => {
    console.log("Loading started..."); // Log when loading starts
    setLoading(true); // Start loading when route changes

    const timer = setTimeout(() => {
      console.log("Loading finished."); // Log when loading finishes
      setLoading(false); // Stop loading after a small delay
    }, 5000); // Adjust this timeout based on your needs

    return () => {
        clearTimeout(timer); // Clean up the timer
        console.log("Loading canceled."); // Log if loading is canceled (e.g., if the component unmounts)
    };
};


  return (
    <Router>
      <AppRoutes 
        isOtherOpen={isOtherOpen} 
        setIsOtherOpen={setIsOtherOpen} 
        isProfileOpen={isProfileOpen} 
        setIsProfileOpen={setIsProfileOpen} 
        loading={loading} // Pass loading state
        onLocationChange={handleLoading}
      />
    </Router>
  );
}

export default App;
