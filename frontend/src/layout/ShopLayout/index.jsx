import { Outlet } from 'react-router-dom';

// material-ui
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';

// project imports
import { styled, useTheme } from '@mui/material';
import Header from './Header';

// assets

const Main = styled('main', { shouldForwardProp: (prop) => prop !== 'open' && prop !== 'theme' })(({ theme, open }) => ({
  ...theme.typography.mainContent,
  borderBottomLeftRadius: 0,
  borderBottomRightRadius: 0,
}));

const ShopLayout = () => {
  const theme = useTheme();


  return (
    <Box sx={{ display: 'flex' }}>
      {/* header */}
      <AppBar
        enableColorOnDark
        position="fixed"
        color="inherit"
        elevation={0}
      >
        <Toolbar>
          <Header/>
        </Toolbar>
      </AppBar>

      {/* main content */}
      <Main theme={theme} >
        <Outlet />
      </Main>
    </Box>
  );
};

export default ShopLayout;
