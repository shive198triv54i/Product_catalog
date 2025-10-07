// import React from 'react';
// import { useSelector, useDispatch } from 'react-redux';
// import type { RootState, AppDispatch } from '../app/store';
// import { removeFromCart, clearCart } from '../redux/features/cart/cartSlice';

// const CartPage: React.FC = () => {
//   const dispatch: AppDispatch = useDispatch();
//   const { items, totalPrice } = useSelector((state: RootState) => state.cart);

//   if (!items.length) return <p>Your cart is empty.</p>;

//   return (
//     <div>
//       <h1>Cart</h1>
//       <ul>
//         {items.map(item => (
//           <li key={item.product.id}>
//             {item.product.name} x {item.quantity} = ₹{item.product.price * item.quantity}
//             <button onClick={() => dispatch(removeFromCart(item.product.id))}>Remove</button>
//           </li>
//         ))}
//       </ul>
//       <p>Total: ₹{totalPrice}</p>
//       <button onClick={() => dispatch(clearCart())}>Clear Cart</button>
//     </div>
//   );
// };

// export default CartPage;