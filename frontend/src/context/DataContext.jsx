import { jwtDecode } from 'jwt-decode'

import { createContext, useEffect } from 'react'

const defaultData = {
  headers: {
    headers: {
      Authorization: "Bearer " + localStorage.getItem('token')
    }
  },
  isUpdated: false,
  user: {
    id: 0,
    name: "undefined",
    email: "undefined",
    password: "undefined",
    role: "undefined",
    createdAt: "undefined"
  },
  currentTask: {
    id: 0,
    parentid: 0,
    childid: 0,
    description: "undefined",
    deadline: "undefined",
    reward: "undefined",
    status: "undefined",
    createdate: "undefined"
  },
  currentList: {
    id: 0,
    parent_id: 0,
    child_id: 0,
    title: "Выберите список",
    createdate: "undefined"
  },
  familycode: "undefined",
  children: [],
  tasks: [],
  lists: []
}

function isTokenExpired(token) {
  if (!token) return true
  
  try {
    const decoded = jwtDecode(token)
    const currentTime = Date.now() / 1000
    return decoded.exp < currentTime
  } catch (error) {
    console.error('Ошибка декодирования токена:', error)
    localStorage.setItem('token', '')
    return true
  }
};

async function AuthCheck() {
  const token = localStorage.getItem('token')
  if (isTokenExpired(token)) {
    localStorage.setItem('token', '')
  }
}

export const DataContext = createContext(defaultData)

export default function DataContextProvider({ children }) {
  useEffect(() => {
    console.log('auth check') // debug
    AuthCheck()
  }, [])

  return (
    <DataContext.Provider value={defaultData}>
      {children}
    </DataContext.Provider>
  )
}
