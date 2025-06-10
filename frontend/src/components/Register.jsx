import { useState } from 'react'
import { useNavigate } from 'react-router'
import axios from 'axios'

import '../styles/main.css'

function Register() {
  const navigate = useNavigate()
  const [accountType, SetAccountType] = useState("")
  const [name, setName] = useState("")
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")


  function handleRegister() {
    const userInfo = {
      name: name,
      email: email,
      password: password,
      isParent: accountType == 'parent' ? true : false
    }
    
    axios.post('/api/register', userInfo)
      .then(res => {
        console.log("register success")
        localStorage.setItem('role', accountType)
        navigate('/login')
      })
      .catch(err => alert(err.response.data))
  }

  return (
    <>
      <div className="pagebox">
        <h2>Регистрация</h2>
        <form action="" className="flex flex-col mt-5">
          <input
            type="text"
            placeholder="Имя"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <input
            type="email"
            placeholder="Эл. почта"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <div className="mt-5">
            <input type="radio" id="parent" name="role" value="parent" onChange={(e) => SetAccountType(e.target.value)} />
            <label htmlFor="parent" className="ml-2">
              Родитель
            </label>
          </div>
          <div className="mt-2">
            <input type="radio" id="child" name="role" value="child" onChange={(e) => SetAccountType(e.target.value)} />
            <label htmlFor="child" className="ml-2">
              Ребенок
            </label>
          </div>
          <input
            type="password"
            placeholder="Пароль"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <button
            className="mt-5 p-2 bg-green-500 text-white sm:bg-transparent sm:text-black border-2 rounded-md
              sm:border-green-500 transition-all ease-in-out transition-15 sm:hover:bg-green-500
                hover:cursor-pointer sm:hover:text-white"
            onClick={(e) => {
              e.preventDefault()
              handleRegister()
            }}
          >
            Зарегистрироваться
          </button>
        </form>
      </div>
    </>
  )
}

export default Register