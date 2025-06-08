import { Navigate } from 'react-router-dom'

export function PrivateRoute({ children, roles }) {
  const isAuthenticated = localStorage.getItem('token')
  const userRole = localStorage.getItem('role')

  if (!isAuthenticated) {
    return <Navigate to="/register" replace />
  }

  if (roles && !roles.includes(userRole)) {
    return <Navigate to="/login" />
  }

  return children
}