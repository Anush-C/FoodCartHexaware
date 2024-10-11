import React, { useEffect, useState } from "react";
import { FaEdit, FaSave } from "react-icons/fa";
import { MdDelete } from "react-icons/md";
import { AiFillCloseCircle } from "react-icons/ai";
import { BsFillHouseAddFill } from "react-icons/bs";
import { jwtDecode } from "jwt-decode";

const CategoryManagement = () => {
  const [categories, setCategories] = useState([]);
  const [filteredCategories, setFilteredCategories] = useState([]);
  const [view, setView] = useState("viewAll");
  const [jwtToken, setJwtToken] = useState(localStorage.getItem('token'));
  const [userID, setUserID] = useState(null);
  const [newCategory, setNewCategory] = useState({
    categoryName: "",
    categoryDescription: "",
  });
  const [updatedCategory, setUpdatedCategory] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");

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
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    const response = await fetch("https://localhost:7263/api/Admin/AllCategories", {
      headers: {
        Authorization: `Bearer ${jwtToken}`, 
        "Accept": "application/json",
      },
    });
    const data = await response.json();
    setCategories(data);
    setFilteredCategories(data); // Set initial filtered categories
  };

  const addCategory = async () => {
    await fetch("https://localhost:7263/api/Admin/categories", {
      method: "POST",
      headers: {
        Authorization: `Bearer ${jwtToken}`, 
        "Content-Type": "application/json",
      },
      body: JSON.stringify(newCategory),
    });
    fetchCategories();
    setNewCategory({ categoryName: "", categoryDescription: "" });
    setView("viewAll");
  };

  const updateCategory = async () => {
    await fetch(`https://localhost:7263/api/Admin/categories/${updatedCategory.categoryID}`, {
      method: "PUT",
      headers: {
        Authorization: `Bearer ${jwtToken}`, 
        "Content-Type": "application/json",
      },
      body: JSON.stringify(updatedCategory),
    });
    fetchCategories();
    setUpdatedCategory(null);
    setView("viewAll");
  };

  const deleteCategory = async (id) => {
    await fetch(`https://localhost:7263/api/Admin/categories/${id}`, {
      method: "DELETE",
      headers:{
        Authorization: `Bearer ${jwtToken}`, 
        accept: "*/*",
      }
    });
    fetchCategories();
  };

  // Handle Search Filter
  const handleSearch = (event) => {
    const value = event.target.value.toLowerCase();
    setSearchTerm(value);
    const filtered = categories.filter(category =>
      category.categoryName.toLowerCase().includes(value) || 
      category.categoryDescription.toLowerCase().includes(value)
    );
    setFilteredCategories(filtered);
  };

  return (
    <div className="container mx-auto p-4 font-sans">
      <h2 className="text-2xl font-bold mb-6 font-sans text-black">Category Management</h2>

      {view === "add" && (
        <div className="mb-4 p-4 bg-gray-100 rounded-lg shadow">
          <h3 className="text-xl font-bold mb-2 font-sans">Add Category</h3>
          <input
            type="text"
            placeholder="Category Name"
            value={newCategory.categoryName}
            onChange={(e) => setNewCategory({ ...newCategory, categoryName: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Description"
            value={newCategory.categoryDescription}
            onChange={(e) => setNewCategory({ ...newCategory, categoryDescription: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <div className="flex justify-between mt-4">
            <button onClick={addCategory} className="bg-black text-white px-4 py-2 rounded-lg hover:bg-green-600 transition duration-300 transform hover:scale-105">
              <FaSave />
            </button>
            <button onClick={() => setView("viewAll")} className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition duration-300 transform hover:scale-105">
              <AiFillCloseCircle className="rounded-md" />
            </button>
          </div>
        </div>
      )}

      {view === "edit" && updatedCategory && (
        <div className="mb-4 p-4 bg-gray-100 rounded-lg shadow">
          <h3 className="text-xl font-bold mb-2">Edit Category</h3>
          <input
            type="text"
            placeholder="Category Name"
            value={updatedCategory.categoryName}
            onChange={(e) => setUpdatedCategory({ ...updatedCategory, categoryName: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <input
            type="text"
            placeholder="Description"
            value={updatedCategory.categoryDescription}
            onChange={(e) => setUpdatedCategory({ ...updatedCategory, categoryDescription: e.target.value })}
            className="block w-full p-2 border border-gray-300 mb-3 rounded"
          />
          <div className="flex justify-between mt-4">
            <button onClick={updateCategory} className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition duration-300 transform hover:scale-105">
              <FaSave className="inline" />
            </button>
            <button onClick={() => setView("viewAll")} className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition duration-300 transform hover:scale-105">
              <AiFillCloseCircle className="inline" />
            </button>
          </div>
        </div>
      )}

      {view === "viewAll" && (
        <div className="mt-4">
          <button onClick={() => setView("add")} className="mb-4 bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600 transition duration-300 transform hover:scale-105">
            <BsFillHouseAddFill />
          </button>
          <input
            type="text"
            placeholder="Search categories..."
            value={searchTerm}
            onChange={handleSearch}
            className="block w-full p-2 border border-gray-300 mb-4 rounded"
          />
          {filteredCategories.length === 0 ? (
            <p className="text-gray-500">No categories available</p>
          ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
              {filteredCategories.map((category) => (
                <div key={category.categoryID} className="bg-white shadow-md rounded-lg p-4 hover:shadow-lg transition-transform duration-500 ease-in-out transform hover:scale-105">
                  <img src="/category.jpeg" alt="Category" className="w-full h-50 object-cover mb-4 rounded-lg" />
                  <h4 className="text-lg font-semibold">{category.categoryName}</h4>
                  <p className="text-gray-700">{category.categoryDescription}</p>
                  <div className="flex justify-between mt-4">
                    <button
                      onClick={() => {
                        setUpdatedCategory(category);
                        setView("edit");
                      }}
                      className="text-black bg-white border-none p-2 rounded hover:bg-gray-200 transition duration-300 transform hover:scale-105"
                    >
                      <FaEdit size={15} />
                    </button>
                    <button
                      onClick={() => deleteCategory(category.categoryID)}
                      className="text-black bg-white border-none p-2 rounded hover:bg-gray-200 transition duration-300 transform hover:scale-105"
                    >
                      <MdDelete />
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default CategoryManagement;
