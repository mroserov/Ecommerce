import productsReducer from '../features/products/productsSlice';
import cartReducer from '../features/cart/cartSlice';
import snackbarReducer from 'features/snackbarSlice';

// reducer import
import { configureStore } from '@reduxjs/toolkit';

const reducer = configureStore({
  reducer: {
    products: productsReducer,
    cart: cartReducer,
    snackbar: snackbarReducer
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      thunk: {
        extraArgument: {}
      },
      serializableCheck: false,
    }),
});

export default reducer;
