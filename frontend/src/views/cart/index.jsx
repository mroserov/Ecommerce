// material-ui
import Grid from '@mui/material/Grid';

// project imports
import { gridSpacing } from 'store/constant';

// assets
import CartList from './CartList';

const Cart = () => {

  return (
    <Grid container spacing={gridSpacing}>
      <Grid item xs={12}>
        <CartList />
      </Grid>
    </Grid>
  );
};

export default Cart;
