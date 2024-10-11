import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import { jwtDecode } from "jwt-decode";  // Removed curly braces
import { toast, ToastContainer } from 'react-toastify';  // Import react-toastify
import 'react-toastify/dist/ReactToastify.css';  // Import toastify CSS

export default function Menu() {
    const { id } = useParams(); // Get the restaurant ID from the route parameters
    const [menuItems, setMenuItems] = useState([]); // State to hold menu items
    const [loading, setLoading] = useState(true); // State to manage loading status
    const [error, setError] = useState(null); // State to manage error messages
    const [fadeIn, setFadeIn] = useState(false); // State for fade-in effect
    const [userID, setUserID] = useState(null);
    const [jwtToken, setJwtToken] = useState(localStorage.getItem('token'));

    // Decoding the JWT to extract user ID
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

    // Fetch menu items based on restaurant ID
    const fetchMenuItems = async () => {
        const token = localStorage.getItem('token');
        try {
            const response = await axios.get(`https://localhost:7263/api/Menus/byRestaurant/${id}`, {
                headers: {
                    'accept': '*/*',
                    'Authorization': `Bearer ${token}`
                }
            });
            setMenuItems(response.data); // Set the fetched menu items in state
        } catch (error) {
            console.error("Error fetching menu items:", error);
            setError("Failed to fetch menu items. Please try again later.");
        } finally {
            setLoading(false); // Set loading to false after fetching
        }
    };

    // Function to handle Add to Cart action
    const handleAddToCart = async (itemID) => {
        try {
            if (!userID) {
                setError('User not logged in.');
                return;
            }

            const response = await fetch('https://localhost:7263/api/Carts/AddToCart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${jwtToken}`,
                },
                body: JSON.stringify({
                    userID: userID,
                    itemID: itemID,
                    quantity: 1,
                    restaurantID: id, // Use restaurant ID
                }),
            });

            if (!response.ok) throw new Error('Failed to add item to cart');

            const data = await response.json();
            console.log('Item added to cart:', data);
            
            // Trigger success toast notification
            toast.success('Item added to cart successfully!', {
                position: "top-right",
                autoClose: 3000,  // Auto-close after 3 seconds
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            });
        } catch (err) {
            console.error('Error adding item to cart:', err);
            setError('Failed to add item to cart. Please try again.');
            toast.error('Error adding item to cart!', {
                position: "top-right",
                autoClose: 3000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            });
        }
    }

    // Fetch menu items when the component mounts or the ID changes
    useEffect(() => {
        fetchMenuItems();
        setFadeIn(true);
    }, [id]);

    if (loading) {
        return <p className="text-center">Loading menu items...</p>;
    }

    return (
        <div className={`p-6  font-sans fade-in ${fadeIn ? 'fade-in-active' : ''}`}>
            <h2 className="text-xl font-semibold mb-6 text-center">Great! Have your choice of Food!!</h2>
            {error ? (
                <p className="text-red-500 text-center">{error}</p>
            ) : menuItems.length > 0 ? (
                <ul className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {menuItems.map(item => (
                        <li key={item.itemID} className="border rounded-lg shadow-xl p-4 bg-white hover:shadow-2xl transition-shadow duration-500 flex flex-col border-7">
                            <img 
                                src={item.imageUrl || process.env.PUBLIC_URL + '/pancake.jpg'}
                                alt={item.itemName}
                                className="w-full h-48 object-cover rounded-lg mb-4 transition-transform duration-300 hover:scale-110"
                            />
                            <div className="flex-1">
                                <h3 className="text-xl font-semibold text-gray-800 hover:text-blue-600 transition-colors duration-300">{item.itemName}</h3>
                                <p className="text-gray-600">{item.itemDescription}</p>
                                <p className="font-medium text-lg mt-2">Price: <span className="text-green-600">${item.itemPrice.toFixed(2)}</span></p>
                                <p className="text-sm text-gray-500 mt-2">Cuisine: {item.cuisineType}</p>
                                {item.dietaryInfo && <p className="text-sm text-gray-500">Dietary Info: {item.dietaryInfo}</p>}
                                <p className="text-sm text-gray-500">Ingredients: {Array.isArray(item.ingredients) ? item.ingredients.join(', ') : item.ingredients}</p>

                                {/* Add to Cart Button */}
                                <div>
                                    <button 
                                        onClick={() => handleAddToCart(item.itemID)}
                                        className='flex justify-center items-center mt-2 bg-white -bottom-3 px-3 py-0.5 rounded-sm font-semibold text-green-600 left-6 shadow-lg cursor-pointer hover:text-black'
                                    >
                                        ADD
                                    </button>
                                </div>
                            </div>
                        </li>
                    ))}
                </ul>
            ) : (
                <p className="text-center">No menu items available for this restaurant.</p>
            )}
            
            {/* Toast container for notifications */}
            <ToastContainer />
        </div>
    );
}
