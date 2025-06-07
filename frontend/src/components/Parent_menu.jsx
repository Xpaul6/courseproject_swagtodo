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

  const [familyCode, setFamilyCode] = useState("")

  function CreateNewTask() {
    // TODO: 
    navigate('/new-task')
  }

  function AddChild() {
    // TODO:
    navigate('/add-child')
  }

  function fetchListsData() {

  }

  function fetchTasksData() {

  }

  function fetchChildrenData() {

  }

  function fetchFamilyCode() {
    axios.get(`/api/familycode/${data.user.id}`, data.headers)
      .then(res => {
        data.familycode = res.data.code
        setFamilyCode(res.data.code)
      })
      .catch(err => alert(err))
  }

  useEffect(() => {
    data.user.id = localStorage.getItem('id')
    console.log(data) // debug
    fetchFamilyCode()
  }, [])

  return (
    <>
      <Profile type="parent" code={familyCode}/>
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
              <div className="underline text-blue-600 mx-1.5 text-center leading-8 sm:hover:cursor-pointer">
                Список 1
              </div>
              <div className="mx-1.5 text-center leading-8 sm:hover:cursor-pointer">
                Список 2
              </div>
              <div className="mx-1.5 text-center leading-8 sm:hover:cursor-pointer">
                Список 3
              </div>
              <div className="text-green-800 rounded-xl border-2 border-green-500 text-center py-1 px-3 mx-1.5 sm:hover:cursor-pointer">
                +
              </div>
            </div>
          </div>
          {/* Current list block */}
          <div className="relative mt-10 sm:mt-0 flex flex-col sm:ml-15 p-2 pt-0 sm:w-2/5 border-2 border-blue-400 rounded-md overflow-x-scroll">
            <h3 className="sticky top-0 text-center py-2 pb-2 border-b-1 border-blue-200 bg-white">
              Список 1
            </h3>
            <Parent_task />
            <Parent_task />
            <Parent_task />
            <Parent_task />
            <Parent_task />
            <button
              className="sticky bottom-1 right-1 ml-auto mt-auto py-2 px-3.5 w-min text-xl text-white border-2
              rounded-xl bg-green-500 sm:cursor-pointer"
              onClick={CreateNewTask}
            >
              +
            </button>
          </div>
          {/* Stats block */}
          <div className="relative mt-10 sm:mt-0 flex flex-col p-2 pt-0 sm:w-2/6 border-2 border-indigo-400 rounded-md
            sm:ml-15 sm:mr-6 overflow-x-scroll">
            <h3 className="sticky text-center mb-2 pt-2 pb-2 border-b-1 border-indigo-200">
              Статистика
            </h3>
            <Child />
            <Child />
            <Child />
            {/* <button
              className="sticky bottom-1 right-1 ml-auto mt-auto py-2 px-3.5 w-min text-xl text-white border-2
              rounded-xl bg-green-500 sm:cursor-pointer"
              onClick={AddChild}
            >
              +
            </button> */}
          </div>
        </div>
      </div>
    </>
  );
}

export default Parent_menu