import { useState } from 'react'
import { useNavigate } from 'react-router'

import '../styles/main.css'

function Register() {
  const navigate = useNavigate()
  const [accountType, SetAccountType] = useState("");

  function Register() {
    // TODO:
    accountType == "parent" ? navigate('/parent-menu') : navigate('/child-menu') 
  }
  return (
    <>
      <div className="pagebox">
        <h2>Регистрация</h2>
        <form action="" className="flex flex-col mt-5">
          <input
            type="text"
            placeholder="Имя"
            className="p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <input
            type="email"
            placeholder="Эл. почта"
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <div className="mt-5">
            <input type="radio" id="parent" name="role" value="parent" onChangeCapture={(e) => SetAccountType(e.target.value)} />
            <label htmlFor="parent" className="ml-2">
              Родитель
            </label>
          </div>
          <div className="mt-2">
            <input type="radio" id="child" name="role" value="child" />
            <label htmlFor="child" className="ml-2">
              Ребенок
            </label>
          </div>
          <input
            type="passsword"
            placeholder="Пароль"
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <button
            className="mt-5 p-2 bg-green-500 text-white sm:bg-transparent sm:text-black border-2 rounded-md
              sm:border-green-500 transition-all ease-in-out transition-15 sm:hover:bg-green-500
                hover:cursor-pointer sm:hover:text-white"
            onClick={(e) => {
              e.preventDefault()
              Register()
            }}
          >
            Зарегистрироваться
          </button>
        </form>
      </div>
    </>
  );
}

export default Register;