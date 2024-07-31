import PropTypes from 'prop-types';
import { useState, forwardRef } from 'react';
import { useDispatch } from 'react-redux';
import { setSearchTerm } from 'features/products/productsSlice';

// material-ui
import { useTheme } from '@mui/material/styles';
import Avatar from '@mui/material/Avatar';
import Box from '@mui/material/Box';
import InputAdornment from '@mui/material/InputAdornment';
import OutlinedInput from '@mui/material/OutlinedInput';

// assets
import { IconSearch, IconX } from '@tabler/icons-react';
import { showSnackbar } from 'features/snackbarSlice';

const HeaderAvatar = forwardRef(({ children, ...others }, ref) => {
  const theme = useTheme();

  return (
    <Avatar
      ref={ref}
      variant="rounded"
      sx={{
        ...theme.typography.commonAvatar,
        ...theme.typography.mediumAvatar,
        bgcolor: 'secondary.light',
        color: 'secondary.dark',
        '&:hover': {
          bgcolor: 'secondary.dark',
          color: 'secondary.light'
        }
      }}
      {...others}
    >
      {children}
    </Avatar>
  );
});

HeaderAvatar.propTypes = {
  children: PropTypes.node
};

const SearchSection = () => {
  const dispatch = useDispatch();
  const [localSearchTerm, setLocalSearchTerm] = useState('');

  const handleSearchChange = (event) => {
    setLocalSearchTerm(event.target.value);
    if (event.target.value.length  === 0) {
      dispatch(setSearchTerm(''));
    }
  };

  const handleSearchClick = () => {
    if (localSearchTerm.length >= 3) {
      dispatch(setSearchTerm(localSearchTerm));
    } else {
      dispatch(showSnackbar({
        title: 'Search',
        message: `Please enter at least 3 characters for the search term.`,
        severity: 'warning',
      }));
    }
  };

  const handleClearClick = () => {
    setLocalSearchTerm("");
    dispatch(setSearchTerm(""));
  };

  return (
    <>
      <Box sx={{ display: {  xs:'none', md: 'block' } }}>
        <OutlinedInput
          id="input-search-header"
          value={localSearchTerm}
          onChange={handleSearchChange}
          placeholder="Search"
          endAdornment={
            <InputAdornment position="end" >
              {localSearchTerm&&
              <HeaderAvatar style={{color:'red'}} onClick={handleClearClick}>
                <IconX stroke={1.5} size="20px" />
              </HeaderAvatar>}
              <HeaderAvatar style={{marginLeft:5}} onClick={handleSearchClick}>
                <IconSearch  stroke={1.8} size="20px" />
              </HeaderAvatar>
            </InputAdornment>
          }
          aria-describedby="search-helper-text"
          inputProps={{ 'aria-label': 'weight', sx: { bgcolor: 'transparent', pl: 0.5 } }}
          sx={{ width: { md: 250, lg: 434 }, ml: 2, px: 2 }}
        />
      </Box>
    </>
  );
};

export default SearchSection;
