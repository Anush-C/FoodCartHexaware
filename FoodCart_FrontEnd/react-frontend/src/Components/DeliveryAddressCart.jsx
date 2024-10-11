import React, { useState, useEffect } from "react";
import { IoIosAddCircle } from "react-icons/io";
import { TbEdit } from "react-icons/tb";
import { MdDelete } from "react-icons/md";
import { jwtDecode } from "jwt-decode"; // Ensure correct import

const DeliveryAddressCart = ({ onSelectAddress }) => {
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    doorNo: "",
    area: "",
    landmark: "",
    pincode: "",
    phone: "",
  });
  const [addresses, setAddresses] = useState([]);
  const [selectedAddress, setSelectedAddress] = useState("");
  const [isEditing, setIsEditing] = useState(false);
  const [editIndex, setEditIndex] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // Load addresses and selected address from localStorage on component mount
  useEffect(() => {
    const storedAddresses = JSON.parse(localStorage.getItem("addresses")) || [];
    const storedSelectedAddress = localStorage.getItem("selectedAddress") || "";
    setAddresses(storedAddresses);
    setSelectedAddress(storedSelectedAddress);

    // Check if the user has a valid JWT token
    const token = localStorage.getItem("token");
    if (token) {
      try {
        jwtDecode(token); // Check if the token can be decoded
        setIsAuthenticated(true); // User is authenticated if token is valid
      } catch (error) {
        console.error("Invalid token:", error);
        setIsAuthenticated(false);
      }
    }
  }, []);

  const openModal = () => {
    setShowModal(true);
    setFormData({
      doorNo: "",
      area: "",
      landmark: "",
      pincode: "",
      phone: "",
    });
    setIsEditing(false);
    setEditIndex(null);
  };

  const handleSaveAddress = () => {
    const { doorNo, area, landmark, pincode, phone } = formData;

    // Validate that all fields are filled
    if (!doorNo || !area || !landmark || !pincode || !phone) {
      alert("Please fill all the fields.");
      return;
    }

    const address = `${doorNo}, ${area}, ${landmark}, Pincode: ${pincode}, Phone: ${phone}`;
    if (isEditing) {
      const updatedAddresses = addresses.map((addr, index) =>
        index === editIndex ? address : addr
      );
      setAddresses(updatedAddresses);
      localStorage.setItem("addresses", JSON.stringify(updatedAddresses));
    } else {
      const newAddresses = [...addresses, address];
      setAddresses(newAddresses);
      localStorage.setItem("addresses", JSON.stringify(newAddresses));
    }
    setShowModal(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleEditAddress = (index) => {
    setIsEditing(true);
    setEditIndex(index);
    const addressParts = addresses[index].split(", ");
    setFormData({
      doorNo: addressParts[0],
      area: addressParts[1],
      landmark: addressParts[2],
      pincode: addressParts[3].split(": ")[1],
      phone: addressParts[4].split(": ")[1],
    });
    openModal();
  };

  const handleDeleteAddress = (index) => {
    const confirmDelete = window.confirm("Are you sure you want to delete this address?");
    if (confirmDelete) {
      const updatedAddresses = addresses.filter((_, i) => i !== index);
      setAddresses(updatedAddresses);
      localStorage.setItem("addresses", JSON.stringify(updatedAddresses));

      // Clear the selected address if the deleted address was selected
      if (selectedAddress === addresses[index]) {
        setSelectedAddress(""); // Clear selected address
        localStorage.removeItem("selectedAddress");
      }
    }
  };

  const handleSelectAddress = (address) => {
    setSelectedAddress(address);
    localStorage.setItem("selectedAddress", address);

    // Call the onSelectAddress prop to notify the parent component
    if (onSelectAddress) {
      onSelectAddress(address);
    }
  };

  return (
    <div className="px-6 py-6 border rounded-sm shadow-md  bg-gray-100">
      <div className="flex items-center justify-between mb-4 w-96">
        <div className="font-semibold text-xl text-gray-700 font-sans flex">Add a delivery address</div>
        <div
          className={`text-gray-600 cursor-pointer hover:text-black ${isAuthenticated ? '' : 'opacity-50 cursor-not-allowed'}`}
          onClick={isAuthenticated ? openModal : null} // Disable click if not authenticated
        >
          <IoIosAddCircle size={25}/>
        </div>
      </div>

      {/* Display Addresses */}
      {addresses.length > 0 ? (
        addresses.map((address, index) => (
          <div key={index} className={`mb-2 ${selectedAddress === address ? 'selected-address' : ''}`}>
            <div className="flex justify-between">
              <input
                type="radio"
                name="selectedAddress"
                checked={selectedAddress === address}
                onChange={() => handleSelectAddress(address)}
              />
              <div className="mt-3 w-96 ml-2">
              <input
                type="text"
                value={address}
                className="w-full bg-orange-50 border-2 border-orange-300 rounded-2xl p-4 shadow-xl transition duration-300 ease-in-out transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-500 text-gray-700 text-lg font-medium tracking-wide"
                readOnly
              />
              </div>
              <button
                onClick={() => handleEditAddress(index)}
                className="mt-8 mb-8 rounded-sm ml-2 bg-gray-100 hover:text-black text-black hover:bg-white p-2"
              >
                <TbEdit />
              </button>
              <button
                onClick={() => handleDeleteAddress(index)}
                className="mt-8 mb-8 rounded-sm ml-2 bg-gray-100 hover:text-black text-black hover:bg-white p-2"
              >
                <MdDelete />
              </button>
            </div>
          </div>
        ))
      ) : (
        <div className="font-bold text-xs opacity-20">NO ADDRESS AVAILABLE. ADD NEW TO GRAB YOUR ORDER</div>
      )}

      {/* Selected Address for Delivery */}
      {selectedAddress && (
        <div className="mt-4">
          <h2 className="font-semibold text-lg font-sans">Selected Address for Delivery:</h2>
          <p className="mt-2 font-sans">{selectedAddress}</p>
        </div>
      )}

      {/* Modal (Form) */}
      {showModal && (
        <div className="fixed inset-0 bg-gray-900 bg-opacity-60 flex justify-center items-center transition-opacity duration-300 ease-in-out animate-fadeIn">
          <div className="bg-white p-8 rounded-xl shadow-2xl max-w-md w-full transform transition-transform duration-300 ease-in-out animate-slideIn">
            <div className="font-bold text-xl text-gray-800 mb-6">
              {isEditing ? "Edit Your Address" : "Enter Your Address"}
            </div>

            {/* Input Fields */}
            {["doorNo", "area", "landmark", "pincode", "phone"].map((field) => (
              <input
                key={field}
                type="text"
                name={field}
                placeholder={field.replace(/([A-Z])/g, " $1").replace(/^./, (str) => str.toUpperCase())} // Format field name
                value={formData[field]}
                onChange={handleChange}
                className="w-full border-2 border-gray-300 rounded-lg p-3 mb-4 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 text-gray-700 transition duration-200 ease-in-out transform hover:scale-105"
              />
            ))}

            <div className="flex justify-end space-x-2">
              <button
                className="bg-red-500 text-white px-4 py-2 rounded-lg"
                onClick={() => setShowModal(false)}
              >
                Cancel
              </button>
              <button
                className="bg-green-500 text-white px-4 py-2 rounded-lg"
                onClick={handleSaveAddress}
              >
                {isEditing ? "Update" : "Save"}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default DeliveryAddressCart;
