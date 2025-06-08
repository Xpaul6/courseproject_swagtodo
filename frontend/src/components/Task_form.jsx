import { useState, useEffect, useContext } from 'react'
import { useNavigate } from "react-router"
import axios from 'axios'

import { DataContext } from '../context/DataContext'

function Task_form({type}) {
  const data = useContext(DataContext)
  const navigate = useNavigate()

  const [description, setDescription] = useState(type == 'new' ? "" : data.currentTask.description)
  const [childId, setChildId] = useState(type == 'new' ? 0 : data.currentTask.childId)
  const [reward, setReward] = useState(type == 'new' ? "" : data.currentTask.reward)
  const [deadline, setDeadline] = useState(type == 'new' ? new Date : new Date(data.currentTask.deadline))

  function handleTaskAdd() {
    const newTaskInfo = {
      parentId: Number(data.user.id),
      childId: Number(childId),
      taskListId: data.currentList.id,
      description: description,
      deadline: (new Date(deadline)).toISOString(),
      reward: reward,
      status: "ongoing"
    }

    axios.post('/api/tasks', newTaskInfo, data.headers)
      .then(res => {
        alert('Успешное добавление')
        navigate('/parent-menu')
      })
      .catch(err => err.response.data)
  }

  function handleTaskEdit() {
    const newTaskInfo = {
      taskId: data.currentTask.taskId,
      parentId: Number(data.user.id),
      childId: Number(childId),
      taskListId: data.currentTask.taskListId,
      description: description,
      deadline: (new Date(deadline)).toISOString(),
      reward: reward,
      status: "ongoing",
      createdAt: data.currentTask.createdAt
    }

    axios.put(`/api/tasks/${data.currentTask.taskId}`, newTaskInfo, data.headers)
      .then(res => {
        alert('Успешное изменение')
        navigate('/parent-menu')
      })
      .catch(err => err.response.data)
  }

  useEffect(() => {
    console.log(data) // debug
    if (data.currentList.id == 0) {
      navigate('/parent-menu')
    }
  }, [data, navigate])

  return (
    <>
      <div className="p-5 w-full">
        <h2 className="text-center">{type == 'new' ? "Новая задача" : "Изменить задачу"}</h2>
        {/* Main block */}
        <div className="flex sm:flex-row sm:justify-around flex-col mt-10 sm:h-[500px]">
          <div
            className="relative mt-10 sm:mt-0 flex flex-col justify-center items-center p-2 py-10 sm:py-2 sm:pb-20
            sm:w-[400px] border-2 border-blue-400 rounded-md overflow-x-scroll"
          >
            <form action="" className="flex flex-col w-4/5 mx-2">
              <h3 className="sticky top-0 text-center py-2 pb-2 border-b-1 border-blue-200 bg-white">
                {data.currentList.title}
              </h3>
              <input
                type="text"
                placeholder="Описание задания"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
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
              <input
                type="text"
                placeholder="Награда"
                value={reward}
                onChange={(e) => setReward(e.target.value)}
                className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
              />
              <input
                type="date"
                value={deadline}
                onChange={(e) => setDeadline(e.target.value)}
                className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
              />
              <button
                className="mt-5 p-2 bg-green-500 text-white sm:bg-transparent sm:text-black border-2 rounded-md
                sm:border-green-500 transition-all ease-in-out transition-150 sm:hover:bg-green-500
                hover:cursor-pointer sm:hover:text-white"
                onClick={(e) => {
                  e.preventDefault()
                  type == 'new' ? handleTaskAdd() : handleTaskEdit()
                }}
              >
                {type == 'new' ? "Создать" : "Изменить"}
              </button>
            </form>
          </div>
        </div>
      </div>
    </>
  )
}

export default Task_form