import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

function RestaurantItems() {
  const { restaurantId } = useParams(); // Get the restaurant ID from the URL
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchItems = async () => {
      try {
        const response = await axios.get(`https://localhost:7263/api/Menus/byRestaurant/${restaurantId}`); // Use the correct API endpoint
        setItems(response.data); // Assuming the response data contains the array of items
      } catch (error) {
        console.error("Error fetching restaurant items:", error);
        setError("Failed to fetch restaurant items. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchItems();
  }, [restaurantId]);

  return (
    <div className="p-6">
      {loading ? (
        <p className="text-center">Loading restaurant items...</p>
      ) : error ? (
        <p className="text-red-500 text-center">{error}</p>
      ) : (
        <div>
          <h1 className="text-2xl font-bold text-gray-800 mb-4">Menu Items</h1>
          {items.length > 0 ? (
            <div className="grid grid-cols-1 gap-4">
              {items.map(item => (
                <div key={item.itemId} className="border p-4 rounded-lg shadow-lg">
                  <h2 className="text-lg font-semibold">{item.itemName}</h2>
                  <p className="text-sm">{item.itemDescription}</p>
                  <p className="font-medium text-lg">Price: <span className="text-green-600">${item.itemPrice.toFixed(2)}</span></p>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-center">No items available for this restaurant.</p>
          )}
        </div>
      )}
    </div>
  );
}

export default RestaurantItems;
