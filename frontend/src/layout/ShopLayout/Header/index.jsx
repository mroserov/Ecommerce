import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
// material-ui
import { useTheme } from '@mui/material/styles';
import Box from '@mui/material/Box';
import { Badge, IconButton } from '@mui/material';

// project imports
import LogoSection from '../LogoSection';
import SearchSection from './SearchSection';

// assets
import { IconShoppingCart } from '@tabler/icons-react';


const Header = () => {
  const theme = useTheme();
  const items = useSelector((state) => state.cart.items);
  const totalItems = items.reduce((total, item) => total + item.quantity, 0);

  return (
    <>
      {/* logo*/}
      <Box
        sx={{
          width: 228,
          display: 'flex',
          [theme.breakpoints.down('md')]: {
            width: 'auto'
          }
        }}>
        <Box component="span" sx={{ display: {  md: 'block' }, flexGrow: 1 }}>
          <LogoSection />
        </Box>
      </Box>

      

      {/* header search */}
      <SearchSection/>

      <Box sx={{ flexGrow: 1 }} />

      {/* cart icon */}
      <IconButton aria-label="cart" component={Link} to="cart">
        <Badge badgeContent={totalItems} color="primary"><IconShoppingCart /> </Badge>
      </IconButton>
      
    </>
  );
};

Header.propTypes = {
  handleLeftDrawerToggle: PropTypes.func
};

export default Header;
