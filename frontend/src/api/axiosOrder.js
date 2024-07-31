import axios from 'axios';

var baseUrl = import.meta.env.VITE_APP_ORDERS_URL;

const orderApi = axios.create({
  baseURL: baseUrl, 
  headers: {
    'Content-Type': 'application/json',
  },
});

export default orderApi;
