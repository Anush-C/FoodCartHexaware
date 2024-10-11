import React, { useState } from "react";
import { IoIosAddCircle } from "react-icons/io";
import { RxCross2 } from "react-icons/rx"; // Import the RxCross2 icon

const Form = () => {
  const [showDialog, setShowDialog] = useState(false);
  const [formData, setFormData] = useState({
    doorNo: "",
    area: "",
    landmark: "",
    pincode: "",
    phone: "",
  });
  const [fullAddress, setFullAddress] = useState("");

  // Function to open the dialog
  const openDialog = () => {
    setShowDialog(true);
  };

  // Function to close the dialog and update the full address
  const handleDone = () => {
    const address = `${formData.doorNo}, ${formData.area}, ${formData.landmark}, Pincode: ${formData.pincode}, Phone: ${formData.phone}`;
    setFullAddress(address);
    
    // Clear form fields after saving the address
    setFormData({
      doorNo: "",
      area: "",
      landmark: "",
      pincode: "",
      phone: "",
    });
    
    setShowDialog(false);
  };

  // Function to handle the input change
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  // Function to edit the address
  const handleEdit = () => {
    const [doorNo, area, landmark, pincode, phone] = fullAddress.split(", ");
    setFormData({
      doorNo: doorNo.split(": ")[0],
      area: area,
      landmark: landmark,
      pincode: pincode.split(": ")[1],
      phone: phone.split(": ")[1],
    });
    setShowDialog(true);
  };

  return (
    <div className="p-4 border rounded-sm shadow-md mx-auto mt-6 w-200 bg-gray-100 relative">
      {/* Header with Add Icon */}
      <div className="flex items-center justify-between mb-4">
        <div className="font-semibold text-xl text-gray-700">
          Add a delivery address
        </div>
        <div
          className="text-gray-600 text-3xl cursor-pointer hover:text-black"
          onClick={openDialog}
        >
          <IoIosAddCircle />
        </div>
      </div>

      {/* Display Full Address */}
      {fullAddress && (
        <div className="mb-6">
          <label className="block mb-2 font-semibold text-orange-600 text-lg">
            Shipping Address:
          </label>
          <input
            type="text"
            value={fullAddress}
            className="w-full bg-orange-50 border-2 border-orange-300 rounded-2xl p-4 shadow-xl transition duration-300 ease-in-out transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-500 text-gray-700 text-lg font-medium tracking-wide"
            readOnly
          />
          <button
            onClick={handleEdit}
            className="mt-2 text-indigo-500 hover:text-indigo-700 font-semibold"
          >
            Edit Address
          </button>
        </div>
      )}

      {/* Dialog Box for Form */}
      {showDialog && (
        <div className="fixed inset-0 bg-gray-900 bg-opacity-80 flex justify-center items-center">
          <div className="bg-white p-8 rounded-lg shadow-2xl w-80 relative">
            <RxCross2
              className="absolute top-4 right-4 text-gray-600 cursor-pointer hover:text-black"
              onClick={() => setShowDialog(false)}
            />
            <div className="font-bold text-xl text-gray-800 mb-6">Enter Your Address</div>

            {/* Input Fields */}
            {["doorNo", "area", "landmark", "pincode", "phone"].map((field) => (
              <input
                key={field}
                type="text"
                name={field}
                placeholder={field.replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase())} // Format field name
                value={formData[field]}
                onChange={handleChange}
                className="w-full border-2 border-gray-300 rounded-lg p-3 mb-4 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 text-gray-700 transition duration-200 ease-in-out transform hover:scale-105"
              />
            ))}

            <div className="flex justify-end">
              <button
                onClick={handleDone}
                className="bg-indigo-500 text-white px-5 py-3 rounded-lg font-semibold shadow-md transition duration-200 ease-in-out transform hover:scale-110 hover:bg-indigo-600 focus:ring-2 focus:ring-indigo-400"
              >
                Save Address
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Form;
