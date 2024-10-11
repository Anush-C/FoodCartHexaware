import React, { useState, useEffect } from "react";
import axios from "axios";
import { IoIosAddCircle } from "react-icons/io";
import { FaEdit, FaSave } from "react-icons/fa";
import { MdDelete } from "react-icons/md";
import { AiFillCloseCircle } from "react-icons/ai";
import { jwtDecode } from "jwt-decode";

const MenuManagement = () => {
  const [menuItems, setMenuItems] = useState([]);
  const [filteredItems, setFilteredItems] = useState([]);
  const [view, setView] = useState("viewAll");
  const [jwtToken, setJwtToken] = useState(localStorage.getItem('token'));
  const [userID, setUserID] = useState(null);
  const [newMenuItem, setNewMenuItem] = useState({
    itemName: "",
    itemDescription: "",
    itemPrice: 0,
    ingredients: "",
    cuisineType: "",
    tasteInfo: "",
    availabilityStatus: "Available",
    dietaryInfo: "",
    imageURL: "",
    categoryID: null,
    rating: null
  });
  const [updatedMenuItem, setUpdatedMenuItem] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedUserID = decodedToken.UserId; 
        setUserID(decodedUserID);
      } catch (error) {
        console.error('Error decoding JWT:', error);
      }
    }
  }, [jwtToken]);

  const fetchMenuItems = async () => {
    try {
      const response = await axios.get("https://localhost:7263/api/Admin/items", {
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          accept: "text/plain",
        },
      });
      setMenuItems(response.data);
      setFilteredItems(response.data);
    } catch (error) {
      console.error("Error fetching menu items", error);
    }
  };

  const filterItems = (term) => {
    const filtered = menuItems.filter(item =>
      item.itemName.toLowerCase().includes(term.toLowerCase())
    );
    setFilteredItems(filtered);
  };

  const addMenuItem = async () => {
    try {
      await axios.post("https://localhost:7263/api/Admin/menuitems", newMenuItem, {
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          "Content-Type": "application/json",
        },
      });
      fetchMenuItems();
      setNewMenuItem({
        itemName: "",
        itemDescription: "",
        itemPrice: 0,
        ingredients: "",
        cuisineType: "",
        tasteInfo: "",
        availabilityStatus: "Available",
        dietaryInfo: "",
        imageURL: "",
        categoryID: null,
        rating: null
      });
      setView("viewAll");
    } catch (error) {
      console.error("Error adding menu item", error);
    }
  };

  const updateMenuItem = async () => {
    if (updatedMenuItem) {
      try {
        await axios.put(
          `https://localhost:7263/api/Admin/menuitems/${updatedMenuItem.itemID}`,
          updatedMenuItem,
          {
            headers: {
              Authorization: `Bearer ${jwtToken}`,
              "Content-Type": "application/json",
            },
          }
        );
        fetchMenuItems();
        setUpdatedMenuItem(null);
        setView("viewAll");
      } catch (error) {
        console.error("Error updating menu item", error);
      }
    }
  };

  const deleteMenuItem = async (itemID) => {
    try {
      await axios.delete(`https://localhost:7263/api/Admin/menuitems/${itemID}`, {
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          accept: "*/*",
        },
      });
      fetchMenuItems();
    } catch (error) {
      console.error("Error deleting menu item", error);
    }
  };

  const setEditMenuItem = (item) => {
    setUpdatedMenuItem(item);
    setView("edit");
  };

  useEffect(() => {
    fetchMenuItems();
  }, []);

  return (
    <div className="flex-1 p-6 bg-white font-sans">
      <h1 className="text-2xl font-bold mb-4 text-center text-black font-sans">
        Menu Management
      </h1>

      <div className="mb-4 flex justify-between">
        <input
          type="text"
          placeholder="Search menu items..."
          value={searchTerm}
          onChange={(e) => {
            setSearchTerm(e.target.value);
            filterItems(e.target.value);
          }}
          className="p-2 border rounded"
        />
        <button
          onClick={() => setView("add")}
          className="mb-2 rounded-xl bg-gray-600 hover:bg-black text-white px-4 py-2 transition duration-300 ease-in-out transform hover:scale-105"
        >
          <IoIosAddCircle size={15} />
        </button>
      </div>

      {/* Conditionally Show Content */}
      {view === "add" && (
        <div className="bg-white p-4 rounded-lg shadow-md transition-transform duration-300 transform hover:scale-105">
          <h2 className="text-lg font-bold mb-4">Add New Menu Item</h2>
          <form onSubmit={(e) => { e.preventDefault(); addMenuItem(); }}>
            <input
              type="text"
              placeholder="Item Name"
              value={newMenuItem.itemName}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, itemName: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <textarea
              placeholder="Description"
              value={newMenuItem.itemDescription}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, itemDescription: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="number"
              placeholder="Price"
              value={newMenuItem.itemPrice}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, itemPrice: parseFloat(e.target.value) })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="text"
              placeholder="Ingredients"
              value={newMenuItem.ingredients}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, ingredients: e.target.value})}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="text"
              placeholder="Cuisine"
              value={newMenuItem.cuisineType}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, cuisineType: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="text"
              placeholder="TasteInfo"
              value={newMenuItem.tasteInfo}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, tasteInfo: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="text"
              placeholder="DietaryInfo"
              value={newMenuItem.dietaryInfo}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, dietaryInfo: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="text"
              placeholder="AvailabilityStatus"
              value={newMenuItem.availabilityStatus}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, availabilityStatus: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="number"
              placeholder="Rating"
              value={newMenuItem.rating}
              onChange={(e) => setNewMenuItem({ ...newMenuItem, rating: parseFloat(e.target.value) })}
              className="p-2 border rounded w-full mb-2"
              required
            />
           
            <div className="flex justify-between mt-4">
              <button
                type="submit"
                className="bg-white text-black px-4 py-2 rounded-3xl hover:bg-gray-200 transition duration-300 ease-in-out"
              >
                <FaSave />
              </button>
              <button
                type="button"
                onClick={() => setView("viewAll")}
                className="bg-white text-black px-4 py-2 rounded-3xl hover:bg-gray-200 transition duration-300 ease-in-out"
              >
                <AiFillCloseCircle />
              </button>
            </div>
          </form>
        </div>
      )}

      {view === "edit" && updatedMenuItem && (
        <div className="bg-white p-4 rounded-lg shadow-md transition-transform duration-300 transform hover:scale-105">
          <h2 className="text-lg font-bold mb-4">Edit Menu Item</h2>
          <form onSubmit={(e) => { e.preventDefault(); updateMenuItem(); }}>
            <input
              type="text"
              placeholder="Item Name"
              value={updatedMenuItem.itemName}
              onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, itemName: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <textarea
              placeholder="Description"
              value={updatedMenuItem.itemDescription}
              onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, itemDescription: e.target.value })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
              type="number"
              placeholder="Price"
              value={updatedMenuItem.itemPrice}
              onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, itemPrice: parseFloat(e.target.value) })}
              className="p-2 border rounded w-full mb-2"
              required
            />
            <input
  type="text"
  placeholder="Ingredients (comma separated)"
  value={updatedMenuItem.ingredients}
  onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, ingredients: e.target.value })}
  className="p-2 border rounded w-full mb-2"
  required
