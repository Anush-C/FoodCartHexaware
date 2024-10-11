import React from 'react';
//import Navbar from '../Components/Navbar';
import Hero from '../Components/Hero';
//import './Home.css';
import Restaurants from '../Components/Restaurants ';
import Explore from '../Components/Explore';
import CustomerReviews from '../Components/CustomerReviews';
import Footer from '../Components/Footer';
const Home = () => {
  return (
    <div className='container'>
      <Hero />
      <Restaurants/>
      <Explore/>
      <CustomerReviews/>
      <Footer/>
    </div>
  );
};

export default Home;
