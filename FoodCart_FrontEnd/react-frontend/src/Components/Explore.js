import React, { useState, useEffect } from 'react';
import './Explore.css';

export default function Explore() {
    const [selectedCuisine, setSelectedCuisine] = useState('');
    const [menuItems, setMenuItems] = useState([]);
    const [topRatedItems, setTopRatedItems] = useState([]);

    // Fetch top-rated items on component mount
    useEffect(() => {
        const fetchTopRatedItems = async () => {
            try {
                const response = await fetch('https://localhost:7263/api/Top_RatedItems/byTopRatedItems?topN=8');
                const data = await response.json();
                setTopRatedItems(data);
            } catch (error) {
                console.error('Error fetching top-rated items:', error);
            }
        };
        
        fetchTopRatedItems();
    }, []); // Empty dependency array means this runs once on mount

    // Handler for cuisine selection
    const handleCuisineChange = async (e) => {
        const cuisine = e.target.value;
        setSelectedCuisine(cuisine);

        if (cuisine === 'disabled') {
            setMenuItems([]); // Clear the fetched menu items when "Select a Cuisine" is selected
            return;
        }

        try {
            const response = await fetch(`https://localhost:7263/api/Menus/byCuisine?cuisineName=${cuisine}`);
            const data = await response.json();
            setMenuItems(data.menuItem);
        } catch (error) {
            console.error('Error fetching cuisine data:', error);
        }
    };

    return (
        <div className="explore-container p-4">
            {/* Popular Cuisines Section */}
            <div className="explore-section">
                <label className="explore-title font-sans text-lg font-bold"><strong>Popular Cuisines Near Me</strong></label>
                <select className="explore-dropdown mt-2 border border-gray-300 rounded p-2" value={selectedCuisine} onChange={handleCuisineChange}>
                    <option value="disabled">Select a Cuisine</option>
                    <option value="American">American</option>
                    <option value="Indian">Indian</option>
                    <option value="Italian">Italian</option>
                    <option value="Russian">Russian</option>
                    <option value="Asian">Asian</option>
                </select>
            </div>

            {/* Display fetched menu items or message */}
            {selectedCuisine && selectedCuisine !== 'disabled' && (
                <div className="menu-items-section mt-6 font-sans">
                    <h3 className="text-lg font-semibold mb-4">Menu Items</h3>
                    {menuItems.length > 0 ? (
                        menuItems.map((item) => (
                            <div 
                                key={item.itemID} 
                                className="menu-item-card flex items-center space-x-4 p-4 bg-gray-100 rounded-lg shadow-md mb-4 transition-transform transform hover:scale-105 hover:bg-gray-200 hover:shadow-lg"
                            >
                                <img src='/sandwich.avif' alt={item.itemName} className="menu-item-image w-24 h-24 rounded-md object-cover" />
                                <div className="menu-item-details flex-grow">
                                    <h4 className="text-md font-semibold">{item.itemName}</h4>
                                    <p className="text-sm text-gray-600">{item.itemDescription}</p>
                                    <p className="text-sm"><strong>Price:</strong> ${item.itemPrice}</p>
                                    <p className="text-sm"><strong>Cuisine:</strong> {item.cuisineType}</p>
                                    <p className="text-sm"><strong>Rating:</strong> {item.rating}</p>
                                </div>
                                <div className="status-info text-right">
                                    <p className="text-sm"><strong>Status:</strong> {item.availabilityStatus}</p>
                                    <p className="text-sm"><strong>Dietary Info:</strong> {item.dietaryInfo}</p>
                                </div>
                            </div>
                        ))
                    ) : (
                        <p className="text-sm text-gray-500">No items available for the selected cuisine.</p>
                    )}
                </div>
            )}

            {/* Display Top Rated Items Section */}
            <div className="top-rated-section mt-6 font-sans">
                <h3 className="text-lg font-bold mb-4 font-sans">Top Rated Items</h3>
                {topRatedItems.length > 0 ? (
                    <div className="top-rated-items grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 gap-4">
                        {topRatedItems.map((item) => (
                            <div 
                                key={item.menuItemName} 
                                className="top-rated-item flex flex-col items-center bg-white rounded-lg shadow-md p-4 transition-transform transform hover:scale-110 hover:bg-white hover:shadow-lg"
                            >
                                <img src='/grillchick.jpg' alt={item.menuItemName} className="w-24 h-24 rounded-md object-cover mb-2" />
                                <p className="text-md text-gray-400">{item.menuItemName}</p>
                                <p className="text-sm text-gray-900 font-medium">{item.rating} ★</p>
                            </div>
                        ))}
                    </div>
                ) : (
                    <p className="text-sm text-gray-500">No top-rated items available.</p>
                )}
            </div>
        </div>
    );
}
