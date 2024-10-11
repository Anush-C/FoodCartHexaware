import React from 'react'

const ReviewInCart = () => {
  return (
    <div className="w-80 p-4 text-sm ml-9">
      <div className='flex flex-col gap-3 border border-gray-200 p-3 px-4 mt-4 rounded-md'>
        <div className="font-bold font-sans">
          Review your order and address details to avoid cancellations
        </div>

        <div>
          Note: Please ensure your address and order details are correct. This
          order, if cancelled, is non-refundable.
        </div>

        <div>
            <button className="font-bold font-sans text-black underline underline-offset-2 cursor-pointer hover:bg-green-600 bg-white p-2">
              Read policy
            </button>
        </div>
      </div>
    </div>
  );
}

export default ReviewInCart