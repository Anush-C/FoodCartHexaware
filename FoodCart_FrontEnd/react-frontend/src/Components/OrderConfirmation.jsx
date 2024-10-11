import React, { useEffect, useState } from 'react';
import axios from 'axios';
import 'tailwindcss/tailwind.css';
import { useParams } from 'react-router-dom';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

const OrderConfirmation = () => {
  const { orderId } = useParams();
  const [orderDetails, setOrderDetails] = useState(null);

  useEffect(() => {
    const fetchOrderDetails = async () => {
      try {
        const response = await axios.get(`https://localhost:7263/api/Orders/confirmation/${orderId}`, {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        });

        setOrderDetails(response.data);
      } catch (error) {
        console.error('Error fetching order details:', error);
      }
    };

    fetchOrderDetails();
  }, [orderId]);

  if (!orderDetails) {
    return <div>Loading...</div>;
  }

  const { items, restaurantName, paymentMethod, deliveryTime, deliveryAgentName, deliveryAgentPhone } = orderDetails;

  const downloadPDF = async () => {
    const input = document.getElementById('order-confirmation');
    const buttons = document.querySelectorAll('.pdf-button'); // Select buttons by class

    // Hide buttons
    buttons.forEach(button => button.classList.add('hidden'));

    const canvas = await html2canvas(input);
    const imgData = canvas.toDataURL('image/png');
    const pdf = new jsPDF();
    const imgWidth = 190; // Set your image width
    const pageHeight = pdf.internal.pageSize.height;
    const imgHeight = (canvas.height * imgWidth) / canvas.width;
    let heightLeft = imgHeight;

    let position = 0;

    // If the height of the image exceeds the page height, split it into multiple pages
    if (heightLeft >= pageHeight) {
      position = 0;
      while (heightLeft >= 0) {
        pdf.addImage(imgData, 'PNG', 10, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
        position -= pageHeight;
        if (heightLeft > 0) {
          pdf.addPage();
        }
      }
    } else {
      pdf.addImage(imgData, 'PNG', 10, 0, imgWidth, imgHeight);
    }

    pdf.save('order_confirmation.pdf');

    // Show buttons again
    buttons.forEach(button => button.classList.remove('hidden'));
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100 mt-4 shadow-lg">
      <div id="order-confirmation" className="bg-white shadow-xl rounded-lg p-8 max-w-2xl w-full">
        <div className='flex items-center justify-center mb-6'>
          <img src='/logofc.png' alt="Logo" className="w-10 h-10 mr-3" />
          <h2 className="text-3xl font-bold text-gray-800 mb-6 text-center font-sans mt-6">Order Confirmation</h2>
        </div>
        
        <div className="mb-8">
          <h3 className="text-lg font-semibold mb-4 text-gray-800 font-sans">Order Summary</h3>
          <table className="min-w-full bg-white border border-gray-300">
            <thead>
              <tr className="bg-gray-100">
                <th className="py-2 px-4 text-left text-sm font-semibold text-gray-600 font-sans border-b">Item Name</th>
                <th className="py-2 px-4 text-left text-sm font-semibold text-gray-600 font-sans border-b">Quantity</th>
                <th className="py-2 px-4 text-left text-sm font-semibold text-gray-600 font-sans border-b">Price</th>
              </tr>
            </thead>
            <tbody>
              {items.map((item) => (
                <tr key={item.itemId} className="hover:bg-gray-50">
                  <td className="py-2 px-4 text-gray-700 font-sans border-b">{item.itemName}</td>
                  <td className="py-2 px-4 text-gray-700 font-sans border-b">{item.quantity}</td>
                  <td className="py-2 px-4 text-gray-700 font-sans border-b">${item.price.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="mb-8">
          <h3 className="text-lg font-semibold mb-2 text-gray-800 font-sans">Restaurant</h3>
          <p className="text-gray-700 font-sans">{restaurantName}</p>
        </div>

        <div className="mb-8">
          <h3 className="text-lg font-semibold mb-2 text-gray-800 font-sans">Payment Method</h3>
          <p className="text-gray-700 capitalize font-sans">{paymentMethod === 'cod' ? 'Cash on Delivery' : 'Card'}</p>
        </div>

        <div className="mb-8">
          <h3 className="text-lg font-semibold mb-2 text-gray-800 font-sans">Estimated Delivery Time</h3>
          <p className="text-gray-700 font-sans">{deliveryTime}</p>
        </div>

        <div className="mb-8">
          <h3 className="text-lg font-semibold mb-2 text-gray-800 font-sans">Delivery Agent</h3>
          <p className="text-gray-700 font-sans">{deliveryAgentName} - {deliveryAgentPhone}</p>
        </div>

        {/* Add a class for the buttons */}
        <button
          className="pdf-button w-full p-3 bg-green-600 text-white font-bold rounded-lg hover:bg-green-700 focus:ring-4 focus:ring-green-300 transition-colors duration-300 mb-4"
          onClick={downloadPDF}
        >
          Download PDF
        </button>

        <button
          className="pdf-button w-full p-3 bg-blue-600 text-white font-bold rounded-lg hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 transition-colors duration-300"
          onClick={() => window.location.href = '/'}
        >
          Go to Dashboard
        </button>
      </div>
    </div>
  );
};

export default OrderConfirmation;
