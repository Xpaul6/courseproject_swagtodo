import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'

function App() {
  const [apiText, setApiText] = useState('defaultText');

  // async function getApiText() {
  //   await fetch('/api/hello')
  //     .then((res) => res.json())
  //     .then((data) => {setApiText(data.message)})
  //     .catch((err) => 'Error: ' + err.message)
  // }

  // useEffect(getApiText, [])

  return (
    <>
      <div className="flex flex-col items-center w-full p-6 mt-50 sm:border-2 border-blue-400 rounded-xl">
        <h2>SWAG ToDo</h2>
        <p>{apiText}</p>
        <div className="flex flex-col items-center mt-4">
          <Link
            to="/a"
            className="text-xl text-blue-600 text-center w-full py-4 px-6 my-2 rounded-xl hover:bg-gray-100
              hover:cursor-pointer hover:underline transition ease duration-150"
          >
            Регистрация
          </Link>
          <Link
            to="/a"
            className="text-xl text-blue-600 text-center w-full py-4 px-6 my-2 rounded-xl hover:bg-gray-100
              hover:cursor-pointer hover:underline transition ease duration-150"
          >
            Войти как родитель
          </Link>
          <Link
            to="/a"
            className="text-xl text-blue-600 text-center w-full py-4 px-6 my-2 rounded-xl hover:bg-gray-100
              hover:cursor-pointer hover:underline transition ease duration-150"
          >
            Войти как ребенок
          </Link>
        </div>
      </div>
    </>
  );
}

export default App
