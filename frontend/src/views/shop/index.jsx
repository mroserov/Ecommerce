// material-ui
import Grid from '@mui/material/Grid';

// project imports
import { gridSpacing } from 'store/constant';

// assets
import ProductList from './ProductList';

// ==============================|| SHOP PAGE ||============================== //

const Shop = () => {

  return (
    <Grid container spacing={gridSpacing}>
      <Grid item xs={12}>
        <ProductList />
      </Grid>
    </Grid>
  );
};

export default Shop;
