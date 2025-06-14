import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router'
import { useContext } from 'react'

import { DataContext } from '../../context/DataContext.jsx'

function Profile({type, familyCode, setFamilyCode = () => {}, handleFamilyLink = () => {}}) {
  const navigate = useNavigate()
  const [isOpen, SetIsOpen] = useState(false)
  const data = useContext(DataContext)

  function LogOut() {
    localStorage.setItem('token', '')
    localStorage.setItem('role', '')
    localStorage.setItem('id', '')
    navigate('/')
  }

  return (
    <>
      <div className="absolute top-1 right-1">
        <button
          className="px-3 py-1 sm:px-4 sm:py-2 border border-gray-100 rounded-md sm:cursor-pointer sm:hover:bg-gray-100"
          onClick={() => (isOpen ? SetIsOpen(false) : SetIsOpen(true))}
        >
          Профиль {type == "parent" ? "родителя" : "ребенка"}
        </button>
        <div
          className={`absolute w-full bg-white top-10 flex flex-col justify-center shadow-md rounded-md
            overflow-hidden transition-all duration-200 ease-in-out z-10 ${
              isOpen ? "max-h-36" : "max-h-0"
            }`}
        >
          <button className="bg-white text-center hover:bg-gray-100 px-4 py-2 cursor-pointer" onClick={LogOut}>
            Выйти
          </button>
          {type == "parent" ? (
            <>
              <div className="bg-white text-center hover:bg-gray-100 px-4 py-2 cursor-pointer">
                Код: {familyCode}
              </div>
            </>
          ) : (
            <>
              <input
                type="code"
                placeholder="Код"
                value={familyCode}
                onChange={(e) => setFamilyCode(e.target.value)}
                className="m-1 p-1 rounded-md border border-gray-300 placeholder:text-center"
              />
              <button className="bg-white text-center hover:bg-gray-100 px-4 py-2 cursor-pointer" onClick={handleFamilyLink}>
                Привязать 
              </button>
            </>
          )}
        </div>
      </div>
    </>
  );
}

export default Profile