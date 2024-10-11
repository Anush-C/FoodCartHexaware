import React, { useEffect, useState, useContext } from 'react';
import { useLocation } from 'react-router-dom';
import axios from 'axios';
import { AuthContext } from '../AuthContext'

const SearchResults = () => {
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);
    const searchTerm = searchParams.get('location');

    const { authData } = useContext(AuthContext); // Access the auth context

    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchResults = async () => {
            try {
                const response = await axios.get(`https://localhost:7263/api/Menus/Location?location`, {
                    params: { address: searchTerm },
                    headers: {
                        Authorization: `Bearer ${authData.token}` // Include the token from context
                    }
                });
                setResults(response.data);
            } catch (error) {
                setError('Failed to fetch data');
                console.error('Error fetching data:', error);
            } finally {
                setLoading(false);
            }
        };

        if (searchTerm) {
            fetchResults();
        }
    }, [searchTerm, authData.token]);

    return (
        <div className="results-page">
            <h2>Search Results for "{searchTerm}"</h2>
            {loading ? (
                <p>Loading...</p>
            ) : error ? (
                <p>{error}</p>
            ) : results.length > 0 ? (
                <ul>
                    {results.map((restaurant) => (
                        <li key={restaurant.RestaurantID}>
                            <h3>{restaurant.RestaurantName}</h3>
                            <p>{restaurant.RestaurantDescription}</p>
                            <ul>
                                {restaurant.MenuItems.map((menuItem) => (
                                    <li key={menuItem.MenuItemID}>
                                        {menuItem.MenuItemName} - {menuItem.Price}
                                    </li>
                                ))}
                            </ul>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No results found</p>
            )}
        </div>
    );
};

export default SearchResults;
