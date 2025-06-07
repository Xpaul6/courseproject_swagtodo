import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'

import './styles/main.css'

import { GuideProvider } from './context/GuideContext.jsx'
import Docs from './components/Docs.jsx'
import Guide from './components/Guide.jsx'

import App from './components/App.jsx'
import Error from './components/Error.jsx'
import Register from './components/Register.jsx'
import Login from './components/Login.jsx'
import Parent_menu from './components/Parent_menu.jsx'
import Child_menu from './components/Child_menu.jsx'
import New_task from './components/New_task.jsx'
import Add_child from './components/Add_child.jsx'

import DataContextProvider from './context/DataContext.jsx'
import { PrivateRoute } from './components/PrivateRoute.jsx'

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
    element: (
      <PrivateRoute>
        <Parent_menu />
      </PrivateRoute>
    ) 
  },
  {
    path: '/child-menu',
    element: (
      <PrivateRoute>
        <Child_menu />
      </PrivateRoute>
    )
  },
  {
    path: '/new-task',
    element: (
      <PrivateRoute>
        <New_task />
      </PrivateRoute>
    )
  },
  {
    path: '/add-child',
    element: (
      <PrivateRoute>
        <Add_child />
      </PrivateRoute>
    )
  }
])

createRoot(document.getElementById('root')).render(
  // <StrictMode>
    <DataContextProvider>
      <GuideProvider>
        <RouterProvider router={router} />
      </GuideProvider>
    </DataContextProvider>
  // </StrictMode>,
)
