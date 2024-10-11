import { BsArrowLeft, BsArrowRight } from "react-icons/bs";
import { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./Hero.css"; // Import the CSS file
import { jwtDecode } from "jwt-decode";

function Hero() {
  const images = [
    { id: 4, src: "turkey.jpg", name: "Turkey Sandwich" },
    { id: 5, src: "omelette.jpg", name: "Steak Dinner" },
    { id: 6, src: "grillchick.jpg", name: "Lasagna" },
    { id: 7, src: "sandwich.avif", name: "Cheeseburger" },
    { id: 8, src: "steak dinner.jpg", name: "Veggie Burger" },
    { id: 11, src: "lasagna.jpg", name: "Lasagna" },
    { id: 13, src: "pizza.jpg", name: "Pizza" },
    { id: 14, src: "chicken.jpg", name: "Chicken" },
    { id: 15, src: "coffee.jpg", name: "Coffee" }
  ];

  const [currentIndex, setCurrentIndex] = useState(0);
  const imagesPerView = 7;

  // Get the visible images based on currentIndex and imagesPerView
  const visibleImages = [];
  for (let i = 0; i < imagesPerView; i++) {
    visibleImages.push(images[(currentIndex + i) % images.length]);
  }

  const fetchMenu = async (id) => {
    const token = localStorage.getItem('token'); // Retrieve the JWT token from storage
    console.log("Fetching menu with token:", token); // Log the token being used
    try {
      const response = await axios.get(`https://localhost:7263/api/Menus/${id}`, {
        headers: {
          'accept': '*/*',
          'Authorization': `Bearer ${token}` // Include the JWT token here
        }
      });
      console.log("Fetched menu data:", response.data); // Log the fetched menu data
      return response.data; // Return the fetched menu items
    } catch (error) {
      console.error("Error fetching menu items:", error);
      return null; // Return null on error
    }
  };

  const navigate = useNavigate();

 const handleMenuClick = async (id) => {
  const menuData = await fetchMenu(id); // Fetch menu items
  if (menuData) {
    navigate(`/menus/${id}`, { state: { menuId: id } }); // Pass only the menuId to the next route
    console.log("Navigating to menus with ID:", id); // Log the ID being passed
  } else {
    console.error("No menu items fetched or an error occurred.");
  }
};


  // Move to the next set of images
  const handleNext = () => {
    if (currentIndex + imagesPerView < images.length) {
      setCurrentIndex((prevIndex) => prevIndex + 1);
    }
  };

  // Move to the previous set of images
  const handlePrevious = () => {
    if (currentIndex > 0) {
      setCurrentIndex((prevIndex) => prevIndex - 1);
    }
  };

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
            <div className="title">What's on your mind?</div>
          </div>
          <div className="controls">
            <div
              onClick={handlePrevious}
              className={`arrow ${currentIndex === 0 ? "disabled" : ""}`} // Disable left arrow at start
              style={{ pointerEvents: currentIndex === 0 ? "none" : "auto", opacity: currentIndex === 0 ? 0.5 : 1 }}
            >
              <BsArrowLeft size={20} />
            </div>
            <div
              onClick={handleNext}
              className={`arrow ${currentIndex + imagesPerView >= images.length ? "disabled" : ""}`} // Disable right arrow at the end
              style={{ pointerEvents: currentIndex + imagesPerView >= images.length ? "none" : "auto", opacity: currentIndex + imagesPerView >= images.length ? 0.5 : 1 }}
            >
              <BsArrowRight size={20} />
            </div>
          </div>
        </div>
        <div className="image-container">
          {visibleImages.map((item, index) => (
            <div key={index} className="image-wrapper" onClick={() => handleMenuClick(item.id)}>
              <img src={item.src} alt={item.name} className="image" />
              <div className="font-sans text-gray-500 flex justify-center mt-2 cursor-pointer">
                <div className="image-name">{item.name}</div> {/* Image name below */}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

export default Hero;
