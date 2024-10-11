import React from 'react';

// Sample reviews
const reviews = [
    {
        id: 1,
        name: "John Doe",
        rating: 5,
        review: "Absolutely loved this app! It made ordering food so easy and convenient.",
    },
    {
        id: 2,
        name: "Jane Smith",
        rating: 4,
        review: "Great experience! Fast delivery and delicious food. Highly recommend!",
    },
    {
        id: 3,
        name: "Alice Johnson",
        rating: 3,
        review: "The app is good, but it could use some improvements in the search feature.",
    },
    {
        id: 4,
        name: "Bob Brown",
        rating: 5,
        review: "Best food delivery service I've used! The variety is amazing.",
    },
    {
        id: 5,
        name: "Charlie White",
        rating: 4,
        review: "Good overall, but I wish there were more local restaurant options.",
    },
];

export default function CustomerReviews() {
    return (
        <div className="mx-auto p-4">
            {/* Common Image for Customer Reviews */}
            <h2 className="text-2xl font-bold mb-4">Customer Reviews</h2>
            <img 
                src="/happy.webp" // Replace with your image path
                alt="Happy Customers"
                className=" object-cover rounded-lg mb-4"
            />
            
            {reviews.map((review) => (
                <div 
                    key={review.id} 
                    className="bg-gray-100 border border-gray-300 rounded-lg p-4 mb-4 shadow transition-transform transform hover:scale-105 hover:shadow-lg"
                >
                    <div className="flex justify-between items-center mb-2">
                        <span className="font-semibold">{review.name}</span>
                        <span className="text-yellow-500">
                            {'★'.repeat(review.rating)}{'☆'.repeat(5 - review.rating)}
                        </span>
                    </div>
                    <p className="text-gray-700 italic">"{review.review}"</p>
                </div>
            ))}
        </div>
    );
}
