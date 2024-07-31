import { createSlice } from '@reduxjs/toolkit';
import { fetchProducts } from './productsApi';

const productsSlice = createSlice({
  name: 'products',
  initialState: {
    items: [],
    status: 'idle',
    error: null,
    total: 0,
    totalPages: 0,
    totalCount: 0,
    currentPage: 1,
    shopCart: [],
    user: null,
    serachTerm: '',
  },
  reducers: {
    setPage: (state, action) => {
      state.currentPage = action.payload;
    },
    setSearchTerm: (state, action) => {
      state.searchTerm = action.payload;
    },
    updateStock: (state, action) => {
      const { products } = action.payload;
      products.forEach((p) => {
        const product = state.items.find((item) => item.id === p?.id);
        if (product) {
          product.stock -= p.quantity;
        }
      });
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchProducts.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchProducts.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.items = action.payload.items;
        state.total = action.payload.total;
        state.totalPages = action.payload.totalPages;
        state.totalCount = action.payload.totalCount;
      })
      .addCase(fetchProducts.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      })
      ;
  },
});

export const { setPage, setSearchTerm, updateStock } = productsSlice.actions;

export default productsSlice.reducer;
