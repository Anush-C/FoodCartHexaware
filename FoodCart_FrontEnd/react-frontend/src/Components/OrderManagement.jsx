import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; 
import { FaEdit } from "react-icons/fa";// Adjust import if necessary

const OrderManagement = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [restaurantId, setRestaurantId] = useState(null);
  const [error, setError] = useState(null);
  const [selectedOrderId, setSelectedOrderId] = useState(null);
  const [newStatus, setNewStatus] = useState('');

  const jwtToken = localStorage.getItem('token');

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedRestaurantId = decodedToken.RestaurantID;
        setRestaurantId(decodedRestaurantId);
      } catch (error) {
        console.error('Error decoding JWT:', error);
      }
    } else {
      setError('No token found');
      setLoading(false);
    }
  }, [jwtToken]);

  useEffect(() => {
    const fetchOrders = async () => {
      if (!restaurantId) return;

      try {
        const response = await axios.get(`https://localhost:7263/api/Restaurant/orders/${restaurantId}`, {
          headers: {
            accept: '*/*',
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        setOrders(response.data);
        setLoading(false);
      } catch (err) {
        setError('Failed to fetch orders');
        setLoading(false);
      }
    };

    fetchOrders();
  }, [restaurantId, jwtToken]);

  const updateOrderStatus = async (orderId) => {
    try {
      // Ensure newStatus is a string and is not empty
      if (!newStatus || typeof newStatus !== 'string') {
        console.error('Invalid status value');
        return;
      }
  
      // Prepare the payload
      const payload = { status: newStatus };
  
      // Make the API request
      await axios.put(
        `https://localhost:7263/api/Restaurant/order/${orderId}`,
        payload, // Use the payload here
        {
          headers: {
            accept: '*/*',
            Authorization: `Bearer ${jwtToken}`,
            'Content-Type': 'application/json',
          },
        }
      );
  
      // Update the state or fetch orders again if needed
      setOrders((prevOrders) =>
        prevOrders.map((order) => (order.orderID === orderId ? { ...order, orderStatus: newStatus } : order))
      );
  
      // Reset state variables after successful update
      setSelectedOrderId(null); // Close the input field after successful update
      setNewStatus(''); // Reset new status input
    } catch (error) {
      console.error('Failed to update order status:', error.response ? error.response.data : error.message);
    }
  };
  

  if (loading) return <p className="text-center text-gray-600 font-sans">Loading orders...</p>;
  if (error) return <p className="text-center text-red-600 font-sans">{error}</p>;

  // Function to determine the status color
  const getStatusColor = (status) => {
    if (status === 'Completed') return 'text-green-500';
    if (status === 'Pending') return 'text-yellow-500';
    if (status === 'Cancelled') return 'text-red-500';
    return 'text-gray-600'; // Fallback color
  };

  return (
    <div className="container mx-auto px-4 font-sans mt-4">
      <h2 className="text-3xl font-bold text-center mb-8">Orders</h2>
      {orders.length > 0 ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {orders.map((order) => (
            <div key={order.orderID} className="bg-white shadow-md rounded-lg p-6 border border-gray-200">
              <p className="text-lg font-semibold text-gray-700"><strong>Order ID:</strong> {order.orderID}</p>
              <p className="text-gray-600 mt-2"><strong>Total Price:</strong> ${order.totalPrice}</p>
              <p className="text-gray-600"><strong>Shipping Address:</strong> {order.shippingAddress}</p>
              <p className={`font-bold ${getStatusColor(order.orderStatus)}`}>
                <strong>Status:</strong> {order.orderStatus}
              </p>
              <p className="text-gray-600"><strong>Order Time:</strong> {new Date(order.orderTime).toLocaleString()}</p>
              <p className="text-gray-600">
                <strong>Delivery Time:</strong>{' '}
                {order.deliveryTime ? new Date(order.deliveryTime).toLocaleString() : 'Not delivered yet'}
              </p>

              {selectedOrderId === order.orderID ? (
                <div>
                  <select
                    value={newStatus}
                    onChange={(e) => setNewStatus(e.target.value)}
                    className="mt-2 p-2 border rounded"
                  >
                    <option value="">Select status</option>
                    <option value="Completed">Completed</option>
                    <option value="Pending">Pending</option>
                    <option value="Cancelled">Cancelled</option>
                  </select>
                  <button
                    onClick={() => updateOrderStatus(order.orderID)}
                    className="bg-blue-500 text-white px-4 py-2 rounded mt-2 ml-4"
                  >
                    Update
                  </button>
                </div>
              ) : (
                <button
                  onClick={() => setSelectedOrderId(order.orderID)}
                  className="bg-white text-black px-4 py-2 rounded hover:bg-transparent mt-2"
                >
                  <FaEdit />
                </button>
              )}
            </div>
          ))}
        </div>
      ) : (
        <p className="text-center text-gray-600">No orders found for this restaurant.</p>
      )}
    </div>
  );
};

export default OrderManagement;
