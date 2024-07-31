import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Snackbar as MuiSnackbar, Alert, AlertTitle } from '@mui/material';
import { hideSnackbar } from 'features/snackbarSlice';

const Snackbar = () => {
  const dispatch = useDispatch();
  const { open, message, title, severity } = useSelector((state) => state.snackbar);

  const handleClose = (event, reason) => {
    if (reason === 'clickaway') {
      return;
    }
    dispatch(hideSnackbar());
  };

  return (
    <MuiSnackbar
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      open={open}
      autoHideDuration={10000}
      onClose={handleClose}
    >
      <Alert onClose={handleClose} severity={severity}>
        {title && <AlertTitle>{title}</AlertTitle>}
        {message}
      </Alert>
    </MuiSnackbar>
  );
};

export default Snackbar;
