import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { IoBicycleOutline } from "react-icons/io5";
import { MdEmail, MdLocationPin } from "react-icons/md";
import { FaPhone, FaSearch } from 'react-icons/fa';

const RestaurantDetails = () => {
  const location = useLocation();
  const { restaurant } = location.state || {}; // Get the restaurant from the state
  const navigate = useNavigate();
  const handleNavigateSearch = ()=>{
    navigate("/search")
  }

  // Logging restaurant details
  console.log("Received restaurant details:", restaurant);

  if (!restaurant) return <p>No restaurant details available.</p>;

  return (
    <div className="mx-64 mt-16">
      <div className="text-2xl text-gray-700 font-semibold mb-8 text-left">{restaurant.name}</div>
    
    <div className='bg-white shadow-xl border-gray-300 shadow-purple-300 px-6 rounded-[18px] flex flex-col gap-3 '>
      <div className="text-gray-500 pt-3 pb-2 text-base font-semibold">{restaurant.description}</div>
      <div className='flex flex-row items-center gap-4 '>
        <div  className='text-gray-800'>
        <MdLocationPin size={23} />
        </div>
        <div className="text-gray-600 ">{restaurant.address}</div>
      </div>

      <div className='flex flex-row  items-center gap-4'>
        <div className='text-gray-800'>
        <FaPhone size={18}/>
        </div>
        <div className="text-gray-600 ">{restaurant.phoneNumber}</div>
      </div>

      <div className='flex flex-row   items-center gap-4'>
        <div  className='text-gray-800'>
        <MdEmail size={21}/>
        </div>
        <div className="text-gray-600 ">{restaurant.email}</div>
      </div>

      <hr className='border-[1px solid]  border-black'/>
      <div className='flex flex-row gap-4 text-sm text-gray-500 pb-3'>
        <div>
        <IoBicycleOutline size={20}/>
        </div> 
       <div>
       Order above 149 for discounted delivery fee
        </div> 
      </div>
    </div>

      <div className='flex flex-row gap-2 font-light justify-center items-center bg-white border-gray-300 border-[1px] shadow-gray-400 rounded-md shadow-sm mt-16 p-2 text-gray-400 cursor-pointer'
      onClick={handleNavigateSearch}>
        <div>
          Search for dishes
        </div>
        <div className='text-gray-400'>
        <FaSearch/>
      </div>
      </div>

    </div>
  );
};

export default RestaurantDetails;
