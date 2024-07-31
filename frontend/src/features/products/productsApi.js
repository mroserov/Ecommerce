import { createAsyncThunk } from '@reduxjs/toolkit';
import { getProducts } from 'api/services/catalogService';

export const fetchProducts = createAsyncThunk(
  'products/fetchProducts',
  async ({ searchTerm = '', page, pageSize }) => {
    const response = await getProducts(searchTerm, page, pageSize);
    return response.data.products;
  }
);
