import { createContext, useEffect } from 'react'

const defaultData = {
  user: {
    userid: 0,
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
    createdat: "undefined"
  },
  // currentList: {TBD}
  tasks: [],
  lists: []
}

export const DataContext = createContext(defaultData)

export default function DataContextProvider({ children }) {
  // useEffect(() => console.log(defaultData), [])

  return (
    <DataContext.Provider value={defaultData}>
      {children}
    </DataContext.Provider>
  )
}
