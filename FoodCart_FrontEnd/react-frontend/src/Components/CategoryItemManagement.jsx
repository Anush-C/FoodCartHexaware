import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faTimes, faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';

const CategoryItemManagement = () => {
  const [categories, setCategories] = useState([]);
  const [error, setError] = useState(null);
  const [restaurantId, setRestaurantId] = useState(null);
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [editCategoryId, setEditCategoryId] = useState(null);
  const [newCategory, setNewCategory] = useState({
    CategoryName: '', // Ensure this matches your API expected field names
    CategoryDescription: '', // Ensure this matches your API expected field names
  });

  const jwtToken = localStorage.getItem('token');

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedRestaurantId = decodedToken.RestaurantID; // Ensure RestaurantID exists in your token
        setRestaurantId(decodedRestaurantId);
      } catch (error) {
        console.error('Error decoding JWT:', error);
      }
    }
  }, [jwtToken]);

  useEffect(() => {
    const fetchCategories = async () => {
      if (!restaurantId) return;
      try {
        const response = await axios.get(`https://localhost:7263/api/Restaurant/categories/${restaurantId}`, {
          headers: {
            accept: '*/*',
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        setCategories(response.data);
      } catch (err) {
        setError('Error fetching categories');
        console.error(err);
      }
    };

    fetchCategories();
  }, [restaurantId, jwtToken]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setNewCategory(prevState => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleAddCategory = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        `https://localhost:7263/api/Restaurant/category?restaurantId=${restaurantId}`, 
        {
          categoryName: newCategory.CategoryName, // Changed to match API expected field names
          description: newCategory.CategoryDescription, // Changed to match API expected field names
        },
        {
          headers: {
            accept: '*/*',
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );
      setCategories(prevCategories => [...prevCategories, response.data]);
      setIsFormVisible(false);
      resetForm();
    } catch (err) {
      setError('Error adding category');
      console.error('Error adding category:', err);
    }
  };

  const handleEditCategory = (category) => {
    setIsEditMode(true);
    setEditCategoryId(category.categoryID);
    setNewCategory({
      CategoryName: category.categoryName, // Ensure this matches your API expected field names
      CategoryDescription: category.description, // Ensure this matches your API expected field names
    });
    setIsFormVisible(true);
  };

  const handleUpdateCategory = async (e) => {
    e.preventDefault();
    try {
      await axios.put(
        `https://localhost:7263/api/Restaurant/category/${editCategoryId}?restaurantId=${restaurantId}`, 
        {
          categoryName: newCategory.CategoryName, // Changed to match API expected field names
          description: newCategory.CategoryDescription, // Changed to match API expected field names
        },
        {
          headers: {
            accept: '*/*',
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );
      setCategories(prevCategories =>
        prevCategories.map(cat => (cat.categoryID === editCategoryId ? { ...cat, ...newCategory } : cat))
      );
      setIsFormVisible(false);
      resetForm();
      setIsEditMode(false);
    } catch (err) {
      setError('Error updating category');
      console.error('Error updating category:', err);
    }
  };

  const handleDeleteCategory = async (categoryId) => {
    try {
      await axios.delete(`https://localhost:7263/api/Restaurant/category/${categoryId}?restaurantId=${restaurantId}`, {
        headers: {
          accept: '*/*',
          Authorization: `Bearer ${jwtToken}`,
        },
      });
      setCategories(prevCategories => prevCategories.filter(cat => cat.categoryID !== categoryId));
    } catch (err) {
      setError('Error deleting category');
      console.error('Error deleting category:', err);
    }
  };

  const resetForm = () => {
    setNewCategory({
      CategoryName: '', // Reset to match API expected field names
      CategoryDescription: '', // Reset to match API expected field names
    });
    setEditCategoryId(null);
  };

  return (
    <div className="rounded-lg p-4">
      <h2 className="text-xl font-bold mb-4 font-sans">All Categories</h2>
      {error && <p className="text-red-600">{error}</p>}

      <button
        onClick={() => {
          resetForm();
          setIsEditMode(false);
          setIsFormVisible(true);
        }}
        className="mb-4 p-2 bg-white text-black hover:bg-white hover:text-orange-600 rounded flex items-center font-bold"
      >
        <FontAwesomeIcon icon={faPlus} className="mr-2" /> NEW
      </button>

      {isFormVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center z-50">
          <div className="bg-white rounded-sm shadow-lg p-6 max-w-sm w-full max-h-[90%] overflow-y-auto relative">
            <h3 className="font-bold text-xl text-center mb-4">
              {isEditMode ? 'Edit Category' : 'Add Category'}
            </h3>
            <button
              onClick={() => setIsFormVisible(false)}
              className="absolute top-2 right-2 text-gray-700 bg-white mt-2 hover:bg-white"
            >
              <FontAwesomeIcon icon={faTimes} />
            </button>
            <form onSubmit={isEditMode ? handleUpdateCategory : handleAddCategory} className="mt-4 space-y-4">
              <input
                type="text"
                name="CategoryName"
                value={newCategory.CategoryName}
                onChange={handleChange}
                placeholder="Name"
                required
                className="border border-gray-300 rounded-xl p-3 w-full focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
              />
              <textarea
                name="CategoryDescription"
                value={newCategory.CategoryDescription}
                onChange={handleChange}
                placeholder="Description"
                required
                className="border border-gray-300 rounded-xl p-3 w-full h-24 focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
              />
              <button
                type="submit"
                className="bg-green-400 text-white rounded-xl p-3 w-full hover:bg-blue-700 transition"
              >
                {isEditMode ? 'Update' : 'Add'}
              </button>
            </form>
          </div>
        </div>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {categories.map((category, index) => (
          <div 
            key={category.categoryID}
            className="border rounded-xl p-4 bg-white shadow-lg"
            style={{ transitionDelay: `${index * 100}ms` }}
          >
            <h3 className="text-lg font-semibold mt-2 font-sans">{category.categoryName}</h3>
            <p className="text-sm text-gray-600 mt-2 font-sans">{category.description}</p>

            <div className="flex mt-4">
              <button 
                onClick={() => handleEditCategory(category)} 
                className="p-2 bg-white text-black rounded hover:bg-white"
              >
                <FontAwesomeIcon icon={faEdit} />
              </button>
              <button 
                onClick={() => handleDeleteCategory(category.categoryID)} 
                className="p-2 bg-white text-red-600 rounded hover:bg-white ml-2"
              >
                <FontAwesomeIcon icon={faTrash} />
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default CategoryItemManagement;
