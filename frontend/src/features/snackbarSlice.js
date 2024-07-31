import { createSlice } from '@reduxjs/toolkit';

const snackbarReducer = createSlice({
  name: 'snackbar',
  initialState: {
    open: false,
    message: '',
    title: '',
    severity: 'info', // 'info', 'success', 'warning', 'error'
  },
  reducers: {
    showSnackbar: (state, action) => {
      state.open = true;
      state.message = action.payload.message;
      state.title = action.payload.title;
      state.severity = action.payload.severity;
    },
    hideSnackbar: (state) => {
      state.open = false;
      state.message = '';
      state.title = '';
      state.severity = 'info';
    },
  },
});

export const { showSnackbar, hideSnackbar } = snackbarReducer.actions;

export default snackbarReducer.reducer;
