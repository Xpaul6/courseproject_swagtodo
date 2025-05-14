import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'

import './styles/main.css'

import { GuideProvider } from './context/GuideContext.jsx'
import Docs from './components/Docs.jsx'

import App from './components/App.jsx'
import Error from './components/Error.jsx'
import Guide from './components/Guide.jsx'
import Register from './components/Register.jsx'
import Login from './components/Login.jsx'
import Parent_menu from './components/Parent_menu.jsx'

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    errorElement: <Error type=""/>, 
  },
  {
    path: '/guide',
    element: <Guide />
  },
  {
    path: '/docs',
    element: <Docs />
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
    <GuideProvider>
      <RouterProvider router={router} />
    </GuideProvider>
  </StrictMode>,
)
