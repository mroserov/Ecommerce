import { createBrowserRouter } from 'react-router-dom';

// routes
import ShopRoutes from './ShopRoutes';

const router = createBrowserRouter([ShopRoutes], {
  basename: import.meta.env.VITE_APP_BASE_NAME
});

export default router;
