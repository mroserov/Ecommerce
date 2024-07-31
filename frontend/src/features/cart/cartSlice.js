import { createSlice,createAsyncThunk } from '@reduxjs/toolkit';
import { 
  getShoppingCart, 
  addItemToShoppingCart, 
  clearShoppingCart, 
  removeItemFromShoppingCart, 
  updateItemInShoppingCart 
} from 'api/services/basketService';
import { getUserId } from 'utils/userId';

// Thunks for async actions
export const fetchCart = createAsyncThunk(
  'cart/fetchCart',
  async (_,{getState}) => {
    const userId = getUserId();
    const response = await getShoppingCart(userId);
    
    const state = getState();
    const products = state.products.items;
    const cartItems = response.items.map(cartItem => {
      const productInfo = products.find(product =>parseInt(product.id) === cartItem.id) || {};
      return {
        ...cartItem,
        ...productInfo,
      };
    });
    return { items: cartItems };
  }
);

export const addToCart = createAsyncThunk(
  'cart/addToCart',
  async (item) => {
    const userId = getUserId();
    await addItemToShoppingCart(userId, item);
    return item;
  }
);

export const updateCartQuantity = createAsyncThunk(
  'cart/updateCartQuantity',
  async (item) => {
    const userId = getUserId();
    await updateItemInShoppingCart(userId, item);
    return item;
  }
);

export const removeFromCart = createAsyncThunk(
  'cart/removeFromCart',
  async (productId) => {
    const userId = getUserId();
    await removeItemFromShoppingCart(userId, productId);
    return productId;
  }
);

export const clearCart = createAsyncThunk(
  'cart/clearCart',
  async () => {
    const userId = getUserId();
    await clearShoppingCart(userId);
    return [];
  }
);

const cartSlice = createSlice({
  name: 'cart',
  initialState: {
    items: [],
    status: 'idle',
    error: null,
  },
  reducers: {
    updateCartPrices: (state, action) => {
      action.payload.forEach(product => {
        const item = state.items.find(cartItem => cartItem.id === product.id);
        if (item && (item.price !== product.price || item.discount !== product.discount)) {
          item.price = product.price;
          item.discount = product.discount;
          const userId = getUserId();
          addItemToShoppingCart(userId, {...item, quantity:0});
        }
      });
    },
  },
  extraReducers: (builder) => {
      builder
        .addCase(fetchCart.pending, (state) => {
          state.status = 'loading';
        })
        .addCase(fetchCart.fulfilled, (state, action) => {
          state.status = 'succeeded';
          state.items = action.payload.items;
        })
        .addCase(fetchCart.rejected, (state, action) => {
          state.status = 'failed';
          state.error = action.error.message;
        })
        .addCase(addToCart.fulfilled, (state, action) => {
          const item = state.items.find((item) => item.id === action.payload.id);
          if (item) {
            item.quantity += action.payload.quantity;
          } else {
            state.items.push(action.payload);
          }
        })
        .addCase(updateCartQuantity.fulfilled, (state, action) => {
          const item = state.items.find((item) => item.id === action.payload.id);
          if (item) {
            item.quantity = action.payload.quantity;
          }
        })
        .addCase(removeFromCart.fulfilled, (state, action) => {
          state.items = state.items.filter((item) => item.id !== action.payload);
        })
        .addCase(clearCart.fulfilled, (state) => {
          state.items = [];
        });
    },
});

export const {  updateCartPrices } = cartSlice.actions;
export default cartSlice.reducer;
