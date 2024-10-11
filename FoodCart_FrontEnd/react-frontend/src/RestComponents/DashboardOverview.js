import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, Grid, Typography } from '@mui/material'; // Using Material-UI for design

const DashboardOverview = () => {
  const [dashboardData, setDashboardData] = useState({
    dailyOrders: 0,
    revenueToday: 0,
    revenueWeek: 0,
    revenueMonth: 0,
    pendingOrders: 0,
  });

  useEffect(() => {
    const fetchDashboardData = async () => {
      const response = await axios.get('/api/restaurant/dashboard');
      setDashboardData(response.data);
    };
    fetchDashboardData();
  }, []);

  return (
    <Grid container spacing={3}>
      <Grid item xs={12} md={4}>
        <Card>
          <Typography variant="h6">Daily Orders</Typography>
          <Typography variant="h4">{dashboardData.dailyOrders}</Typography>
        </Card>
      </Grid>
      <Grid item xs={12} md={4}>
        <Card>
          <Typography variant="h6">Revenue Today</Typography>
          <Typography variant="h4">${dashboardData.revenueToday}</Typography>
        </Card>
      </Grid>
      <Grid item xs={12} md={4}>
        <Card>
          <Typography variant="h6">Pending Orders</Typography>
          <Typography variant="h4">{dashboardData.pendingOrders}</Typography>
        </Card>
      </Grid>
    </Grid>
  );
};

export default DashboardOverview;
