import { lazy } from 'react';

// project imports
import ShopLayout from 'layout/ShopLayout';
import Loadable from 'ui-component/Loadable';

// Shop routing
const ShopDefault = Loadable(lazy(() => import('views/shop')));

// Cart routing
const CartDefault = Loadable(lazy(() => import('views/cart')));


const ShopRoutes = {
  path: '/',
  element: <ShopLayout />,
  children: [
    {
      path: '/',
      element: <ShopDefault />
    },
    {
      path: 'cart',
      element: <CartDefault />
    }
  ]
};

export default ShopRoutes;
