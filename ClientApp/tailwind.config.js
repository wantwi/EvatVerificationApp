/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      backgroundImage: {
        'side-bg': 'url(/src/assets/bg.png)',
      },
    },
  },
  plugins: [],
}
