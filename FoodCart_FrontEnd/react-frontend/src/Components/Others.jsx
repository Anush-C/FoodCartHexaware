import React, { useState } from 'react';
import { IoIosClose } from "react-icons/io";
import { FaSearch } from "react-icons/fa";

const Others = ({ setIsOpen }) => { // Accept setIsOpen as a prop
  const [location, setLocation] = useState("");

  const handleInputChange = (e) => {
    setLocation(e.target.value);
  };

  const handleSearch = async () => {
    try {
      const response = await fetch(`https://localhost:7263/api/Menus/restaurants?address=${123}`, {
        method: 'GET',
        headers: {
          'Accept': '*/*'
        }
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const data = await response.json();
      console.log(data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  return (
    <div>
      <div className='text-gray-400 mt-8'>
        <IoIosClose size={40} onClick={() => setIsOpen(false)} /> {/* Close modal */}
      </div>
      <div className='z-50 w-[400px] flex justify-center items-center mx-4 mt-4 bg-white h-12 border-black drop-shadow-md shadow-sm rounded-sm p-2'>
        <input
          type="text"
          placeholder="Search for area, street name..."
          className="w-full h-full bg-transparent border-none outline-none p-2 font-sans" 
          value={location}
          onChange={handleInputChange}
        />
        <button onClick={handleSearch} className="ml-2 bg-blue-500 text-white rounded p-2"><FaSearch /></button>
      </div>
    </div>
  );
}

export default Others;
