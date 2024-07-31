import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { 
  removeFromCart, 
  clearCart, 
  updateCartQuantity, 
  fetchCart 
} from 'features/cart/cartSlice';
import { processOrder } from 'api/services/orderService';
import { showSnackbar } from 'features/snackbarSlice';
import {
  Container,
  Grid,
  Card,
  CardContent,
  Typography,
  Button,
  Box,
  Divider,
  IconButton, Chip,
  TextField
} from '@mui/material';
import QuantitySelector from 'ui-component/QuantitySelector';
import DeleteIcon from '@mui/icons-material/Delete';
import { gridSpacing } from 'store/constant';
import config from 'config';
import CartListSkeleton from './CartListSkeleton';

const CartList = () => {
  const dispatch = useDispatch();
  const items = useSelector((state) => state.cart.items);
  const status = useSelector((state) => state.cart.status);
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [address, setAddress] = useState('');
  const [emailError, setEmailError] = useState(false);
  const [addressError, setAddressError] = useState(false);

  useEffect(() => {
    dispatch(fetchCart());
  }, [dispatch]);

  if (status === 'loading') {
    return <CartListSkeleton />;
  }

  const handleRemove = (id) => {
    dispatch(removeFromCart(id)).then((action)=>{
      console.log(action);
      dispatch(showSnackbar({
        title: 'Remove product',
        message: `Product was removed from your cart.`,
        severity: 'success',
      }));
    });
  };

  const handleClearCart = () => {
    dispatch(clearCart());
  };

  const handleQuantityChange = (product, quantity) => {
    dispatch(updateCartQuantity({ ...product, quantity })).then((action)=>{
      dispatch(showSnackbar({
        title: 'Update cart',
        message: 'Cart was successfully updated',
        severity: 'success',
      }));
    });
  };

  const handleProcessOrder = async () => {
    try {
      let isError=false;
      if (email.length==0){
        setEmailError(true);
        isError=true;
      }
      if (address.length==0){
        setAddressError(true);
        isError=true;
      }
      if (items.length === 0 || isError) {
        dispatch(showSnackbar({
          title: 'Order data incomplete',
          message: `You must enter your email and address to process the order..`,
          severity: 'error',
        }));
        return;
      }
      
      const response = await processOrder({
        orderItems: items.map(item => ({
          ...item, productId: item.id
        })),
        email,
        address
      });

      if (response.status === 200) {
        dispatch(showSnackbar({
          title: 'Order complete',
          message: `Order processed successfully`,
          severity: 'success',
        }));
        handleClearCart();
        setEmail('');
        setAddress('');
        navigate(config.basename);
      }else{
        dispatch(showSnackbar({
          title: 'Error',
          message: `Error processing order ${response.message}`,
          severity: 'error',
        }));
      }
    } catch (error) {
      dispatch(showSnackbar({
        title: 'Error',
        message: `Error processing order ${error}`,
        severity: 'error',
      }));
      console.error('Error processing order:', error);
    }
  };

  const subtotal = items.reduce((total, item) => total + (item.price * item.quantity), 0)
  const discount = items.reduce((total, item) => total + ((item.price * (item.discount / 100)) * item.quantity), 0);
  const total = subtotal - discount;

  return (
    <Container>
      <Typography variant="h2" gutterBottom>
        My shopping cart
      </Typography>
      <Grid container marginTop={1} spacing={gridSpacing}>
        <Grid item xs={12} md={8}>
          {items.length === 0 ? (
            <>
              <Grid item xs={6}>
                <Typography variant="body2" color="text.secondary">
                  Your cart is empty.
                </Typography>
                <Button
                  variant="contained"
                  color="primary"
                  fullWidth
                  component={Link}
                  to={config.defaultPath}
                >
                  Go Home
                </Button>
              </Grid>
            </>
          ) : (
            <>
              <Box mb={2}>
                <Grid container spacing={gridSpacing} alignItems="center">
                  <Grid item xs={2}>
                    <Typography variant="h4" fontWeight="bold">Product</Typography>
                  </Grid>
                  <Grid item xs={4}></Grid>
                  <Grid item xs={4}>
                    <Typography variant="h4" fontWeight="bold">Quantity</Typography>
                  </Grid>
                  <Grid item xs={2}>
                    <Typography variant="h4" fontWeight="bold">Total</Typography>
                  </Grid>
                </Grid>
              </Box>
              {items.map((item) => (
                <Card key={item.id} style={{ marginBottom: '20px' }}>
                  <CardContent>
                    <Grid container spacing={2} alignItems="center">
                      <Grid item xs={2} sx={{ display: {  xs:'none', md: 'block' } }}>{item.discount > 0 && (
                        <Chip
                          label={`${item.discount}%`}
                          color="secondary"
                          style={{
                            position: 'absolute',
                            margin: '0px',
                            backgroundColor: '#9c27b0',
                            color: 'white',
                          }}
                        />
                      )}
                        <img src={item.imageUrl} alt={item.name} style={{ width: '100%' }} />
                      </Grid>
                      <Grid item xs={4}>
                        <Typography variant="h4">{item.name} -{item.stock}</Typography>
                        <Typography variant="body1">Code: {item.slug}</Typography>
                        {(item.stock - item.quantity) > 0 ? (
                          <Typography variant="body2" color="text.secondary">
                            Stock: {item.stock - item.quantity}
                          </Typography>
                        ) : (
                          <Typography variant="body2" color="error">
                            Out of stock
                          </Typography>
                        )}
                        <IconButton color="primary" onClick={() => handleRemove(item.id)}>
                          <DeleteIcon />
                        </IconButton>
                        <Button color="primary" onClick={() => handleRemove(item.id)}>
                          Delete
                        </Button>
                      </Grid>
                      <Grid item xs={4}>
                        <QuantitySelector
                          value={item.quantity}
                          onChange={(value) => handleQuantityChange(item, value)}
                          min={1}
                          max={item.stock}
                          inputDisable={true}
                        />
                      </Grid>
                      <Grid item xs={2}>
                        <Typography variant="h4">${(item.price * item.quantity).toFixed(2)}</Typography>
                      </Grid>
                    </Grid>
                  </CardContent>
                </Card>
              ))}
              <Button
                variant="contained"
                color="error"
                fullWidth
                onClick={handleClearCart}
              >
                Clear Cart
              </Button>
            </>
          )}
        </Grid>
        <Grid item xs={12} md={4}>
          <Typography variant="h3" gutterBottom>
            Shopping cart details
          </Typography>
          <Card>
            <CardContent>
              <Box mt={2} mb={2}>
                <Grid container spacing={2}>
                  <Grid item xs={6}>
                    <Typography variant="body2">Sub Total ({items.reduce((total, item) => total + item.quantity, 0)} units)</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="body2" align="right">${subtotal.toFixed(2)}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="body2">Discount</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="body2" align="right" color="error">-${discount.toFixed(2)}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="h5">Total</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="h5" align="right">${total.toFixed(2)}</Typography>
                  </Grid>
                </Grid>
              </Box><Divider />
              <Box mt={3} mb={2}>
                <Typography variant="h4" gutterBottom>
                  User data
                </Typography>
                <Grid container spacing={2} mt={2}>
                  <Grid item xs={12}>
                    <TextField
                      error={emailError}
                      fullWidth required email
                      label="Email"
                      type="text"
                      value={email}
                      onChange={(e) => {setEmail(e.target.value); setEmailError(e.target.value.length==0)}}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <TextField
                      error={addressError}
                      fullWidth required
                      label="Address"
                      type="text"
                      value={address}
                      onChange={(e) => {setAddress(e.target.value); setAddressError(e.target.value.length==0)}}
                    />
                  </Grid>
                </Grid></Box>
              <Button
                variant="contained"
                color="primary"
                fullWidth
                disabled={items.length === 0}
                onClick={handleProcessOrder}
              >
                Process Order
              </Button>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Container>
  );
};

export default CartList;
