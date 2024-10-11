import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import axios from "axios";

function MenuDetails() {
  const location = useLocation();
  const [menuItem, setMenuItem] = useState(null);
  const [error, setError] = useState(null);
  const [jwtToken] = useState(localStorage.getItem('token'));
  const [loading, setLoading] = useState(true);
  const [fetchedData, setFetchedData] = useState(null);

  useEffect(() => {
    if (location.state && location.state.menuId) {
      const menuId = location.state.menuId;
      fetchMenu(menuId);
    } else {
      setError("Menu ID is not available.");
      setLoading(false);
    }
  }, [location.state]);

  const fetchMenu = async (id) => {
    try {
      const response = await axios.get(`https://localhost:7263/api/Menus/${id}`, {
        headers: {
          Authorization: `Bearer ${jwtToken}`,
        },
      });
      setFetchedData(response.data.menuItem);
      if (response.data) {
        setMenuItem(response.data);
      } else {
        setError("Menu item not found.");
      }
    } catch (error) {
      console.error("Error fetching menu items:", error);
      setError("Failed to fetch menu items. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  const navigate = useNavigate(); // Initialize useNavigate

  return (
    <div className="p-6 bg-white border-none shadow-xl min-h-screen">
      {error && <p className="text-red-500 text-center">{error}</p>}
      {loading ? (
        <p className="text-center">Loading menu details...</p>
      ) : menuItem ? (
        <div className="bg-white p-6 rounded-lg shadow-lg mx-auto max-w-screen-lg">
          {/* Menu Item Details */}
          <h1 className="text-3xl font-bold font-sans text-gray-800 mb-4">{fetchedData?.itemName}</h1>

          {/* Reduced width for the image */}
          <img
            src={fetchedData?.imageUrl || process.env.PUBLIC_URL + "/salad.webp"}
            alt={fetchedData?.itemName}
            className=" h-80 object-cover rounded-lg mb-6  mx-auto hover:scale-105 transition-transform duration-300"
          />

          <div>
            <p className="text-lg text-gray-600 mb-4 font-sans">{fetchedData?.itemDescription}</p>
            <p className="text-xl font-bold text-gray-800 font-sans">
              Price: <span className="text-green-600">${fetchedData?.itemPrice?.toFixed(2)}</span>
            </p>
            <p className="text-sm text-gray-500 mt-2 font-sans">Cuisine Type: {fetchedData?.cuisineType}</p>
            <p className="text-sm text-gray-500 font-sans">Ingredients: {Array.isArray(fetchedData?.ingredients) ? fetchedData?.ingredients.join(", ") : fetchedData?.ingredients}</p>
            <p className="text-sm text-gray-500 font-sans">Dietary Info: {fetchedData?.dietaryInfo}</p>
            <p className="text-sm text-gray-500 font-sans">Taste Info: {fetchedData?.tasteInfo}</p>
            <p className="text-sm text-gray-500 font-sans">Availability Status: {fetchedData?.availabilityStatus}</p>
            <p className="text-sm text-gray-500 font-sans">Rating: {fetchedData?.rating}</p>
          </div>

          {/* Restaurants Section */}
          <h2 className="text-2xl font-semibold mt-8 mb-4 font-sans">Available at Restaurants:</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 font-sans">
            {fetchedData?.restaurants && fetchedData?.restaurants.length > 0 ? (
              fetchedData?.restaurants.map((restaurant) => (
                <div
                  key={restaurant.restaurantID}
                  className="border p-4 rounded-lg shadow-lg hover:shadow-xl transition-shadow cursor-pointer"
                  onClick={() => navigate(`/restaurant/${restaurant.restaurantID}`)}
                >
                  {/* Common restaurant image */}
                  <img
                    src={restaurant.imageUrl || process.env.PUBLIC_URL + "/rest.webp"}
                    alt={restaurant.name}
                    className="w-full h-40 object-cover rounded-md mb-4"
                  />
                  <h3 className="text-lg font-bold">{restaurant.name}</h3>
                  <p className="text-sm text-gray-500 ">{restaurant.description}</p>
                  <p className="text-sm mt-2">Address: {restaurant.address}</p>
                  <p className="text-sm mt-2">Contact: {restaurant.phoneNumber}</p>
                  <p className="text-sm mt-2">Email: {restaurant.email}</p>
                </div>
              ))
            ) : (
              <p className="text-center col-span-full">No restaurants available for this menu item.</p>
            )}
          </div>
        </div>
      ) : (
        <p className="text-center">No menu item available.</p>
      )}
    </div>
  );
}

export default MenuDetails;
