import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'

import './styles/main.css'

import App from './components/App.jsx'

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    error: <div>404 not found</div>
  },
])

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>,
)
