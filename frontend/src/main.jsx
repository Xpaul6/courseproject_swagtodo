import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'

import './styles/main.css'

import App from './components/App.jsx'
import Error from './components/Error.jsx'
import Register from './components/Register.jsx'
import Login from './components/Login.jsx'
import Parent_menu from './components/Parent_menu.jsx'

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    errorElement: <Error type="incorrect page url"/>, 
  },
  {
    path: '/register',
    element: <Register />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/parent-menu',
    element: <Parent_menu />
  }
])

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>,
)
