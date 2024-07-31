// src/components/QuantitySelector.jsx
import React from 'react';
import { Button, TextField, Box, Typography } from '@mui/material';

const QuantitySelector = ({ value, onChange, min, max, inputDisable = false }) => {
  const handleIncrease = () => {
    if (value < max) {
      onChange(value + 1);
    }
  };

  const handleDecrease = () => {
    if (value > min) {
      onChange(value - 1);
    }
  };

  const handleInputChange = (e) => {
    const newValue = parseInt(e.target.value);
    if (!isNaN(newValue) && newValue >= min) {
      if (newValue > max) {
        onChange(max);
      } else
        onChange(newValue);
    }
  };

  return (
    <Box display="flex" alignItems="center">
      <Button
        variant="outlined"
        size="small"
        onClick={handleDecrease}
        disabled={value <= min}
        style={{ minWidth: '40px', height: '40px', fontSize: '30px' }}>-</Button>
        <TextField
          value={value}
          onChange={handleInputChange}
          inputProps={{ min, max }}
          disabled={inputDisable}
          size="small"
          style={{ width: '50px', margin: '0 3px 0 3px', textAlign: 'center' }} />
      <Button
        variant="outlined"
        size="small"
        onClick={handleIncrease}
        disabled={value >= max}
        style={{ minWidth: '40px', height: '40px', fontSize: '20px' }}>+</Button>
    </Box>
  );
};

export default QuantitySelector;
