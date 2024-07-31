// material-ui
import { useTheme } from '@mui/material/styles';

import logo from 'assets/images/logo.svg';
const Logo = () => {
  const theme = useTheme();

  return (
    <img src={logo} alt="eShop" width="200" />
    
  );
};

export default Logo;
