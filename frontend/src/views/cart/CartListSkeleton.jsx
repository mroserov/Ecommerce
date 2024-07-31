import React from 'react';
import { Skeleton } from '@mui/material';
import {
  Container,
  Grid,
  Card,
  CardContent,
  Box,
  Typography,
  Button,
  Divider,
} from '@mui/material';
import { gridSpacing } from 'store/constant';

const CartListSkeleton = () => {
  return (
    <Container>
      <Typography variant="h2" gutterBottom>
        <Skeleton width="50%" />
      </Typography>
      <Grid container marginTop={1} spacing={gridSpacing}>
        <Grid item xs={12} md={8}>
          {[...Array(3)].map((_, index) => (
            <Card key={index} style={{ marginBottom: '20px' }}>
              <CardContent>
                <Grid container spacing={2} alignItems="center">
                  <Grid item xs={2}>
                    <Skeleton variant="rectangular" width="100%" height={100} />
                  </Grid>
                  <Grid item xs={4}>
                    <Skeleton width="80%" />
                    <Skeleton width="60%" />
                    <Skeleton width="40%" />
                    <Box mt={2}>
                      <Skeleton variant="rectangular" width={80} height={36} />
                    </Box>
                  </Grid>
                  <Grid item xs={3}>
                    <Skeleton variant="rectangular" width="60%" height={36} />
                  </Grid>
                  <Grid item xs={3}>
                    <Skeleton width="60%" />
                  </Grid>
                </Grid>
              </CardContent>
            </Card>
          ))}
          <Box mt={2}>
            <Skeleton variant="rectangular" width="100%" height={56} />
          </Box>
        </Grid>
        <Grid item xs={12} md={4}>
          <Typography variant="h3" gutterBottom>
            <Skeleton width="50%" />
          </Typography>
          <Card>
            <CardContent>
              <Box mt={2} mb={2}>
                <Grid container spacing={2}>
                  <Grid item xs={6}>
                    <Skeleton width="80%" />
                  </Grid>
                  <Grid item xs={6}>
                    <Skeleton width="50%" align="right" />
                  </Grid>
                  <Grid item xs={6}>
                    <Skeleton width="80%" />
                  </Grid>
                  <Grid item xs={6}>
                    <Skeleton width="50%" align="right" />
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="h5">
                      <Skeleton width="80%" />
                    </Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Divider />
                    <Typography variant="h5" align="right">
                      <Skeleton width="50%" />
                    </Typography>
                  </Grid>
                </Grid>
              </Box>
              <Skeleton variant="rectangular" width="100%" height={56} />
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Container>
  );
};

export default CartListSkeleton;
