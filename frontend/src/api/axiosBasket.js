import axios from 'axios';

var baseUrl = import.meta.env.VITE_APP_BASKET_URL;

const basketApi = axios.create({
  baseURL: baseUrl,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default basketApi;
