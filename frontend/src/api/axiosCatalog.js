import axios from 'axios';

var baseUrl = import.meta.env.VITE_APP_CATALOG_URL;

const catalogApi = axios.create({
  baseURL: baseUrl,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default catalogApi;
