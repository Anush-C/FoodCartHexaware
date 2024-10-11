// src/components/AdminDashboard.js
import React, { useState } from 'react';
import UserManagement from './UserManagement';
import RestaurantManagement from './RestaurantManagement';
import MenuManagement from './MenuManagement';
import CategoryManagement from './CategoryManagement';
import ConfirmDialog from './ConfirmBox';
import './Animation.css';
import { CiLogout } from "react-icons/ci";
import { useNavigate } from 'react-router-dom';
import { FaUserAlt } from "react-icons/fa";
import { IoMdRestaurant } from "react-icons/io"; // Import the icon
import { MdOutlineRestaurantMenu, MdCategory } from "react-icons/md";
import DeliveryAgentManagement from './DeliveryAgentManagement';

const AdminDashboard = () => {
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [activeComponent, setActiveComponent] = useState('userManagement');

  const handleLogoutClick = () => {
    setIsDialogOpen(true);
  };

  const navigate = useNavigate();
  const handleConfirmLogout = () => {
    console.log("User logged out");
    setIsDialogOpen(false);
    navigate('/');
  };

  const handleCloseDialog = () => {
    setIsDialogOpen(false);
  };

  const renderActiveComponent = () => {
    switch (activeComponent) {
      case 'userManagement':
        return <UserManagement />;
      case 'restaurantManagement':
        return <RestaurantManagement />;
      case 'menuManagement':
        return <MenuManagement />;
      case 'categoryManagement':
        return <CategoryManagement />;
      case 'deliveryagentManagement' :
        return <DeliveryAgentManagement/>
      default:
        return <UserManagement />;
    }
  };

  return (
    <div className="flex min-h-screen">
      {isDialogOpen && (
        <ConfirmDialog
          isOpen={isDialogOpen}
          onClose={handleCloseDialog}
          onConfirm={handleConfirmLogout}
          message="Are you sure you want to log out?"
        />
      )}
      <header className="bg-blue-600 text-white p-4 flex justify-between items-center shadow-md fixed top-0 left-0 w-full z-10">
        <h1 className="text-2xl font-semibold">Admin Dashboard</h1>
        <button onClick={handleLogoutClick} className="text-white hover:bg-blue-500 p-2 rounded">
          <CiLogout />
        </button>
      </header>

      <aside className="bg-gray-800 text-white w-64 p-6 shadow-lg rounded-sm fixed top-0 left-0 bottom-0  pt-16 mt-10"> {/* Sidebar */}
        <h2 className="text-lg font-bold mb-6 text-center text-white font-sans">Management</h2>
        <ul>
          <li>
            <button
              className={`w-full text-left py-3 px-4 rounded-lg transition-all duration-300 bg-gray-800 font-sans mb-4 flex items-center
                ${activeComponent === 'userManagement' ? 'bg-green-600 text-white border-l-4 border-blue-400' : 'hover:bg-blue-500 hover:text-white'}`}
              onClick={() => setActiveComponent('userManagement')}
            >
              <FaUserAlt className="mr-2" /> {/* Icon added here */}
              User Management
            </button>
          </li>
          <li>
            <button
              className={`w-full text-left py-3 px-4 rounded-lg transition-all duration-300 bg-gray-800 font-sans mb-4 flex items-center
                ${activeComponent === 'restaurantManagement' ? 'bg-green-600 text-white border-l-4 border-green-400' : 'hover:bg-green-500 hover:text-white'}`}
              onClick={() => setActiveComponent('restaurantManagement')}
            >
              <IoMdRestaurant size={25} className="mr-2" />
              Restaurant Management
            </button>
          </li>
          <li>
            <button
              className={`w-full text-left py-3 px-4 rounded-lg transition-all duration-300 bg-gray-800 font-sans flex items-center
                ${activeComponent === 'menuManagement' ? 'bg-green-600 text-white border-l-4 border-yellow-400' : 'hover:bg-yellow-500 hover:text-white'}`}
              onClick={() => setActiveComponent('menuManagement')}
            >
              <MdOutlineRestaurantMenu size={22} className="mr-2" />
              Menu Management
            </button>
          </li>
          <li>
            <button
              className={`w-full text-left py-3 px-4 rounded-lg transition-all duration-300 bg-gray-800 font-sans mt-4 flex items-center
                ${activeComponent === 'categoryManagement' ? 'bg-green-600 text-white border-l-4 border-red-400' : 'hover:bg-red-500 hover:text-white'}`}
              onClick={() => setActiveComponent('categoryManagement')}
            >
              <MdCategory size={25} className="mr-2" />
              Category Management
            </button>
          </li>
          <li>
            <button
              className={`w-full text-left py-3 px-4 rounded-lg transition-all duration-300 bg-gray-800 font-sans mt-4 flex items-center
                ${activeComponent === 'deliveryagentManagement' ? 'bg-green-600 text-white border-l-4 border-red-400' : 'hover:bg-red-500 hover:text-white'}`}
              onClick={() => setActiveComponent('deliveryagentManagement')}
            >
              <MdCategory size={25} className="mr-2" />
              DeliveryAgent Management
            </button>
          </li>
        </ul>
      </aside>

      <main className="ml-64 flex-grow p-6 bg-gray-100 min-h-screen overflow-y-auto pt-20"> {/* Main Content */}
        {renderActiveComponent()}
      </main>
    </div>
  );
};

export default AdminDashboard;
