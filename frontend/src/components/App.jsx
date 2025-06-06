import { useState, useEffect } from 'react'
import { Link, useNavigate } from 'react-router-dom'

import '../styles/main.css'

function App() {
  const navigate = useNavigate()

  const role = localStorage.getItem('role')
  const token = localStorage.getItem('token')

  useEffect(() => {
    if (token != '' && role != '') {
      navigate(`/${role}-menu`)
    }
  }, [navigate])

  return (
    <>
      <div className="pagebox">
        <h2>SWAG ToDo</h2>
        <div className="flex flex-col items-center mt-4">
          <Link to="/register" className="link">
            Регистрация
          </Link>
          <Link to="/login" className="link">
            Войти как родитель
          </Link>
          <Link to="/login" className="link">
            Войти как ребенок
          </Link>
        </div>
      </div>
    </>
  )
}

export default App
