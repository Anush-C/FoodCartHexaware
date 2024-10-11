import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; 
import { useNavigate, useLocation } from 'react-router-dom';
import MenuItemManagement from './MenuItemManagement';
import CategoryManagement from './CategoryItemManagement';
import OrderManagement from './OrderManagement';
import ConfirmBox from './ConfirmBox';
import { FaSignOutAlt, FaUtensils, FaClipboardList, FaTags } from 'react-icons/fa';
import { BsArrowLeftShort } from 'react-icons/bs';

const HotelOwnerDashboard = () => {
  const [restaurant, setRestaurant] = useState({
    id: null,
    name: '',
    description: '',
    phone: '',
    email: '',
    address: '',
    openingHours: '',
    closingHours: '',
  });
  const [activeComponent, setActiveComponent] = useState('menuItemManagement');
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [restaurantId, setRestaurantId] = useState(null);
  const jwtToken = localStorage.getItem('token');
  const navigate = useNavigate();
  const location = useLocation();
  const [open, setOpen] = useState(true);

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedRestaurantId = decodedToken.RestaurantID;
        setRestaurantId(decodedRestaurantId);
      } catch (error) {
        console.error('Error decoding JWT:', error);
        navigate('/login');
      }
    } else {
      navigate('/login');
    }
  }, [jwtToken, navigate]);

  useEffect(() => {
    const queryParams = new URLSearchParams(location.search);
    const restaurantIdFromQuery = queryParams.get('restaurantId');

    const fetchRestaurantDetails = async () => {
      const idToUse = restaurantId || restaurantIdFromQuery;
      if (!idToUse) return;
      try {
        const response = await axios.get(`https://localhost:7263/api/Restaurant/${idToUse}`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
            accept: 'application/json',
          },
        });
        setRestaurant({
          id: response.data.restaurantID,
          name: response.data.restaurantName,
          description: response.data.restaurantDescription,
          phone: response.data.restaurantPhone,
          email: response.data.restaurantEmail,
          address: response.data.restaurantAddress,
          openingHours: response.data.openingHours,
          closingHours: response.data.closingHours,
        });
      } catch (error) {
        console.error('Error fetching restaurant data:', error);
        navigate('/login');
      }
    };

    fetchRestaurantDetails();
  }, [restaurantId, location.search, jwtToken, navigate]);

  if (!restaurant.name) {
    return <div className="loading">Loading...</div>;
  }

  

  const confirmLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  const cancelLogout = () => {
    setIsDialogOpen(false);
  };

  const renderActiveComponent = () => {
    const components = [
      { name: 'menuItemManagement', component: <MenuItemManagement restaurantId={restaurant.id} /> },
      { name: 'categoryManagement', component: <CategoryManagement restaurantId={restaurant.id} /> },
      { name: 'orderManagement', component: <OrderManagement restaurantId={restaurant.id} /> },
    ];

    return (
      <div className="flex flex-col space-y-4">
        {components.map(({ name, component }) => (
          <div
            key={name}
            className={`transition-opacity duration-300 ${
              activeComponent === name ? 'bg-white border border-white shadow-lg' : 'opacity-100'
            } p-4 rounded shadow`}
          >
            {activeComponent === name && component}
          </div>
        ))}
      </div>
    );
  };

  return (
    <div className={`dashboard flex h-screen overflow-hidden bg-gray-100`}>
      <aside className={`bg-white h-full px-8 pt-8 transition-all duration-300 ${open ? "w-72" : "w-20"} relative shadow-lg`}>
        <BsArrowLeftShort
          className={`bg-blue-500 text-white text-3xl rounded-full absolute right-3 top-9 border border-blue-700 cursor-pointer transform transition-transform duration-300 ${!open && "rotate-180"}`}
          onClick={() => setOpen(!open)}
        />
        <h2 className={`text-lg font-semibold font-sans mb-4 border-b pb-2 ${open ? 'block' : 'hidden'}`}>Restaurant Info</h2>
        <div className={`flex flex-col space-y-2 font-sans ${open ? 'block' : 'hidden'}`}>
          <p><strong>Name:</strong> {restaurant.name}</p>
          <p><strong>Description:</strong> {restaurant.description}</p>
          <p><strong>Phone:</strong> {restaurant.phone}</p>
          <p><strong>Email:</strong> {restaurant.email}</p>
          <p><strong>Address:</strong> {restaurant.address}</p>
          <p><strong>Opening Hours:</strong> {restaurant.openingHours}</p>
          <p><strong>Closing Hours:</strong> {restaurant.closingHours}</p>
        </div>
      </aside>

      <main className="content flex-grow p-6 overflow-auto">
        <header className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-bold text-orange-500 font-sans ">{restaurant.name} Dashboard</h1>
          <button 
            className="flex items-center p-2 font-sans rounded hover:bg-red-200 transition" 
            onClick={confirmLogout}
          >
            <FaSignOutAlt className="mr-2" /> Logout
          </button>
        </header>
        
        <nav className="flex space-x-4 mb-4">
          <button className={`flex items-center p-2  font-sans rounded hover:bg-blue-200 transition ${activeComponent === 'menuItemManagement' && 'bg-orange-400'}`} onClick={() => setActiveComponent('menuItemManagement')}>
            <FaUtensils className="mr-2" /> Menu Item Management
          </button>
          <button className={`flex items-center p-2  font-sans rounded hover:bg-blue-200 transition ${activeComponent === 'categoryManagement' && 'bg-orange-400'}`} onClick={() => setActiveComponent('categoryManagement')}>
            <FaTags className="mr-2" /> Category Management
          </button>
          <button className={`flex items-center p-2  font-sans rounded hover:bg-blue-200 transition ${activeComponent === 'orderManagement' && 'bg-orange-400'}`} onClick={() => setActiveComponent('orderManagement')}>
            <FaClipboardList className="mr-2" /> Order Management
          </button>
        </nav>

        <div className="dashboard-content bg-white rounded-lg shadow-lg p-4 transition-transform duration-300">
          {renderActiveComponent()}
        </div>
      </main>
    </div>
  );
};

export default HotelOwnerDashboard;
