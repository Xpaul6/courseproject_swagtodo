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
      <div className="flex flex-col items-center w-full py-6 px-10 mt-50 sm:border-2 border-blue-400 rounded-xl">
        <h2>SWAG ToDo</h2>
        <p>{apiText}</p>
        <div className="flex flex-col items-center mt-4">
          <Link to="/a" className="link">
            Регистрация
          </Link>
          <Link to="/a" className="link">
            Войти как родитель
          </Link>
          <Link to="/a" className="link">
            Войти как ребенок
          </Link>
        </div>
      </div>
    </>
  );
}

export default App
