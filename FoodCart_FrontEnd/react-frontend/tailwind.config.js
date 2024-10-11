/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",  // Specifies which files Tailwind should scan for class names
  ],
  theme: {
    extend: {
      fontFamily: {
        "sans": ['Poppins'],  // Adds 'Poppins' as the default sans-serif font
      }
      
    },
  },
  plugins: [],
}
