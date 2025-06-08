import { useState } from 'react'

function Parent_task({description, id, status, handleTaskDelete}) {
  const [isOpen, SetIsOpen] = useState(false)

  return (
    <div className="flex justify-around items-center my-2 relative">
      <div className={`w-fit mx-10 ${isOpen ? "underline" : ""}`}>
        {description}
      </div>
      <button
        className={`absolute right-3 p-2 cursor-pointer ${
          isOpen ? "rotate-180" : ""
        } transition-all duration-200`}
        onClick={() => (isOpen ? SetIsOpen(false) : SetIsOpen(true))}
      >
        ▼
      </button>
      <div
        className={`absolute right-7 bg-white top-8 flex flex-col justify-center shadow-md rounded-md
          overflow-hidden transition-all duration-200 ease-in-out z-10 ${
            isOpen ? "max-h-36" : "max-h-0"
          }`}
      >
        <button className="bg-white hover:bg-gray-100 px-4 py-2 text-left cursor-pointer">
          Редактировать
        </button>
        <button className="bg-white hover:bg-gray-100 px-4 py-2 text-left cursor-pointer" onClick={() => handleTaskDelete(id)}>
          Удалить
        </button>
        <button className="bg-white hover:bg-gray-100 px-4 py-2 text-left cursor-pointer">
          Подтвердить
        </button>
      </div>
    </div>
  )
}

export default Parent_task