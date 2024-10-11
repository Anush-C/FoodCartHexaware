import React from 'react';
import { Link } from 'react-router-dom'; // Import Link for routing

const Sidebar = () => {
  return (
    <div className="bg-gray-800 text-white w-64 h-full p-4">
      <h2 className="text-lg font-bold mb-4">Admin Menu</h2>
      <ul className="space-y-2">
        <li>
          <Link to="/admin/users" className="hover:text-blue-400">User Management</Link>
        </li>
        <li>
          <Link to="/admin/restaurants" className="hover:text-blue-400">Restaurant Management</Link>
        </li>
        <li>
          <Link to="/admin/menu" className="hover:text-blue-400">Menu Management</Link>
        </li>
        <li>
          <Link to="/admin/categories" className="hover:text-blue-400">Category Management</Link>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
