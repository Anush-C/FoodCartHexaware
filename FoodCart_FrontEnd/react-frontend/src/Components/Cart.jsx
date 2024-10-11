import React, { useEffect, useState } from "react";
import FoodPaymentCard from "./FoodPaymentCard.jsx";
import ReviewInCart from "./ReviewInCart";
import AccountCard from "./AccountCard.jsx";
import DeliveryAddressCart from "./DeliveryAddressCart.jsx";
import PaymentCard from "../Components/PaymentCard.jsx";

const Cart = () => {
  const [jwtToken, setJwtToken] = useState(null);

  useEffect(() => {
    // Get the token from localStorage or wherever it's stored
    const token = localStorage.getItem("token");
    setJwtToken(token);
  }, []); // Only run this effect once when the component mounts

  return (
    <div className="flex flex-row justify-center mt-6">
      <div className="flex flex-col ">
        {/* Conditionally render AccountCard if jwtToken is not present */}
        {!jwtToken && (
          <div>
            <AccountCard />
          </div>
        )}
        <div>
          <DeliveryAddressCart />
        </div>
        <div>
          <PaymentCard />
        </div>
      </div>
      <div>
        <FoodPaymentCard />
        <ReviewInCart />
      </div>
    </div>
  );
};

export default Cart;
