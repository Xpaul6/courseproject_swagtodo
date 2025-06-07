import { useState, useContext } from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'

import '../styles/main.css'

import { DataContext } from '../context/DataContext'

function Login() {
  const navigate = useNavigate()

  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")

  const data = useContext(DataContext)

  function handleLogin() {
    const loginInfo = {
      email: email,
      password: password
    }

    axios.post('/api/login', loginInfo)
      .then(res => {
        console.log('login success')
        localStorage.setItem('token', res.data.token)
        localStorage.setItem('role', res.data.role)
        localStorage.setItem('id', res.data.userId)
        data.user.role = res.data.role
        data.user.id = res.data.userId
        navigate(res.data.role == 'parent' ? navigate('/parent-menu') : navigate('/child-menu'))
      })
      .catch(err => alert("Incorrect email or password"))
  }

  return (
    <>
      <div className="pagebox">
        <h2>Вход</h2>
        <form action="" className="flex flex-col mt-5">
          <input
            type="email"
            placeholder="Эл. почта"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <input
            type="password"
            placeholder="Пароль"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <button
            className="mt-5 p-2 bg-green-500 text-white sm:bg-transparent sm:text-black border-2 rounded-md
              sm:border-green-500 transition-all ease-in-out transition-150 sm:hover:bg-green-500
              hover:cursor-pointer sm:hover:text-white"
            onClick={(e) => {
              e.preventDefault()
              handleLogin()
            }}
          >
            Войти
          </button>
        </form>
      </div>
    </>
  );
}

export default Login