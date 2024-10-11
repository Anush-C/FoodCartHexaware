import React, { useEffect, useState } from 'react';
import { FiUser, FiPhone, FiMail, FiMapPin } from 'react-icons/fi';
import { AiOutlineCheckCircle, AiOutlineCloseCircle } from 'react-icons/ai';
import { jwtDecode } from 'jwt-decode';

const DeliveryAgentManagement = () => {
  const [agents, setAgents] = useState([]);
  const [orders, setOrders] = useState([]);
  const [selectedAgent, setSelectedAgent] = useState(null);
  const [orderId, setOrderId] = useState('');
  const [message, setMessage] = useState('');
  const [jwtToken, setJwtToken] = useState(localStorage.getItem('token'));
  const [userID, setUserID] = useState(null);
  const [showOrders, setShowOrders] = useState(false); // State to toggle orders view

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedUserID = decodedToken.UserId;
        console.log('Decoded User ID:', decodedUserID);
        setUserID(decodedUserID);
      } catch (error) {
        console.error('Error decoding JWT:', error);
      }
    }
  }, [jwtToken]);

  useEffect(() => {
    // Fetch all agents
    const fetchAgents = async () => {
      try {
        const response = await fetch('https://localhost:7263/api/DeliveryAgents/Allagents', {
          method: 'GET',
          headers: {
            Authorization: `Bearer ${jwtToken}`,
            accept: '*/*',
          },
        });
        const data = await response.json();
        setAgents(data);
      } catch (error) {
        console.error('Error fetching agents:', error);
        setMessage('Error fetching delivery agents.');
      }
    };

    // Fetch all orders
    const fetchOrders = async () => {
      try {
        const response = await fetch('https://localhost:7263/api/DeliveryAgents', {
          method: 'GET',
          headers: {
            Authorization: `Bearer ${jwtToken}`,
            accept: '*/*',
            'Content-Type': 'application/json',
          },
        });
        const data = await response.json();
        setOrders(data);
      } catch (error) {
        console.error('Error fetching orders:', error);
        setMessage('Error fetching orders.');
      }
    };

    fetchAgents();
    fetchOrders(); // Call the fetchOrders function
  }, [jwtToken]);

  const updateAvailability = async (agentId, availability) => {
    try {
      const response = await fetch(`https://localhost:7263/api/DeliveryAgents/${agentId}/updateAvailability`, {
        method: 'PUT',
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(availability),
      });

      if (response.ok) {
        setMessage(`Agent ${agentId} availability updated to ${availability ? 'Available' : 'Unavailable'}`);
        setAgents(agents.map(agent => agent.deliveryAgentID === agentId ? { ...agent, isAvailable: availability } : agent));
      } else {
        setMessage('Failed to update agent availability.');
      }
    } catch (error) {
      console.error('Error updating availability:', error);
      setMessage('Error updating availability.');
    }
  };

  const assignAgentToOrder = async () => {
    if (!selectedAgent || !orderId) {
      setMessage('Please select an agent and enter a valid Order ID.');
      return;
    }

    try {
      const response = await fetch(`https://localhost:7263/api/DeliveryAgents/${orderId}/assignAgent/${selectedAgent}`, {
        method: 'PUT',
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          'Content-Type': 'application/json',
        },
      });

      if (response.ok) {
        setMessage(`Agent ${selectedAgent} assigned to order ${orderId} successfully.`);
        setAgents(agents.map(agent => agent.deliveryAgentID === selectedAgent ? { ...agent, isAvailable: false } : agent));
      } else {
        setMessage('Failed to assign agent to order.');
      }
    } catch (error) {
      console.error('Error assigning agent to order:', error);
      setMessage('Error assigning agent to order.');
    }
  };

  return (
    <div className="max-w-6xl mx-auto p-8 font-sans">
      <h2 className="text-3xl font-bold mb-6 text-gray-800">Delivery Agent Management</h2>

      {message && <p className="bg-blue-100 text-blue-700 p-4 mb-6 rounded-lg">{message}</p>}

      {/* All Delivery Agents as Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6">
        {agents.map(agent => (
          <div
            key={agent.deliveryAgentID}
            className="bg-white shadow-lg rounded-lg p-6 transition-transform transform hover:scale-105 hover:shadow-xl"
          >
            <div className="flex items-center space-x-4">
              <FiUser className="text-gray-500 text-2xl" />
              <div>
                <h3 className="text-xl font-semibold">{agent.name}</h3>
                <div className="text-gray-600 text-sm">
                  <div className="flex items-center space-x-2">
                    <FiPhone />
                    <span>{agent.phoneNumber}</span>
                  </div>
                  <div className="flex items-center space-x-2">
                    <FiMail />
                    <span>{agent.email}</span>
                  </div>
                  <div className="flex items-center space-x-2">
                    <FiMapPin />
                    <span>{agent.address}</span>
                  </div>
                </div>
              </div>
            </div>
            <div className="mt-4">
              {agent.isAvailable ? (
                <div className="text-green-500 flex items-center space-x-2">
                  <AiOutlineCheckCircle className="text-2xl" />
                  <span>Available</span>
                </div>
              ) : (
                <div className="text-red-500 flex items-center space-x-2">
                  <AiOutlineCloseCircle className="text-2xl" />
                  <span>Unavailable</span>
                </div>
              )}
            </div>
            <button
              className={`mt-4 w-full py-2 text-white rounded-md ${
                agent.isAvailable ? 'bg-red-500 hover:bg-red-600' : 'bg-green-500 hover:bg-green-600'
              } transition duration-300`}
              onClick={() => updateAvailability(agent.deliveryAgentID, !agent.isAvailable)}
            >
              {agent.isAvailable ? 'Mark Unavailable' : 'Mark Available'}
            </button>
          </div>
        ))}
      </div>

      {/* Toggle Orders View */}
      <div className="bg-white shadow-md rounded-lg overflow-hidden mt-8">
        <div
          className="cursor-pointer p-4 bg-gray-800 text-white text-2xl font-semibold"
          onClick={() => setShowOrders(!showOrders)}
        >
          {showOrders ? 'Hide Orders' : 'View All Orders'}
        </div>

        {showOrders && (
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 mt-6">
            {orders.map(order => (
              <div
                key={order.orderID}
                className="bg-white shadow-lg rounded-lg p-6 transition-transform transform hover:scale-105 hover:shadow-xl"
              >
                <h3 className="text-xl font-semibold">Order ID: {order.orderID}</h3>
<p className="text-gray-600">Customer ID: {order.userID}</p>
<p className="text-gray-600">Total Price: {order.totalPrice}</p>
<p
  className={`text-gray-600 font-semibold ${
    order.orderStatus === 'Pending' ? 'text-yellow-500' :
    order.orderStatus === 'Completed' ? 'text-green-500' :
    order.orderStatus === 'Cancelled' ? 'text-red-500' : 'text-gray-500'
  }`}
>
  Status: {order.orderStatus}
</p>
<p className="text-gray-600">Agent ID: {order.deliveryAgentID || 'Not Assigned'}</p>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Agent Assignment */}
      <div className="bg-white shadow-md rounded-lg p-6 mt-8">
        <h3 className="text-2xl font-semibold mb-4">Assign Agent to Order</h3>
        <div className="space-y-4">
          <div>
            <label htmlFor="orderId" className="block text-gray-700 font-medium">
              Order ID:
            </label>
            <input
              type="text"
              id="orderId"
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:ring-blue-500 focus:border-blue-500"
              value={orderId}
              onChange={e => setOrderId(e.target.value)}
              placeholder="Enter Order ID"
            />
          </div>
          <div>
            <label htmlFor="agentSelect" className="block text-gray-700 font-medium">
              Select Agent:
            </label>
            <select
              id="agentSelect"
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:ring-blue-500 focus:border-blue-500"
              value={selectedAgent}
              onChange={e => setSelectedAgent(e.target.value)}
            >
              <option value="">Select an Agent</option>
              {agents.map(agent => (
                <option key={agent.deliveryAgentID} value={agent.deliveryAgentID}>
                  {agent.name}
                </option>
              ))}
            </select>
          </div>
          <button
            className="w-full py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition duration-300"
            onClick={assignAgentToOrder}
          >
            Assign Agent
          </button>
        </div>
      </div>
    </div>
  );
};

export default DeliveryAgentManagement;
