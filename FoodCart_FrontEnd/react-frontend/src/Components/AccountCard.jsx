import React from 'react'
import { useNavigate } from 'react-router-dom'
import { MdAccountBalance } from "react-icons/md";

const AccountCard = () => {

  const navigate = useNavigate();
  const handleLog =()=>{
    navigate('/login')
  }

  const handleRegister =()=>{
    navigate('/register')
  }
  return (
    <div>
      <div className='flex justify-center items-center bg-gray-100 px-8'>
        <div className="p-8 w-[600px] flex flex-col gap-10">
          
          <div >
            <div className='flex flex-row'>
            <div className='text-xl'><MdAccountBalance /></div>
            <div className="font-bold ml-2" >Account</div>
            </div>
            <div className="">
              To place your order now, log in to your existing account or sign
              up.
            </div>
          </div>

          <div className="flex gap-10">
            <div className="text-lime-400 w-max p-2 px-5 bg-white border border-lime-400 text-sm flex flex-col justify-center items-center cursor-pointer" onClick={handleLog}>
              <div className="font-light">Have an account?</div>
              <div className="capitalize font-semibold">LOG IN</div>
            </div>

            <div className="text-white w-max p-2 px-5 bg-lime-400 border border-lime-400 text-sm flex flex-col justify-center items-center cursor-pointer" onClick={handleRegister}>
              <div className="font-light">New to FoodCart?</div>
              <div className="capitalize font-semibold">SIGN UP</div>
            </div>
          </div>
        </div>

        <div>
          <img
            src="https://media-assets.swiggy.com/swiggy/image/upload/fl_lossy,f_auto,q_auto,w_147,h_140/Image-login_btpq7r"
            alt="food_image"
          />
        </div>
      </div>
    </div>
  );
}

export default AccountCard