import { useState, useEffect, useContext } from 'react'
import axios from 'axios'

import { DataContext } from '../context/DataContext'

import Child_task from './blocks/Child_task'
import Profile from './blocks/Profile'

function Child_menu() {
  const data = useContext(DataContext)

  const [familyCode, setFamilyCode] = useState('')
  const [lists, setLists] = useState(data.lists)
  const [tasks, setTasks] = useState(data.tasks)
  const [currentList, setCurrentList] = useState(data.currentList)

  function selectList(searchId) {
    lists.forEach(list => {
      if (list.listId == searchId) {
        setCurrentList(list)
        data.currentList.listId = list.listId
        data.currentList.title = list.title
      }
    })
  }

  function handleComplete(taskId) {
    axios.post(`/api/tasks/${taskId}/complete`, {}, data.headers)
      .then(res => {
        alert('Задание отмечено как выполненное')
        fetchTasksData()
      })
      .catch(err => alert(err.response.data))
  }

  function fetchListsData() {
    axios.get(`/api/tasklists/child/${data.user.id}`, data.headers)
      .then(res => {
        data.lists = res.data
        setLists(res.data)
        if (res.data.length != 0) {
          setCurrentList(res.data[0])
        }
      })
      .catch(err => alert(err.response.data))
  }

  function fetchTasksData() {
    axios.get(`/api/tasks/child/${data.user.id}`, data.headers)
      .then(res => {
        data.tasks = res.data
        setTasks(res.data)
      })
      .catch(err => alert(err.response.data))
  }

  function handleFamilyLink() {
    const info = {
      code: familyCode,
      childId: localStorage.getItem('id')
    }
    axios.post('/api/family/join', info, data.headers)
      .then(res => {
        alert(res.data)
      })
      .catch(err => alert(err.response.data))
  }

  useEffect(() => {
    data.user.id = localStorage.getItem('id')
    data.headers.headers.Authorization = "Bearer " + localStorage.getItem('token')
    fetchListsData()
    fetchTasksData()
  }, [])

  return (
    <>
      <Profile
        type="child"
        familyCode={familyCode}
        setFamilyCode={setFamilyCode}
        handleFamilyLink={handleFamilyLink}
      />
      <div className="p-5 w-full">
        <h2 className="text-center mt-6 sm:mt-0">Меню ребенка</h2>
        {/* Main block */}
        <div className="flex sm:flex-row sm:justify-center flex-col mt-10 sm:h-[500px]">
          {/* Lists titles */}
          <div className="overflow-x-auto whitespace-nowrap sm:overflow-visible sm:whitespace-normal sm:ml-6 sm:w-[100px]">
            <div
              className="flex flex-row sm:flex-col justify-evenly border-b-2 pb-1 sm:pb-0 sm:border-2 sm:h-full
              border-gray-300 sm:rounded-md"
            >
              {lists.map((list) => (
                <div
                  key={list.listId}
                  className={`${
                    currentList.listId == list.listId
                      ? "underline text-blue-600"
                      : ""
                  } mx-1.5 text-center leading-8 sm:hover:cursor-pointer`}
                  onClick={() => selectList(list.listId)}
                >
                  {list.title}
                </div>
              ))}
            </div>
          </div>
          {/* Current list block */}
          <div className="relative mt-10 sm:mt-0 flex flex-col sm:ml-15 p-2 pt-0 sm:w-2/5 border-2 border-blue-400 rounded-md overflow-x-scroll">
            <h3 className="sticky top-0 text-center py-2 pb-2 border-b-1 border-blue-200 bg-white">
              {currentList.title}
            </h3>
            {tasks
              .filter((task) => task.taskListId == currentList.listId)
              .filter((task) => task.status != 'completed')
              .map((task) => (
                <Child_task
                  key={task.taskId}
                  description={task.description}
                  id={task.taskId}
                  status={task.status}
                  reward={task.reward}
                  handleComplete={handleComplete}
                />
              ))}
          </div>
        </div>
      </div>
    </>
  )
}

export default Child_menu