/>

<input
  type="text"
  placeholder="Cuisine Type"
  value={updatedMenuItem.cuisineType}
  onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, cuisineType: e.target.value })}
  className="p-2 border rounded w-full mb-2"
  required
/>

<input
  type="text"
  placeholder="Taste Info"
  value={updatedMenuItem.tasteInfo}
  onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, tasteInfo: e.target.value })}
  className="p-2 border rounded w-full mb-2"
/>

<select
  value={updatedMenuItem.availabilityStatus}
  onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, availabilityStatus: e.target.value })}
  className="p-2 border rounded w-full mb-2"
>
  <option value="Available">Available</option>
  <option value="Unavailable">Unavailable</option>
</select>

<input
  type="text"
  placeholder="Dietary Info (e.g., Vegetarian, Gluten-Free)"
  value={updatedMenuItem.dietaryInfo}
  onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, dietaryInfo: e.target.value })}
  className="p-2 border rounded w-full mb-2"
/>

<input
  type="text"
  placeholder="Image URL"
  value={updatedMenuItem.imageURL}
  onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, imageURL: e.target.value })}
  className="p-2 border rounded w-full mb-2"
/>

<input
  type="number"
  placeholder="Category ID"
  value={updatedMenuItem.categoryID}
  onChange={(e) => setUpdatedMenuItem({ ...updatedMenuItem, categoryID: parseInt(e.target.value) })}
  className="p-2 border rounded w-full mb-2"
/>

            
            <div className="flex justify-between mt-4">
              <button
                type="submit"
                className="bg-white text-black px-4 py-2 rounded-3xl hover:bg-gray-200 transition duration-300 ease-in-out"
              >
                <FaSave />
              </button>
              <button
                type="button"
                onClick={() => setView("viewAll")}
                className="bg-white text-black px-4 py-2 rounded-3xl hover:bg-gray-200 transition duration-300 ease-in-out"
              >
                <AiFillCloseCircle />
              </button>
            </div>
          </form>
        </div>
      )}

      {view === "viewAll" && (
        <div className="mt-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {filteredItems.map((item) => (
            <div key={item.itemID} className="p-4 border rounded-lg shadow-md bg-white transition-transform duration-300 hover:scale-105">
              <img
                src="/biriyani.jpg" // Common image link
                alt={item.itemName}
                className="mb-2 rounded"
              />
              <h3 className="font-bold">{item.itemName}</h3>
              <p>{item.itemDescription}</p>
              <p className="text-lg font-semibold text-green-600">{`$${item.itemPrice.toFixed(2)}`}</p>
              <div className="flex justify-between mt-2">
                <button
                  onClick={() => setEditMenuItem(item)}
                  className="bg-yellow-300 hover:bg-yellow-400 text-black px-2 py-1 rounded transition duration-300"
                >
                  <FaEdit />
                </button>
                <button
                  onClick={() => deleteMenuItem(item.itemID)}
                  className="bg-red-500 hover:bg-red-600 text-white px-2 py-1 rounded transition duration-300"
                >
                  <MdDelete />
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MenuManagement;
