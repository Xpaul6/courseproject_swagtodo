import { useState, useEffect, useContext } from 'react'
import { useNavigate } from "react-router"
import axios from 'axios'

import { DataContext } from '../context/DataContext'

function List_form() {
  const data = useContext(DataContext)
  const navigate = useNavigate()

  const [listName, setListName] = useState("")
  const [childId, setChildId] = useState(0)

  function handleListAdd() {
    const newListInfo = {
      parentId: Number(data.user.id),
      childId: childId,
      title: listName
    }

    axios.post('/api/tasklists', newListInfo, data.headers)
      .then(res => {
        alert('Список создан')
        navigate('/parent-menu')
      })
      .catch(err => alert(err.response.data))
  }

  return (
    <>
      <div className="p-5 w-full">
        <h2 className="text-center">Новый список</h2>
        {/* Main block */}
        <div className="flex sm:flex-row sm:justify-around flex-col mt-10 sm:h-[500px]">
          <div
            className="relative mt-10 sm:mt-0 flex flex-col justify-center items-center p-2 py-10 sm:py-2 sm:pb-20
            sm:w-[400px] border-2 border-blue-400 rounded-md overflow-x-scroll"
          >
            <form action="" className="flex flex-col w-4/5 mx-2">
              <input
                type="text"
                placeholder="Название списка"
                value={listName}
                onChange={(e) => setListName(e.target.value)}
                className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
              />
              <select
                className="mt-5 p-1 rounded-md border border-gray-300 text-center"
                defaultValue={childId}
                onChange={(e) => setChildId(e.target.value)}
              >
                <option value={0} disabled>
                  Выберите ребенка
                </option>
                {data.children.map((child) => (
                  <option key={child.userId} value={child.userId}>
                    {child.name}
                  </option>
                ))}
              </select>
              <button
                className="mt-5 p-2 bg-green-500 text-white sm:bg-transparent sm:text-black border-2 rounded-md
                sm:border-green-500 transition-all ease-in-out transition-150 sm:hover:bg-green-500
                hover:cursor-pointer sm:hover:text-white"
                onClick={(e) => {
                  e.preventDefault()
                  handleListAdd()
                }}
              >
                Создать
              </button>
            </form>
          </div>
        </div>
      </div>
    </>
  )
}

export default List_form