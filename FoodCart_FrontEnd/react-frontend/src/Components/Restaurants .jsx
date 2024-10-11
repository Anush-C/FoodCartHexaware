import { BsArrowLeft, BsArrowRight } from "react-icons/bs";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import {jwtDecode} from "jwt-decode"; // Import jwt-decode

export default function Restaurants() {
    const navigate = useNavigate();
    
    // Sample restaurant data
    const restaurants = [
        { id: 1, name: "The Breakfast Club", address: "123 Morning St", image: "r1.avif" },
        { id: 2, name: "Lunch Haven", address: "456 Lunchtime Ave", image: "r2.avif" },
        { id: 3, name: "Dinner Delight", address: "789 Evening Blvd", image: "r3.avif" },
        { id: 4, name: "Burger Bonanza", address: "321 Burger Ln", image: "r4.avif" },
        { id: 5, name: "Pizza Palace", address: "654 Pizza Rd", image: "r5.avif" },
        { id: 6, name: "Italian Bistro", address: "987 Pasta Dr", image: "r6.avif" },
        { id: 7, name: "Arabian Nights", address: "135 Spice St", image: "r7.avif" },
        { id: 8, name: "Appetizer Alley", address: "246 Snack Ct", image: "r8.avif" },
    ];

    const [currentIndex, setCurrentIndex] = useState(0);
    const imagesPerView = 7;
    const totalImages = restaurants.length;

    const visibleRestaurants = [];
    for (let i = 0; i < imagesPerView; i++) {
        const restaurant = restaurants[(currentIndex + i) % totalImages];
        visibleRestaurants.push(restaurant);
    }

    // Function to fetch menu items for the selected restaurant
    const fetchMenuItems = async (restaurantId) => {
        const token = localStorage.getItem('token'); // Retrieve the JWT token from storage
        try {
            const response = await axios.get(`https://localhost:7263/api/Menus/byRestaurant/${restaurantId}`, {
                headers: {
                    'accept': '*/*',
                    'Authorization': `Bearer ${token}` // Include the JWT token here
                }
            });
            return response.data; // Return the fetched menu items
        } catch (error) {
            console.error("Error fetching menu items:", error);
            return []; // Return an empty array on error
        }
    };

    // Navigate to the menu items of the selected restaurant
    const handleRestaurantClick = async (id) => {
        const menuItems = await fetchMenuItems(id); // Fetch menu items
        navigate(`/menu/${id}`, { state: { menuItems } }); // Pass menu items to the next route
    };

    // Functions for pagination
    const handlePrevious = () => {
        setCurrentIndex((prevIndex) => (prevIndex - imagesPerView + totalImages) % totalImages);
    };

    const handleNext = () => {
        setCurrentIndex((prevIndex) => (prevIndex + imagesPerView) % totalImages);
    };

    // Optional: Decode JWT token to extract user information
    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            const decodedToken = jwtDecode(token);
            console.log("Decoded JWT:", decodedToken); 
        }
    }, []);

    return (
        <div className="container">
            <div className="carousel">
                <div className="header">
                    <div className="font-sans">
                    <div className="title">Top Restaurant Chains</div>
                    </div>
                    <div className="controls">
                        <div
                            onClick={handlePrevious}
                            className={`arrow ${currentIndex === 0 ? "disabled" : ""}`}
                        >
                            <BsArrowLeft size={20} />
                        </div>
                        <div
                            onClick={handleNext}
                            className={`arrow ${currentIndex + imagesPerView >= totalImages ? "disabled" : ""}`}
                        >
                            <BsArrowRight size={20} />
                        </div>
                    </div>
                </div>
                <div className="image-container">
                    {visibleRestaurants.map((restaurant, index) => (
                        <div key={index} className="restaurant-card" onClick={() => handleRestaurantClick(restaurant.id)}>
                            <div className="rounded-lg shadow-xl">
                            <img src={restaurant.image} alt={restaurant.name} className="image" />
                            </div>
                            <div className="font-medium mt-5 text-gray-500 cursor-pointer font-sans text-base">
                            <div className="restaurant-info">
                                <h3 className="restaurant-name">{restaurant.name}</h3>
                                <p className="restaurant-address">{restaurant.address}</p>
                            </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}
