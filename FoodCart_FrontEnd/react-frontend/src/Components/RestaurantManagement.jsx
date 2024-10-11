import React, { useEffect, useState } from "react";
import { FaEdit, FaSave } from "react-icons/fa";
import { MdDelete } from "react-icons/md";
import { AiFillCloseCircle } from "react-icons/ai";
import { BsFillHouseAddFill } from "react-icons/bs";
import { jwtDecode } from "jwt-decode";

const RestaurantManagement = () => {
  const [restaurants, setRestaurants] = useState([]);
  const [view, setView] = useState("viewAll"); // viewAll, add, edit
  const [userID, setUserID] = useState(null);
  const [jwtToken, setJwtToken] = useState(localStorage.getItem('token'));
  const [newRestaurant, setNewRestaurant] = useState({
    restaurantName: "",
    restaurantDescription: "",
    restaurantPhone: "",
    restaurantEmail: "",
    restaurantAddress: "",
    openingHours: "",
    closingHours: "",
  });
  const [updatedRestaurant, setUpdatedRestaurant] = useState(null);

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
    fetchRestaurants();
  }, []);

  const fetchRestaurants = async () => {
    const response = await fetch("https://localhost:7263/api/Admin/restaurants", {
      headers: {
        Authorization: `Bearer ${jwtToken}`,
        accept: "application/json"
      }
    });
    const data = await response.json();
    setRestaurants(data);
  };

  const addRestaurant = async () => {
    try {
      const response = await fetch("https://localhost:7263/api/Admin/restaurants", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          "Content-Type": "application/json", // Corrected header
        },
        body: JSON.stringify(newRestaurant),
      });
  
      // Check for successful response
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
  
      fetchRestaurants(); // Refresh the restaurant list after adding
      setNewRestaurant({
        restaurantName: "",
        restaurantDescription: "",
        restaurantPhone: "",
        restaurantEmail: "",
        restaurantAddress: "",
        openingHours: "",
        closingHours: "",
      });
      setView("viewAll");
    } catch (error) {
      console.error("Error adding restaurant", error);
      alert("Error adding restaurant: " + error.message);
    }
  };
  
  const updateRestaurant = async () => {
    await fetch(`https://localhost:7263/api/Admin/restaurants/${updatedRestaurant.restaurantID}`, {
      method: "PUT",
      headers: {
        Authorization: `Bearer ${jwtToken}`,
        "Content-type": "application/json"
      },
      body: JSON.stringify(updatedRestaurant),
    });
    fetchRestaurants();
    setUpdatedRestaurant(null);
    setView("viewAll");
  };

  const deleteRestaurant = async (id) => {
    await fetch(`https://localhost:7263/api/Admin/restaurants/${id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${jwtToken}`,
        accept: "application/json"
      }
    });
    fetchRestaurants();
  };

  return (
    <div className="container mx-auto p-4 font-sans text-sm">
      <h2 className="text-2xl font-bold mb-6 font-sans text-black">Restaurant Management</h2>

      {view === "add" && (
        <div className="mb-4 p-4 bg-gray-100 rounded-lg shadow">
          <h3 className="text-xl font-bold mb-2 font-sans">Add Restaurant</h3>
          <input
            type="text"
            placeholder="Restaurant Name"
            value={newRestaurant.restaurantName}
            onChange={(e) => setNewRestaurant({ ...newRestaurant, restaurantName: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Description"
            value={newRestaurant.restaurantDescription}
            onChange={(e) => setNewRestaurant({ ...newRestaurant, restaurantDescription: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Phone Number"
            value={newRestaurant.restaurantPhone}
            onChange={(e) => setNewRestaurant({ ...newRestaurant, restaurantPhone: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="email"
            placeholder="Email"
            value={newRestaurant.restaurantEmail}
            onChange={(e) => setNewRestaurant({ ...newRestaurant, restaurantEmail: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Address"
            value={newRestaurant.restaurantAddress}
            onChange={(e) => setNewRestaurant({ ...newRestaurant, restaurantAddress: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Opening Hours (HH:MM:SS)"
            value={newRestaurant.openingHours}
            onChange={(e) => setNewRestaurant({ ...newRestaurant, openingHours: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Closing Hours (HH:MM:SS)"
            value={newRestaurant.closingHours}
            onChange={(e) => setNewRestaurant({ ...newRestaurant, closingHours: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <div className="flex justify-between mt-4">
            <button onClick={addRestaurant} className="bg-black text-white px-4 py-2 rounded-lg hover:bg-green-600 transition">
              <FaSave /> 
            </button>
            <button onClick={() => setView("viewAll")} className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition">
              <AiFillCloseCircle className="rounded-md" />
            </button>
          </div>
        </div>
      )}

      {view === "edit" && updatedRestaurant && (
        <div className="mb-4 p-4 bg-gray-100 rounded-lg shadow">
          <h3 className="text-xl font-bold mb-2">Edit Restaurant</h3>
          <input
            type="text"
            placeholder="Restaurant Name"
            value={updatedRestaurant.restaurantName}
            onChange={(e) => setUpdatedRestaurant({ ...updatedRestaurant, restaurantName: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Description"
            value={updatedRestaurant.restaurantDescription}
            onChange={(e) => setUpdatedRestaurant({ ...updatedRestaurant, restaurantDescription: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Phone Number"
            value={updatedRestaurant.restaurantPhone}
            onChange={(e) => setUpdatedRestaurant({ ...updatedRestaurant, restaurantPhone: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="email"
            placeholder="Email"
            value={updatedRestaurant.restaurantEmail}
            onChange={(e) => setUpdatedRestaurant({ ...updatedRestaurant, restaurantEmail: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Address"
            value={updatedRestaurant.restaurantAddress}
            onChange={(e) => setUpdatedRestaurant({ ...updatedRestaurant, restaurantAddress: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Opening Hours (HH:MM:SS)"
            value={updatedRestaurant.openingHours}
            onChange={(e) => setUpdatedRestaurant({ ...updatedRestaurant, openingHours: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Closing Hours (HH:MM:SS)"
            value={updatedRestaurant.closingHours}
            onChange={(e) => setUpdatedRestaurant({ ...updatedRestaurant, closingHours: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <div className="flex justify-between mt-4">
            <button onClick={updateRestaurant} className="bg-black text-white px-4 py-2 rounded-lg hover:bg-green-600 transition">
              <FaSave /> 
            </button>
            <button onClick={() =>  setView("viewAll")} className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition">
              <AiFillCloseCircle className="rounded-md" />
            </button>
          </div>
        </div>
      )}

{view === "viewAll" && (
  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
    {restaurants.map((restaurant) => (
      <div key={restaurant.restaurantID} className="bg-white rounded-lg shadow-md p-4">
        <img
          src="/rest.webp" // Replace with your common image URL
          alt="Restaurant"
          className="w-full h-32 object-cover rounded-lg mb-4"
        />
        <h3 className="text-xl font-semibold">{restaurant.restaurantName}</h3>
        <p className="text-gray-700 mt-1 italic">{restaurant.restaurantDescription}</p>
        <p className="text-gray-600 mt-2"><strong>Phone:</strong> {restaurant.restaurantPhone}</p>
        <p className="text-gray-600"><strong>Email:</strong> {restaurant.restaurantEmail}</p>
        <p className="text-gray-600"><strong>Address:</strong> {restaurant.restaurantAddress}</p>
        <p className="text-gray-600"><strong>Opening Hours:</strong>{restaurant.openingHours} - {restaurant.closingHours}</p>
        {/* Move the buttons here */}
        <div className="mt-4 flex justify-between">
          <button
            onClick={() => {
              setUpdatedRestaurant(restaurant);
              setView("edit");
            }}
            className="text-black hover:text-white p-2"
          >
            <FaEdit />
          </button>
          <button
            onClick={() => deleteRestaurant(restaurant.restaurantID)}
            className="text-red-500 hover:text-white p-2"
          >
            <MdDelete />
          </button>
        </div>
      </div>
    ))}
  </div>
)}


      <div className="mt-6">
        <button onClick={() => setView("add")} className="bg-black text-white px-4 py-2 rounded-lg hover:bg-green-600 transition">
          <BsFillHouseAddFill /> Add Restaurant
        </button>
      </div>
    </div>
  );
};

export default RestaurantManagement;
