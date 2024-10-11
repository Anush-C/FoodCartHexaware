import React, { useEffect, useState } from "react";
import { FaPlus, FaMinus, FaTrash } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

const FoodPaymentCard = () => {
  const [cartItems, setCartItems] = useState([]);
  const [message, setMessage] = useState(""); // New state for the message
  const navigate = useNavigate();

  // Fees
  const deliveryFee = 0;
  const extraDiscount = 0;
  const platformFee = 0;

  // Calculate total amount to be paid
  const getTotalAmount = (itemTotal) => {
    return itemTotal + deliveryFee + platformFee;
  };

  // Fetch cart items including restaurant data
  const fetchAllCarts = async () => {
    try {
      const response = await fetch("https://localhost:7263/api/Carts/AllCarts", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });


      if (response.ok) {
        const result = await response.json();
        const items = result.map((item) => ({
          cartId: item.cartID,
          name: item.itemName,
          price: item.totalCost,
          quantity: item.quantity,
          restaurantId: item.restaurantID,
          restaurantName: item.restaurantName,
          
        }));
        
        if (items.length > 0) {
          console.log("Storing Restaurant ID: ", items[0].restaurantId); // Add this log
          localStorage.setItem("restaurantId", items[0].restaurantId);
        }
        
        

        
        
        
        setCartItems(items);
      } else {
        console.error("Failed to fetch carts:", response.statusText);
      }
    } catch (error) {
      console.error("Error while fetching carts:", error);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      try {
        const decodedToken = jwtDecode(token);
        const userId = decodedToken.UserId;

        if (userId) {
          fetchAllCarts();
        }
      } catch (error) {
        console.error("Error decoding token:", error);
      }
    }
  }, []);

  // Calculate total amount for cart items
  const totalAmount = cartItems.reduce(
    (acc, item) => acc + item.price * item.quantity,
    0
  );

  const updateCartItemQuantity = async (cartId, newQuantity) => {
    try {
      const response = await fetch("https://localhost:7263/api/Carts/UpdateCart", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({ cartId, quantity: newQuantity }), // Update body with cartId and new quantity
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(`Failed to update quantity: ${errorData.message}`);
      }
    } catch (error) {
      console.error("Error while updating quantity:", error);
    }
  };

  const handleDecrement = (index) => {
    const updatedCartItems = [...cartItems];
    if (updatedCartItems[index].quantity > 1) {
      const newQuantity = updatedCartItems[index].quantity - 1;
      updatedCartItems[index].quantity = newQuantity;
      setCartItems(updatedCartItems);
      updateCartItemQuantity(updatedCartItems[index].cartId, newQuantity); // Call API to update quantity
    }
  };


  const handleIncrement = (index) => {
    const updatedCartItems = [...cartItems];
    const newQuantity = updatedCartItems[index].quantity + 1;
    updatedCartItems[index].quantity = newQuantity;
    setCartItems(updatedCartItems);
    updateCartItemQuantity(updatedCartItems[index].cartId, newQuantity); // Call API to update quantity
  };



  const handleDelete = async (cartId) => {
    console.log("Deleting cart with CartId:", cartId); // Debugging line
    
    if (!cartId) {
      console.error("CartId is undefined or invalid");
      return;
    }
  
    try {
      const response = await fetch(`https://localhost:7263/api/Carts/RemoveCartItem/${cartId}`, 
        {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );
  
      if (response.ok) {
        setCartItems((prevItems) => prevItems.filter((item) => item.cartId !== cartId));
        console.log("Item removed successfully");
      } else {
        const errorData = await response.json();
        console.error("Failed to delete the item:", errorData);
      }
    } catch (error) {
      console.error("Error while deleting item:", error);
    }
  };

  // Function to handle "To Pay" button click
  const handlePaymentOptions = () => {
    navigate("/checkout", { state: { restaurantName: cartItems[0].restaurantName } });
  };
  

  return (
    <div className="ml-8">
      <div className="bg-white flex flex-col w-80 shadow-lg">
        <div className="flex items-start pl-4 pt-4 shadow-[0_3px_10px_rgb(0,0,0,0.2)]">
          <div className="w-20 h-20 p-1">
            <img
              src="https://media.istockphoto.com/id/1829241109/photo/enjoying-a-brunch-together.jpg?s=2048x2048&w=is&k=20&c=0UD2e_KMbkkMcx4j9ZaxGf-z1nMHxch9hi3_0BQmuAo="
              alt="restaurant"
              className="rounded-lg"
            />
          </div>

          <div className="flex flex-col ml-2">
            <div className="text-lg font-semibold mt-3 font-sans">
              {cartItems.length > 0 ? cartItems[0].restaurantName : "Restaurant Name"}
            </div>
          </div>
        </div>

        <div className="px-4 py-4">
          {cartItems.map((item, index) => (
            <div
              key={item.cartId}
              className="flex justify-between items-center my-2 text-sm"
            >
              <div className="w-28 font-sans">{item.name}</div>

              <div className="flex items-center space-x-2 hover:text-red-500">
                <FaMinus
                  className="cursor-pointer"
                  onClick={() => handleDecrement(index)}
                />
              </div>
              <div className="hover:text-green-500 flex items-center space-x-2">
                <span>x{item.quantity}</span>
                <FaPlus
                  className="cursor-pointer"
                  onClick={() => handleIncrement(index)}
                />
              </div>

              <div>Rs. {item.price * item.quantity}</div>

              <FaTrash
                className="cursor-pointer text-red-600"
                onClick={() => handleDelete(item.cartId)}
              />
            </div>
          ))}
        </div>

        {/* Display message if an item is already in the cart */}
        {message && (
          <div className="px-4 py-2 bg-red-100 text-red-600 rounded-lg mb-4">
            {message}
          </div>
        )}

        <div className="bg-gray-100 px-4 py-4">
          <div className="font-bold mb-2 font-sans">Bill details</div>

          <div className="flex justify-between text-sm font-sans">
            <span>Item total</span>
            <span>Rs. {totalAmount}</span>
          </div>

          <div className="flex justify-between text-sm my-1 font-sans">
            <span>Delivery charges</span>
            <span>Rs. {deliveryFee}</span>
          </div>

          <div className="flex justify-between text-sm font-sans">
            <span>Extra discount for you</span>
            <span>Rs. {extraDiscount}</span>
          </div>

          <hr className="my-2" />

          <div className="flex justify-between text-sm font-sans">
            <span>Platform fee</span>
            <span>Rs. {platformFee}</span>
          </div>

          <hr className="my-2" />

          <div className="flex justify-between items-center text-lg font-bold font-sans">
            <span>To Pay</span>
            <button
              className="bg-green-400 text-white px-6 py-2 rounded-lg hover:bg-lime-600"
              onClick={handlePaymentOptions}
            >
              Rs. {getTotalAmount(totalAmount)}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default FoodPaymentCard;
