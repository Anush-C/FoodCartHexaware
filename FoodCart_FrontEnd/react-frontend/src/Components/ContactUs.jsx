import React from 'react';

const ContactUs = () => {
    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-4">
            <h1 className="text-3xl font-bold mb-6">Contact Us</h1>
            <div className="bg-white shadow-md rounded-lg p-8 w-full max-w-md">
                <h2 className="text-xl font-semibold mb-4">Get in Touch</h2>
                <p className="mb-2">
                    <strong>Phone:</strong> +1 (234) 567-8901
                </p>
                <p className="mb-2">
                    <strong>Email:</strong> support@foodcart.com
                </p>
                <h3 className="text-lg font-semibold mt-4 mb-2">Follow Us</h3>
                <div className="flex space-x-4">
                    <a href="https://www.facebook.com" target="_blank" rel="noopener noreferrer">
                        <img src="/fb.png" alt="Facebook" className="w-8 h-8"/>
                    </a>
                    <a href="https://www.twitter.com" target="_blank" rel="noopener noreferrer">
                        <img src="/twitter.png" alt="Twitter" className="w-8 h-8"/>
                    </a>
                    <a href="https://www.instagram.com" target="_blank" rel="noopener noreferrer">
                        <img src="/instagram.jpeg" alt="Instagram" className="w-8 h-8"/>
                    </a>
                    <a href="https://www.linkedin.com" target="_blank" rel="noopener noreferrer">
                        <img src="/linkedin.png" alt="LinkedIn" className="w-8 h-8"/>
                    </a>
                </div>
            </div>
        </div>
    );
};

export default ContactUs;
