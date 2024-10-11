import React, { useState, useEffect } from "react";
import axios from "axios";
import { IoIosPersonAdd } from "react-icons/io";
import { FaUserEdit, FaSave } from "react-icons/fa";
import { MdDelete } from "react-icons/md";
import { AiFillCloseCircle } from "react-icons/ai";
import { FaSort } from "react-icons/fa";
import { jwtDecode } from "jwt-decode";

const UserManagement = () => {
  const [users, setUsers] = useState([]);
  const [view, setView] = useState("viewAll");
  const [jwtToken, setJwtToken] = useState(localStorage.getItem("token"));
  const [userID, setUserID] = useState(null);
  const [newUser, setNewUser] = useState({
    userName: "",
    password: "",
    email: "",
    phoneNumber: "",
    alternativePhoneNumber: "",
    role: "",
    restaurantID: "",
  });
  const [updatedUser, setUpdatedUser] = useState(null);

  useEffect(() => {
    if (jwtToken) {
      try {
        const decodedToken = jwtDecode(jwtToken);
        const decodedUserID = decodedToken.UserId;
        console.log("Decoded User ID:", decodedUserID);
        setUserID(decodedUserID);
      } catch (error) {
        console.error("Error decoding JWT:", error);
      }
    }
  }, [jwtToken]);

  const fetchUsers = async () => {
    try {
      const response = await axios.get("https://localhost:7263/api/Admin/users", {
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          accept: "text/plain",
        },
      });
      setUsers(response.data);
    } catch (error) {
      console.error("Error fetching users", error);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const addUser = async () => {
    try {
      if (newUser.role === "Hotel Owner" && !newUser.restaurantID) {
        alert("The restaurant field is required when the role is 'Hotel Owner'.");
        return;
      }

      const payload = { ...newUser };
      if (newUser.role !== "Hotel Owner") {
        delete payload.restaurantID;
      }

      await axios.post("https://localhost:7263/api/Admin/users", payload, {
        headers: {
          Authorization: `Bearer ${jwtToken}`,
          "Content-Type": "application/json",
        },
      });

      fetchUsers();
      setNewUser({
        userName: "",
        password: "",
        email: "",
        phoneNumber: "",
        alternativePhoneNumber: "",
        role: "",
        restaurantID: null,
      });
      setView("viewAll");
    } catch (error) {
      console.error("Error adding user", error);
      if (error.response) {
        alert(error.response.data.title || "Error occurred while adding user.");
      } else {
        alert(error.message);
      }
    }
  };

  const updateUser = async () => {
    if (updatedUser) {
      try {
        if (!updatedUser.userName || !updatedUser.email || !updatedUser.phoneNumber) {
          alert("Username, Email, and Phone Number are required.");
          return;
        }

        await axios.put(
          `https://localhost:7263/api/Admin/users/${updatedUser.userID}`,
          updatedUser,
          {
            headers: {
              Authorization: `Bearer ${jwtToken}`,
              "Content-Type": "application/json",
            },
          }
        );

        fetchUsers();
        setUpdatedUser(null);
        setView("viewAll");
      } catch (error) {
        console.error("Error updating user", error);
        if (error.response) {
          alert(error.response.data.title || error.message);
        } else {
          alert(error.message);
        }
      }
    }
  };

  

  const setEditUser = (user) => {
    setUpdatedUser(user);
    setView("edit");
  };

  const sortUsersByRole = () => {
    const sortedUsers = [...users].sort((a, b) => {
      return a.role.localeCompare(b.role);
    });
    setUsers(sortedUsers);
  };

  return (
    <div className="flex-1 p-6 bg-white font-sans">
      <h1 className="text-2xl font-bold mb-4 text-center text-black font-sans">
        User Management
      </h1>
      <div className="flex flex-row mb-4">
        <button
          onClick={() => setView("add")}
          className="mb-2 btn bg-gray-600 hover:bg-black text-white px-4 py-2 rounded-md"
        >
          <IoIosPersonAdd size={20} />
        </button>
        <button
          onClick={sortUsersByRole}
          className="mb-2 rounded-xl btn bg-gray-600 hover:bg-black text-white ml-2 px-4 py-2"
        >
          <FaSort />
        </button>
      </div>

      {/* Add New User Form */}
      {view === "add" && (
        <div className="form-container bg-white p-6 rounded-lg shadow-lg mb-4">
          <h2 className="text-xl font-bold mb-6 text-center">Add New User</h2>
          <form>
            <input
              type="text"
              placeholder="Username"
              value={newUser.userName}
              onChange={(e) => setNewUser({ ...newUser, userName: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            />
            <input
              type="password"
              placeholder="Password"
              value={newUser.password}
              onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            />
            <input
              type="email"
              placeholder="Email"
              value={newUser.email}
              onChange={(e) => setNewUser({ ...newUser, email: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            />
            <input
              type="text"
              placeholder="Phone Number"
              value={newUser.phoneNumber}
              onChange={(e) => setNewUser({ ...newUser, phoneNumber: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            />
            <input
              type="text"
              placeholder="Alternative Phone Number"
              value={newUser.alternativePhoneNumber}
              onChange={(e) => setNewUser({ ...newUser, alternativePhoneNumber: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
            />
            <select
              value={newUser.role}
              onChange={(e) => setNewUser({ ...newUser, role: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            >
              <option value="">Select Role</option>
              <option value="Customer">Customer</option>
              <option value="Admin">Admin</option>
              <option value="Hotel Owner">Hotel Owner</option>
            </select>
            {newUser.role === "Hotel Owner" && (
              <input
                type="text"
                placeholder="Restaurant ID"
                value={newUser.restaurantID}
                onChange={(e) => setNewUser({ ...newUser, restaurantID: e.target.value })}
                className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
                required
              />
            )}
            <div className="flex justify-between">
              <button
                type="button"
                onClick={addUser}
                className="btn bg-white text-black px-4 py-2 rounded-md flex items-center"
              >
                <FaSave size={20} className="mr-2" /> 
              </button>
              <button
                onClick={() => setView("viewAll")}
                className="btn bg-red-600 text-white px-2 py-2 rounded-md flex items-center"
              >
                <AiFillCloseCircle size={20} className="mr-2" /> 
              </button>
            </div>
          </form>
        </div>
      )}

      {/* Edit User Form */}
      {view === "edit" && updatedUser && (
        <div className="form-container bg-white p-6 rounded-lg shadow-lg mb-4">
          <h2 className="text-xl font-bold mb-6 text-center">Edit User</h2>
          <form>
            <input
              type="text"
              placeholder="Username"
              value={updatedUser.userName}
              onChange={(e) => setUpdatedUser({ ...updatedUser, userName: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            />
            <input
              type="email"
              placeholder="Email"
              value={updatedUser.email}
              onChange={(e) => setUpdatedUser({ ...updatedUser, email: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            />
            <input
              type="text"
              placeholder="Phone Number"
              value={updatedUser.phoneNumber}
              onChange={(e) => setUpdatedUser({ ...updatedUser, phoneNumber: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            />
            <input
              type="text"
              placeholder="Alternative Phone Number"
              value={updatedUser.alternativePhoneNumber}
              onChange={(e) => setUpdatedUser({ ...updatedUser, alternativePhoneNumber: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
            />
            <select
              value={updatedUser.role}
              onChange={(e) => setUpdatedUser({ ...updatedUser, role: e.target.value })}
              className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
              required
            >
              <option value="Customer">Customer</option>
              <option value="Admin">Admin</option>
              <option value="Hotel Owner">Hotel Owner</option>
            </select>
            {updatedUser.role === "Hotel Owner" && (
              <input
                type="text"
                placeholder="Restaurant ID"
                value={updatedUser.restaurantID}
                onChange={(e) => setUpdatedUser({ ...updatedUser, restaurantID: e.target.value })}
                className="input-field border border-gray-300 rounded-md w-full p-2 mb-4"
                required
              />
            )}
            <div className="flex justify-between">
              <button
                type="button"
                onClick={updateUser}
                className="btn bg-green-600 text-white px-4 py-2 rounded-md flex items-center"
              >
                <FaSave className="mr-2" /> Update
              </button>
              <button
                onClick={() => {
                  setUpdatedUser(null);
                  setView("viewAll");
                }}
                className="btn bg-red-600 text-white px-4 py-2 rounded-md flex items-center"
              >
                <AiFillCloseCircle className="mr-2" /> Cancel
              </button>
            </div>
          </form>
        </div>
      )}

      {/* User List */}
      {view === "viewAll" && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {users.map((user) => (
            <div key={user.userID} className="user-card bg-gray-100 rounded-lg p-4 shadow-md">
              <h3 className="text-lg font-bold">{user.userName}</h3>
              <p>Email: {user.email}</p>
              <p>Phone: {user.phoneNumber}</p>
              <p>Role: {user.role}</p>
              <div className="flex justify-between mt-4">
                <button
                  onClick={() => setEditUser(user)}
                  className="btn bg-gray hover:bg-white text-black px-2 py-1 rounded-md flex items-center"
                >
                  <FaUserEdit className="mr-1" /> 
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default UserManagement;
