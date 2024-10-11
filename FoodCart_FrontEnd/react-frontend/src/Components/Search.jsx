import React, { useState, useEffect } from 'react';
//import { useParams } from "react-router-dom";
import { FaArrowRight , FaSearch } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';
import { IoMdRefresh } from "react-icons/io";
import { jwtDecode } from 'jwt-decode';
import { ToastContainer, toast } from 'react-toastify'; // Import ToastContainer and toast
import 'react-toastify/dist/ReactToastify.css'; // Import CSS for the toast

const Search = () => {
  //const { id } = useParams(); 
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [category, setCategory] = useState('');
  const [minPrice, setMinPrice] = useState('');
  const [maxPrice, setMaxPrice] = useState('');
  const [cuisine, setCuisine] = useState('');
  const [dietaryInfo, setDietaryInfo] = useState('');
  const [isFiltering, setIsFiltering] = useState(false);
  const [userID, setUserID] = useState(null);
  const [jwtToken, setJwtToken] = useState(localStorage.getItem('token'));
  
  const navigate = useNavigate();

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

  console.log("userID", userID);

  const handleSearch = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setResults([]);

    try {
      let response;
      if (isFiltering) {
        response = await fetch(`https://localhost:7263/api/Menus/byFilters?category=${encodeURIComponent(category)}&minPrice=${minPrice}&maxPrice=${maxPrice}&cuisine=${encodeURIComponent(cuisine)}&dietaryInfo=${encodeURIComponent(dietaryInfo)}`);
      } else {
        response = await fetch(`https://localhost:7263/api/Menus/Search?query=${encodeURIComponent(query)}`);
      }

      if (!response.ok) throw new Error('Network response was not ok');

      const data = await response.json();
      if (data.menuItems && Array.isArray(data.menuItems)) {
        setResults(data.menuItems);
      } else {
        setError('Unexpected data format received.');
      }
    } catch (err) {
      setError('Oops! No items available. Try different filters or use Search.');
    } finally {
      setLoading(false);
    }
  };

  const handleSearchClick = (restaurant) => {
    navigate(`/restaurant/${restaurant.restaurantID}`, { state: { restaurant } });
  };

  const handleResetFilters = () => {
    setCategory('');
    setMinPrice('');
    setMaxPrice('');
    setCuisine('');
    setDietaryInfo('');
  };

  const handleAddToCart = async (itemID, restaurantID) => {
    try {
      if (!userID) {
        setError('User not logged in.');
        return;
      }
  
      // Log the values being sent
      console.log('Adding to cart:', {
        userID,
        itemID,
        quantity: 1,
        restaurantID,
      });
  
      const response = await fetch('https://localhost:7263/api/Carts/AddToCart', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${jwtToken}`,
        },
        body: JSON.stringify({
          userID,
          itemID,
          quantity: 1,
          restaurantID,
        }),
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(`Failed to add item to cart: ${errorData.message}`);
      }
  
      const data = await response.json();
      console.log('Item added to cart:', data);
      toast.success('Item successfully added to cart!');
    } catch (err) {
      console.error('Error adding item to cart:', err);
      setError('Failed to add item to cart. Please try again.');
      toast.error('Failed to add item to cart. Please try again.');
    }
  };
  

  return (
    <div className="mt-8 flex flex-col items-center mx-40 ">
      <ToastContainer /> {/* Add ToastContainer here */}
      <form onSubmit={handleSearch} className="flex flex-wrap w-full max-w-2xl">
        <div className="flex flex-col w-full mb-4">
          {!isFiltering && (
            <input
              type='text'
              className='rounded-sm shadow-inner w-full shadow-gray-200 outline-none p-2 mb-2'
              placeholder='Search for restaurants and food'
              value={query}
              onChange={(e) => {
                setQuery(e.target.value);
                setIsFiltering(false);
              }}
            />
          )}
        </div>

        {isFiltering && (
          <div className="flex flex-col space-y-4 w-full">
            <div className="flex flex-row gap-4 items-center justify-center">
              <select
                className='border rounded-md w-28 h-8 text-[12px]'
                value={category}
                onChange={(e) => setCategory(e.target.value)}
              >
                <option value="">All Categories</option>
                <option value="Breakfast">Breakfast</option>
                <option value="Lunch">Lunch</option>
                <option value="Dinner">Dinner</option>
                <option value="Burgers">Burgers</option>
                <option value="Pizza">Pizza</option>
                <option value="Italian">Italian</option>
                <option value="Arabian">Arabian</option>
                <option value="Appetizers">Appetizers</option>
                <option value="Main Dishes">Main Dishes</option>
                <option value="Beverages">Beverages</option>
              </select>
              <div
                onClick={handleSearch}
                className='bg-gray-500 p-1.5 text-white rounded-md flex text-base justify-center items-center font-poppins font-light shadow-lg hover:shadow-xl transition-shadow cursor-pointer'
              >
                <FaSearch />
              </div>
           
              <input
                type='number'
                placeholder='Min Price'
                className='border rounded-md w-28 h-8 text-[12px] p-2'
                value={minPrice}
                onChange={(e) => setMinPrice(e.target.value)}
              />
              <input
                type='number'
                placeholder='Max Price'
                className='border rounded-md w-28 h-8 text-[12px] p-2'
                value={maxPrice}
                onChange={(e) => setMaxPrice(e.target.value)}
              />
              <input
                type='text'
                placeholder='Cuisine'
                className='border rounded-md w-24 h-8 text-[12px] p-2'
                value={cuisine}
                onChange={(e) => setCuisine(e.target.value)}
              />
              <input
                type='text'
                placeholder='Dietary Info (e.g., Vegetarian)'
                className='border p-2 rounded-md w-44 h-8 text-[12px]'
                value={dietaryInfo}
                onChange={(e) => setDietaryInfo(e.target.value)}
              />
              <div
                type='button'
                onClick={handleResetFilters}
                className='bg-orange-500 text-white p-1 rounded-md text-xl font-poppins font-light shadow-lg hover:shadow-xl transition-shadow cursor-pointer'
              >
                <IoMdRefresh />
              </div>
            </div>
            
          </div>
        )}
      </form>

      <label className="mt-4 flex items-center">
        <input 
          type="checkbox" 
          checked={isFiltering} 
          onChange={() => setIsFiltering(!isFiltering)} 
          className="mr-2"
        />
        <div className='font-sans text-sm underline text-orange-400'>
        {isFiltering? 'Switch to Search' : 'Apply Filters'}
        </div>
      </label>

      {loading && <p>Loading...</p>}
      {error && <p className="text-red-500">{error}</p>}

      {results.length > 0 && (
        <div className="mt-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 w-full px-4">
          {results.map((item) => (
            <React.Fragment key={item.itemID}>
              {item.restaurants.map((restaurant) => (
                <div
                  key={restaurant.restaurantID}
                  className="bg-white shadow-lg rounded-lg p-6 hover:shadow-2xl transition-shadow"
                >
                  <div className='flex justify-between items-center py-3'>
                    <div className="font-semibold text-sm text-gray-500">
                      By {restaurant.name}
                    </div>
                    <FaArrowRight 
                      size={16} 
                      className='text-gray-500 font-thin cursor-pointer' 
                      onClick={() => handleSearchClick(restaurant)}
                    />
                  </div>
                  <hr className='border-dashed border-gray-300' />
                  <div className='flex flex-row justify-between items-center mt-4'>
                    <div>
                      <div className="mt-2 text-gray-700 font-semibold text-lg">
                        {item.itemName}
                      </div>
                      <div className="mt-1 text-gray-500 font-semibold">
                        ${item.itemPrice.toFixed(2)}
                      </div>
                    </div>
                    <div className='relative'>
                      <img src='/salad.webp' alt="" className='w-28 h-28 rounded-md shadow-lg' />
                      <div
                        className='absolute flex flex-row bg-white -bottom-3 px-3 py-0.5 rounded-sm font-semibold text-green-600 left-6 shadow-lg cursor-pointer'
                        onClick={() => handleAddToCart(item.itemID, restaurant.restaurantID)}
                      >
                        ADD
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </React.Fragment>
          ))}

        </div>
      )}
    </div>
  );
};

export default Search;
