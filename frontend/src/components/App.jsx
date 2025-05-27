import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'

import '../styles/main.css'

function App() {
  const [apiText, setApiText] = useState('defaultApiText');

  // async function getApiText() {
  //   await fetch('/api/hello')
  //     .then((res) => res.json())
  //     .then((data) => {setApiText(data.message)})
  //     .catch((err) => console.log('Error: ' + err.message))
  // }

  // useEffect(getApiText, [])

  return (
    <>
      <div className="pagebox">
        <h2>SWAG ToDo</h2>
        <p>{apiText}</p>
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
