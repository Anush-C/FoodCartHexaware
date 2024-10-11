import React, { useEffect, useState } from 'react';
import {jwtDecode} from 'jwt-decode'; // Correct import for jwtDecode

const OrdersPage = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState(null);
  const [cancelReason, setCancelReason] = useState('');
  const [isConfirming, setIsConfirming] = useState(false);
  const [orderToCancel, setOrderToCancel] = useState(null);

  useEffect(() => {
    const jwtToken = localStorage.getItem('token');
    if (jwtToken) {
      const { UserId } = jwtDecode(jwtToken); // Destructured UserId

      // Fetch user orders
      fetch(`https://localhost:7263/api/UserProfile/orders/${UserId}`, {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          Accept: 'application/json',
        },
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error('Failed to fetch orders');
          }
          return response.json();
        })
        .then((data) => {
          setOrders(data);
        })
        .catch((error) => {
          console.error('Error fetching orders:', error);
          setError('No orders for the past 30 days');
        });
    }
  }, []);

  // Handle order cancellation
  const handleCancelOrder = (orderID) => {
    setOrderToCancel(orderID);
    setIsConfirming(true);
  };

  const confirmCancellation = () => {
    if (orderToCancel && cancelReason) {
      const jwtToken = localStorage.getItem('token');
      fetch(`https://localhost:7263/api/Orders/cancel/${orderToCancel}`, {
        method: 'POST',
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ reason: cancelReason }), // Send reason with the request
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error('Failed to cancel order');
          }
          return response.json();
        })
        .then(() => {
          // Update the order status to 'Cancelled' without removing it from the list
          setOrders((prevOrders) =>
            prevOrders.map((order) =>
              order.orderID === orderToCancel
                ? { ...order, orderStatus: 'Cancelled' }
                : order
            )
          );
          setIsConfirming(false);
          setCancelReason('');
          setOrderToCancel(null);
        })
        .catch((error) => {
          console.error('Error cancelling order:', error);
          setError('Error cancelling the order');
        });
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-white to-black flex justify-center items-center p-8 font-sans">
      <div className="w-full max-w-4xl bg-white rounded-lg shadow-lg p-6">
        <h2 className="text-3xl font-bold text-orange-500 mb-6 text-center">Your Orders</h2>

        {/* Display error message */}
        {error ? (
          <div className="text-red-500 text-center">{error}</div>
        ) : orders.length > 0 ? (
          <ul className="space-y-4">
            {orders.map((order) => (
              <li
                key={order.orderID}
                className="bg-white p-4 border border-gray-200 rounded-lg shadow-md transition duration-300 hover:shadow-lg"
              >
                {/* Common image for each order */}
                <img
                  src="/turkey.jpg"
                  alt="Order Icon"
                  className="w-20 h-20 mb-4 mx-auto rounded-full object-cover border-4 border-gray-300 shadow-md"
                />

                <h3 className="text-xl font-semibold text-gray-700 mb-2">Order ID: {order.orderID}</h3>
                <p className="text-lg text-gray-600">
                  Total Price: <span className="font-bold">${order.totalPrice.toFixed(2)}</span>
                </p>
                <p className="text-lg text-gray-600">
                  Shipping Address: <span className="font-semibold">{order.shippingAddress}</span>
                </p>
                <p className="text-lg text-gray-600">
                  Order Status:{" "}
                  <span
                    className={`font-semibold ${
                      order.orderStatus === "Delivered"
                        ? "text-green-600"
                        : order.orderStatus === "Cancelled"
                        ? "text-red-500"
                        : "text-yellow-500"
                    }`}
                  >
                    {order.orderStatus}
                  </span>
                </p>
                <p className="text-lg text-gray-600">
                  Order Time: {new Date(order.orderTime).toLocaleString()}
                </p>

                {/* Display order items */}
                <div className="mt-4">
                  <h4 className="text-lg font-semibold text-gray-700">Items:</h4>
                  <ul className="ml-4 list-disc">
                    {order.items.map((item, index) => (
                      <li key={index} className="text-gray-600">
                        {item.quantity}x {item.itemName}
                      </li>
                    ))}
                  </ul>
                </div>

                {/* Cancel order section */}
                {order.orderStatus === 'Pending' && (
                  <div className="mt-4">
                    <select
                      className="border border-gray-300 p-2 rounded"
                      value={cancelReason}
                      onChange={(e) => setCancelReason(e.target.value)}
                    >
                      <option value="">Select a reason</option>
                      <option value="Changed my mind">Changed my mind</option>
                      <option value="Wrong item ordered">Wrong item ordered</option>
                      <option value="Found a better price">Found a better price</option>
                    </select>
                    <button
                      onClick={() => handleCancelOrder(order.orderID)}
                      className="ml-2 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
                    >
                      Cancel Order
                    </button>
                  </div>
                )}
              </li>
            ))}
          </ul>
        ) : (
          <div className="text-gray-500 text-center text-lg">No orders found.</div>
        )}
      </div>

      {/* Confirmation box */}
      {isConfirming && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 flex justify-center items-center">
          <div className="bg-white p-6 rounded shadow-lg">
            <h3 className="text-lg font-semibold mb-4">Are you sure you want to cancel this order?</h3>
            <button
              onClick={confirmCancellation}
              className="bg-red-500 text-white px-4 py-2 rounded mr-4 hover:bg-red-600"
            >
              Yes, Cancel
            </button>
            <button
              onClick={() => setIsConfirming(false)}
              className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
            >
              No, Go Back
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default OrdersPage;
