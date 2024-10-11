import React, { useState } from 'react';
import DeliveryAddressCart from './DeliveryAddressCart'; // Import the DeliveryAddressCart component
import Checkout from './Checkout'; // Import the Checkout component

const Main = () => {
  const [selectedAddress, setSelectedAddress] = useState("");

  const handleSelectAddress = (address) => {
    setSelectedAddress(address);
  };

  return (
    <div>
      <DeliveryAddressCart onSelectAddress={handleSelectAddress} />
      <Checkout shippingAddress={selectedAddress} />
    </div>
  );
};

export default Main;
