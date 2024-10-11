import React from 'react';
import DashboardOverview from './DashboardOverview';
import MenuManagement from './MenuManagement';


const RestaurantDashboard = () => {
  return (
    <div>
      <h1>Restaurant Dashboard</h1>
      <DashboardOverview />
      <MenuManagement />
    
      {/* Add CategoryManagement when created */}
    </div>
  );
};

export default RestaurantDashboard;
