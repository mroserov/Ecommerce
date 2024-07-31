import { RouterProvider } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import { CssBaseline, StyledEngineProvider } from '@mui/material';

import Snackbar from './ui-component/Snackbar';
// routing
import router from 'routes';

// defaultTheme
import themes from 'themes';

// project imports
import NavigationScroll from 'layout/NavigationScroll';

const App = () => {

  return (
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={themes()}>
        <CssBaseline />
          <Snackbar/>
          <RouterProvider router={router} />
      </ThemeProvider>
    </StyledEngineProvider>
  );
};

export default App;
