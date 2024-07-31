import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setPage, updateStock } from 'features/products/productsSlice';
import { addToCart, updateCartPrices, fetchCart } from 'features/cart/cartSlice';
import { fetchProducts } from 'features/products/productsApi';
import { showSnackbar } from 'features/snackbarSlice';
import { getProductById } from 'api/services/catalogService';
import {
  Grid,
  Card,
  CardMedia,
  CardContent,
  Typography,
  Button,
  Skeleton,
  Pagination,
  Chip,
} from '@mui/material';

import { gridSpacing } from 'store/constant';
import QuantitySelector from 'ui-component/QuantitySelector';
import CartEmpty from 'assets/images/empty-cart.svg';
import config from 'config';

const ProductList = () => {
  const dispatch = useDispatch();
  const { items, status, error, totalPages, currentPage, totalCount, searchTerm } = useSelector(
    (state) => state.products
  );
  // Sync cart items with Products
  const cartItems = useSelector((state) => state.cart.items);

  useEffect(() => {
    dispatch(fetchProducts({ page: currentPage, pageSize: config.pageSize, searchTerm })).then((action) => {
      if (action.type === 'products/fetchProducts/fulfilled') {
        dispatch(updateCartPrices(action.payload.items));
      }
    });
    //dispatch(fetchCart());
    dispatch(fetchCart()).then((action)=>{
      if (action.type === 'cart/fetchCart/fulfilled') {
        dispatch(updateStock({ products: action.payload.items }));
      }
    });
  }, [dispatch, currentPage, searchTerm, totalPages]);

  const handlePageChange = (event, value) => {
    dispatch(setPage(value));
  };

  // Quantities of products to add a cart
  const [quantities, setQuantities] = useState({});

  // Update quantities based on cart items
  useEffect(() => {
    const initialQuantities = {};
    items.forEach(item => {
      const cartItem = cartItems.find(cartItem => cartItem.id === item.id);
      const quantity = cartItem ? cartItem.quantity : 0;
      initialQuantities[item.id] = quantity;
    });
    //setQuantities(initialQuantities);
  }, [items, cartItems]);

  // Handle when a quantity of change
  const handleQuantityChange = (id, value) => {
    setQuantities((prev) => ({
      ...prev,
      [id]: value,
    }));
  };

  const handleAddToCart = async (product) => {
    const response = await getProductById(product.id);
    const currentProductInCart = cartItems.find(cartItem => cartItem.id === product.id);
    const productStock = response.data.productById.stock - (currentProductInCart?.quantity || 0);
    const quantity = quantities[product.id] || 1;

    if (quantity > productStock) {
      dispatch(showSnackbar({
        title: 'Out of Stock',
        message: `The product ${product.name} is out of stock.`,
        severity: 'error',
      }));
      setQuantities((prev) => ({
        ...prev,
        [product.id]: 1,
      }));
    } else {
      // New quantity in inpu must be less than the stock
      const newQuantity = (productStock - quantity) > quantity ? quantity:(productStock - quantity) ;
      setQuantities((prev) => ({
        ...prev,
        [product.id]: newQuantity,
      }));
      dispatch(addToCart({ ...product, quantity }));
    }
    const products = [{ id: product.id, quantity }];
    dispatch(updateStock({ products: products }));
  };

  return (
    <Grid container spacing={gridSpacing}>
      <Grid item xs={12}>
        <Typography variant="h4" gutterBottom>
          Product Catalog - {totalCount} Products
        </Typography>
        {searchTerm && (
          <Typography variant="subtitle1" gutterBottom>
            {totalCount} Products - Search results for: "{searchTerm}"
          </Typography>
        )}
      </Grid>
      {status === 'loading' && (
        <Grid item xs={12}>
          <Grid container spacing={gridSpacing}>
            {[1, 2, 3, 4, 5, 6].map((i) => (
              <Grid key={i} item xs={12} sm={6} md={4}>
                <Card>
                  <CardContent>
                    <Skeleton variant="rectangular" height={118} />
                    <Skeleton />
                    <Skeleton width="60%" />
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Grid>
      )}
      {status === 'failed' && (
        <Grid item xs={12}>
          <Grid item textAlign="center">
            <img
              src={CartEmpty}
              alt="Empty"
              width={100}
              style={{ color: 'gray' }} />
            <Typography variant="h4" component="div">
              No products found. [{error}]
            </Typography>
          </Grid>
        </Grid>
      )}
      {status === 'succeeded' && items && items.length > 0 && (
        <Grid item xs={12}>
          <Grid container spacing={2}>
            {items.map((product) => (
              <Grid item key={product?.id} xs={12} sm={6} md={4} lg={3}>
                <Card variant="outlined">
                  {product.discount > 0 && (
                    <Chip
                      label={`${product.discount}% off`}
                      color="secondary"
                      style={{
                        position: 'absolute',
                        margin: '10px',
                        backgroundColor: '#9c27b0',
                        color: 'white',
                      }}
                    />
                  )}
                  <CardMedia
                    sx={{ height: 250 }}
                    component="img"
                    image={product.imageUrl}
                    alt={product.name}
                  />
                  <CardContent>
                    <Typography variant="h4" component="div">
                      {product.name}
                    </Typography>
                    <Typography variant="h4" color="text.secondary">
                      ${product.price}
                    </Typography>
                    <Typography variant="body1" color="text.primary">
                      {product.description}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Code: {product.slug}
                    </Typography>
                    {product.stock > 0 ? (
                      <Typography variant="body2" color="text.secondary">
                        Stock: {product.stock}
                      </Typography>
                    ) : (
                      <Typography variant="body2" color="error">
                        Out of stock
                      </Typography>
                    )}
                    <Grid container spacing={2} >
                      <Grid item>
                        <QuantitySelector
                          value={quantities[product.id] || 1}
                          onChange={(value) => handleQuantityChange(product.id, value)}
                          min={1}
                          max={product.stock} />
                      </Grid>
                      <Grid item xs={12} sm={12} md={12}>
                        <Button
                          variant="contained"
                          color="primary"
                          fullWidth
                          style={{ height: '40px' }}
                          disabled={product.stock === 0}
                          onClick={() => handleAddToCart(product)}> Add To Cart </Button>
                      </Grid>
                    </Grid>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Grid>
      )}
      {status === 'succeeded' && items && items.length == 0 && (
        <Grid item xs={12}>
          <Grid item textAlign="center">
            <img src={CartEmpty} alt="Empty" width={100} style={{ color: 'gray' }} />
            <Typography variant="h4" component="div">
              No products found.
            </Typography>
          </Grid>
        </Grid>
      )}
      <Grid item xs={12}>
        <Pagination showFirstButton showLastButton
          variant="outlined"
          shape="rounded"
          count={totalPages}
          page={currentPage}
          onChange={handlePageChange}
          color="primary"
          style={{ marginTop: '20px' }} />
      </Grid>
    </Grid>
  );
};

export default ProductList;
