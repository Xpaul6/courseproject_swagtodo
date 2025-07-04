import { useState, useEffect, useContext } from 'react'
import { useNavigate } from 'react-router'
import axios from 'axios'

import { DataContext } from '../context/DataContext'

import '../styles/main.css'

import Parent_task from './blocks/Parent_task'
import Child from './blocks/Child'
import Profile from './blocks/Profile'

function Parent_menu() {
  const navigate = useNavigate()
  const data = useContext(DataContext)

  const [familyCode, setFamilyCode] = useState(data.familycode)
  const [lists, setLists] = useState(data.lists)
  const [currentList, setCurrentList] = useState(data.currentList)
  const [children, setChildren] = useState(data.children)
  const [tasks, setTasks] = useState(data.tasks)

  function createNewList() {
    navigate('/new-list')
  }

  function createNewTask() {
    navigate('/new-task')
  }

  function handleListDelete() {
    axios.delete(`/api/tasklists/${currentList.listId}`, data.headers)
      .then(_ => {
        alert('Список удален')
        fetchListsData()
      })
      .catch(err => alert(err.response.data))
  }

  function handleTaskDelete(taskId) {
    axios.delete(`/api/tasks/${taskId}`, data.headers)
      .then(_ => {
        alert("Удалено успешно")
        fetchTasksData()
      })
      .catch(err => err.response.data)
  }

  function handleTaskEdit(taskId) {
    data.tasks.forEach(task => {
      if (task.taskId == taskId) {
        data.currentTask = task
        console.log(data.currentTask)
        navigate("/edit-task")
      }
    })
  }

  function handleTaskApprove(taskId) {
    axios.post(`/api/tasks/${taskId}/approve`, {}, data.headers)
      .then(res => {
        alert('Выполнение подтверждено')
        fetchTasksData()
      })
      .catch(err => alert(err.response.data))
  }

  function handleTaskReject(taskId) {
    axios.post(`/api/tasks/${taskId}/reject`, {}, data.headers)
      .then(res => {
        alert('Выполнение отклонено')
        fetchTasksData()
      })
      .catch(err => alert(err.response.data))
  }

  function selectList(searchId) {
    lists.forEach(list => {
      if (list.listId == searchId) {
        setCurrentList(list)
        data.currentList.listId = list.listId
        data.currentList.title = list.title
      }
    })
  }

  function fetchListsData() {
    axios.get(`/api/tasklists/parent/${data.user.id}`, data.headers)
      .then(res => {
        data.lists = res.data
        setLists(res.data)
        if (data.currentList.listId == 0) {
          setCurrentList(res.data[0])
        }
        if (!res.data.includes(currentList)) {
          setCurrentList(res.data[0])
        }
      })
      .catch(err => alert(err.response.data))
  }

  function fetchTasksData() {
    axios.get(`/api/tasks/parent/${data.user.id}`, data.headers)
      .then(res => {
        data.tasks = res.data
        setTasks(res.data)
      })
      .catch(err => alert(err.response.data))
  }

  function fetchChildrenData() {
    axios.get(`/api/family/children/${data.user.id}`, data.headers)
      .then(res => {
        data.children = res.data
        setChildren(res.data)
      })
      .catch(err => alert(err.response.data))
  }

  function fetchFamilyCode() {
    axios.get(`/api/family/code/${data.user.id}`, data.headers)
      .then(res => {
        data.familycode = res.data.code
        setFamilyCode(res.data.code)
      })
      .catch(err => alert(err))
  }

  useEffect(() => {
    data.user.id = localStorage.getItem('id')
    data.headers.headers.Authorization = "Bearer " + localStorage.getItem('token')
    fetchFamilyCode()
    fetchListsData()
    fetchChildrenData()
    fetchTasksData()
  }, [])

  return (
    <>
      <Profile type="parent" familyCode={familyCode} />
      <div className="p-5 w-full">
        <h2 className="text-center mt-6 sm:mt-0">Меню родителя</h2>
        {/* Main block */}
        <div className="flex sm:flex-row sm:justify-around flex-col mt-10 sm:h-[500px]">
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
              <div
                key="addbtn"
                className="text-green-800 rounded-xl border-2 border-green-500 text-center py-1 px-3 mx-1.5 sm:hover:cursor-pointer"
                onClick={createNewList}
              >
                +
              </div>
            </div>
          </div>
          {/* Current list block (tasks) */}
          <div className="relative mt-10 sm:mt-0 flex flex-col sm:ml-15 p-2 pt-0 sm:w-2/5 border-2 border-blue-400 rounded-md overflow-x-scroll">
            <h3 className="sticky top-0 text-center py-2 pb-2 border-b-1 border-blue-200 bg-white">
              {currentList ? currentList.title : "Cоздайте список"}
            </h3>
            {tasks
              .filter((task) => task.taskListId == currentList.listId)
              .filter((task) => task.status != "completed")
              .map((task) => (
                <Parent_task
                  key={task.taskId}
                  description={task.description}
                  id={task.taskId}
                  status={task.status}
                  handleTaskDelete={handleTaskDelete}
                  handleTaskEdit={handleTaskEdit}
                  handleTaskApprove={handleTaskApprove}
                  handleTaskReject={handleTaskReject}
                />
              ))}
            {currentList && currentList.listId != 0 ? (
              <>
                <button
                  className="sticky bottom-1 right-1 ml-auto mt-auto py-2 px-3.5 w-min text-xl text-white border-2
                rounded-xl bg-green-500 sm:cursor-pointer"
                  onClick={createNewTask}
                >
                  +
                </button>
                <button
                  className="absolute top-1.5 right-4.5 z-10 py-1 px-2 text-red-500 cursor-pointer rounded-md border-2 border-transparent
                    sm:hover:border-red-500 transition-all ease-in-out"
                  onClick={handleListDelete}
                >
              X
            </button>
              </>
            ) : (
              <></>
            )}
          </div>
          {/* Stats block */}
          <div
            className="relative mt-10 sm:mt-0 flex flex-col p-2 pt-0 sm:w-2/6 border-2 border-indigo-400 rounded-md
            sm:ml-15 sm:mr-6 overflow-x-scroll"
          >
            <h3 className="sticky text-center mb-2 pt-2 pb-2 border-b-1 border-indigo-200">
              Статистика
            </h3>
            {children.map((child) => (
              <Child name={child.name} key={child.userId} id={child.userId} />
            ))}
          </div>
        </div>
      </div>
    </>
  )
}

export default Parent_menu