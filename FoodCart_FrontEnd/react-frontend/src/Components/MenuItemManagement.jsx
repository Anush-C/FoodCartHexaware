import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faTimes, faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';

const MenuItemsList = () => {
  const [menuItems, setMenuItems] = useState([]);
  const [allMenuItems, setAllMenuItems] = useState([]);
  const [error, setError] = useState(null);
  const [restaurantId, setRestaurantId] = useState(null);
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [isLinkingExisting, setIsLinkingExisting] = useState(false);
  const [editItemId, setEditItemId] = useState(null);
  const [newItem, setNewItem] = useState({
    name: '',
    description: '',
    price: 0,
    ingredients: '',
    cuisineType: '',
    tasteInfo: '',
    availabilityStatus: '',
    dietaryInfo: '',
    imageURL: '', // Keep this for specific images
    categoryID: 0,
  });
  const [selectedExistingItemId, setSelectedExistingItemId] = useState('');

  const jwtToken = localStorage.getItem('token');
  const commonImageURL = 'https://example.com/common-image.jpg'; // Common image URL

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedRestaurantId = decodedToken.RestaurantID;
        setRestaurantId(decodedRestaurantId);
      } catch (error) {
        console.error('Error decoding JWT:', error);
      }
    }
  }, [jwtToken]);

  useEffect(() => {
    const fetchMenuItems = async () => {
      if (!restaurantId) return;
      try {
        const response = await axios.get(`https://localhost:7263/api/Restaurant/menuitems/${restaurantId}`, {
          headers: {
            accept: '*/*',
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        setMenuItems(response.data);
      } catch (err) {
        setError('Error fetching menu items');
        console.error(err);
      }
    };

    fetchMenuItems();
  }, [restaurantId, jwtToken]);

  useEffect(() => {
    const fetchAllMenuItems = async () => {
      try {
        const response = await axios.get('https://localhost:7263/api/Menus', {
          headers: {
            accept: '*/*',
            Authorization: `Bearer ${jwtToken}`,
          },
        });

        if (response.data.menuItem && Array.isArray(response.data.menuItem)) {
          setAllMenuItems(response.data.menuItem);
        } else {
          setAllMenuItems([]);
          console.error('Unexpected response format:', response.data);
        }
      } catch (err) {
        console.error('Error fetching all menu items:', err);
        setError('Unable to fetch menu items');
      }
    };

    fetchAllMenuItems();
  }, [jwtToken]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setNewItem(prevState => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleAddItem = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(`https://localhost:7263/api/Restaurant/menuitem`, {
        ...newItem,
        imageURL: newItem.imageURL || commonImageURL // Use common image if no specific image is provided
      }, {
        headers: {
          accept: '*/*',
          Authorization: `Bearer ${jwtToken}`,
        },
      });
      setMenuItems(prevItems => [...prevItems, response.data]);
      setIsFormVisible(false);
      resetForm();
    } catch (err) {
      console.error('Error adding item:', err);
    }
  };

  const handleEditItem = (item) => {
    setIsEditMode(true);
    setEditItemId(item.itemID);
    setNewItem({
      name: item.name,
      description: item.description,
      price: item.price,
      ingredients: item.ingredients,
      cuisineType: item.cuisineType,
      tasteInfo: item.tasteInfo,
      availabilityStatus: item.availabilityStatus,
      dietaryInfo: item.dietaryInfo,
      imageURL: item.imageURL || commonImageURL, // Use common image if none is provided
      categoryID: item.categoryID,
    });
    setIsFormVisible(true);
  };

  const handleUpdateItem = async (e) => {
    e.preventDefault();
    try {
      await axios.put(`https://localhost:7263/api/Restaurant/menuitem/${editItemId}`, {
        ...newItem,
        imageURL: newItem.imageURL || commonImageURL // Use common image if no specific image is provided
      }, {
        headers: {
          accept: '*/*',
          Authorization: `Bearer ${jwtToken}`,
        },
      });
      setMenuItems(prevItems =>
        prevItems.map(item => (item.itemID === editItemId ? { ...item, ...newItem } : item))
      );
      setIsFormVisible(false);
      resetForm();
      setIsEditMode(false);
    } catch (err) {
      console.error('Error updating item:', err);
    }
  };

  const handleLinkExistingItem = async (e) => {
    e.preventDefault();
    
    if (selectedExistingItemId && restaurantId) {
      try {
        await axios.post(
          `https://localhost:7263/api/Menus/${restaurantId}/menuitem/${selectedExistingItemId}`, 
          {}, 
          {
            headers: {
              accept: '*/*',
              Authorization: `Bearer ${jwtToken}`,
            },
          }
        );
        setIsFormVisible(false);
        setSelectedExistingItemId('');
      } catch (err) {
        console.error('Error linking existing item:', err);
      }
    }
  };

  const handleDeleteItem = async (itemId) => {
    try {
      await axios.delete(`https://localhost:7263/api/Restaurant/menuitem/${itemId}`, {
        headers: {
          accept: '*/*',
          Authorization: `Bearer ${jwtToken}`,
        },
      });
      setMenuItems(prevItems => prevItems.filter(item => item.itemID !== itemId));
    } catch (err) {
      console.error('Error deleting item:', err);
    }
  };

  const resetForm = () => {
    setNewItem({
      name: '',
      description: '',
      price: 0,
      ingredients: '',
      cuisineType: '',
      tasteInfo: '',
      availabilityStatus: '',
      dietaryInfo: '',
      imageURL: '', // Reset to empty for new entries
      categoryID: 0,
    });
    setEditItemId(null);
    setSelectedExistingItemId('');
  };

  return (
    <div className="rounded-lg p-4 font-sans">
      <h2 className="text-xl font-bold mb-4 font-sans">Menu Items</h2>
      {error && <p className="text-red-600">{error}</p>}
      
      <button 
        onClick={() => {
          resetForm();
          setIsEditMode(false);
          setIsLinkingExisting(false); 
          setIsFormVisible(true);
        }} 
        className="mb-4 p-2 bg-white text-black hover:bg-white hover:text-orange-600 rounded flex items-center font-sans font-bold"
      >
        <FontAwesomeIcon icon={faPlus} className="mr-2" /> NEW
      </button>

      <button 
        onClick={() => {
          setIsFormVisible(true);
          setIsLinkingExisting(true); 
        }}
        className="mb-4 p-2 bg-white text-black hover:bg-white hover:text-orange-600 rounded flex items-center font-sans font-bold"
      >
        Link Existing
      </button>

      {isFormVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center z-50">
          <div className="bg-white rounded-sm shadow-lg p-6 max-w-sm w-full max-h-[90%] overflow-y-auto relative">
            <h3 className="font-bold text-xl text-center mb-4 font-sans">
              {isLinkingExisting ? 'Link Existing Menu Item' : (isEditMode ? 'Edit Menu Item' : 'Add Menu Item')}
            </h3>
            <button
              onClick={() => setIsFormVisible(false)}
              className="absolute top-2 right-2 text-gray-700 bg-white mt-2 hover:bg-white"
            >
              <FontAwesomeIcon icon={faTimes} />
            </button>

            {isLinkingExisting ? (
              <form onSubmit={handleLinkExistingItem} className="mt-4 space-y-4">
                <select 
                  value={selectedExistingItemId} 
                  onChange={(e) => setSelectedExistingItemId(e.target.value)} 
                  className="w-full p-2 border rounded"
                >
                  <option value="">Select Existing Item</option>
                  {allMenuItems.map(item => (
                    <option key={item.itemID} value={item.itemID}>
                      {item.itemName}
                    </option>
                  ))}
                </select>
                <button type="submit" className="w-full bg-blue-500 text-white p-2 rounded">
                  Link Item
                </button>
              </form>
            ) : (
              <form onSubmit={isEditMode ? handleUpdateItem : handleAddItem} className="mt-4 space-y-4">
                <input 
                  type="text" 
                  name="name" 
                  value={newItem.name} 
                  onChange={handleChange} 
                  placeholder="Name" 
                  className="w-full p-2 border rounded" 
                  required 
                />
                <input 
                  type="text" 
                  name="description" 
                  value={newItem.description} 
                  onChange={handleChange} 
                  placeholder="Description" 
                  className="w-full p-2 border rounded" 
                  required 
                />
                <input 
                  type="number" 
                  name="price" 
                  value={newItem.price} 
                  onChange={handleChange} 
                  placeholder="Price" 
                  className="w-full p-2 border rounded" 
                  required 
                />
                <input 
                  type="text" 
                  name="ingredients" 
                  value={newItem.ingredients} 
                  onChange={handleChange} 
                  placeholder="Ingredients" 
                  className="w-full p-2 border rounded" 
                />
                <input 
                  type="text" 
                  name="cuisineType" 
                  value={newItem.cuisineType} 
                  onChange={handleChange} 
                  placeholder="Cuisine Type" 
                  className="w-full p-2 border rounded" 
                />
                <input 
                  type="text" 
                  name="tasteInfo" 
                  value={newItem.tasteInfo} 
                  onChange={handleChange} 
                  placeholder="Taste Info" 
                  className="w-full p-2 border rounded" 
                />
                <input 
                  type="text" 
                  name="availabilityStatus" 
                  value={newItem.availabilityStatus} 
                  onChange={handleChange} 
                  placeholder="Availability Status" 
                  className="w-full p-2 border rounded" 
                />
                <input 
                  type="text" 
                  name="dietaryInfo" 
                  value={newItem.dietaryInfo} 
                  onChange={handleChange} 
                  placeholder="Dietary Info" 
                  className="w-full p-2 border rounded" 
                />
                <input 
                  type="text" 
                  name="imageURL" 
                  value={newItem.imageURL} 
                  onChange={handleChange} 
                  placeholder="Image URL" 
                  className="w-full p-2 border rounded" 
                />
                <input
                  type='number'
                  name="categoryID" 
                  value={newItem.categoryID} 
                  onChange={handleChange} 
                  placeholder='CategoryID'
                  className="w-full p-2 border rounded"
                />
                <button type="submit" className="w-full bg-blue-500 text-white p-2 rounded">
                  {isEditMode ? 'Update Item' : 'Add Item'}
                </button>
              </form>
            )}
          </div>
        </div>
      )}

      <div className="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {menuItems.map(item => (
          <div key={item.itemID} className="border rounded p-4">
            <img 
              src='/turkey.jpg' 
              alt={item.name} 
              className="w-full h-52 object-cover mb-2" 
            />
            <h3 className="font-bold">{item.name}</h3>
            <p className='mt-3 italic'>{item.description}</p>
            <p className= "text-green-500 font-bold">Price: ${item.price}</p>
            <p className='mt-1'><strong>Ingredients: </strong>{item.ingredients}</p>
            <p className='mt-1'><strong>Cuisine: </strong>{item.cuisineType}</p>
            <p className='mt-1'><strong>DietaryInfo: </strong>{item.dietaryInfo}</p>
            <p className='mt-1'><strong>TasteInfo: </strong>{item.tasteInfo}</p>


            <div className="flex justify-between mt-2">
              <button onClick={() => handleEditItem(item)} className="text-blue-500 bg-white hover:bg-white p-2">
                <FontAwesomeIcon icon={faEdit} />
              </button>
              <button onClick={() => handleDeleteItem(item.itemID)} className="text-red-500  bg-white hover:bg-white p-2">
                <FontAwesomeIcon icon={faTrash} />
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default MenuItemsList;
