import { useState } from 'react'

function Parent_task() {
  const [isOpen, setIsOpen] = useState(false)

  return (
    <div className="flex justify-around items-center my-2 relative">
      <div className={`w-fit mx-10 ${isOpen ? "underline" : ""}`}>Task description</div>
      <button
        className="absolute right-3 p-2 rotate-90 cursor-pointer"
        onClick={() => (isOpen ? setIsOpen(false) : setIsOpen(true))}
      >
        {/* {isOpen? '▲' : '▼'} */}
        {isOpen ? '<' : '>'}
      </button>
      <div
        className={`absolute right-7 bg-white top-8 flex flex-col justify-center shadow-md rounded-md
          overflow-hidden transition-all duration-200 ease-in-out ${isOpen ? "max-h-36 z-10" : "max-h-0 z-10"}`}
      >
        <button className="bg-white hover:bg-gray-100 px-4 py-2 text-left cursor-pointer">
          Редактировать
        </button>
        <button className="bg-white hover:bg-gray-100 px-4 py-2 text-left cursor-pointer">
          Удалить
        </button>
        <button className="bg-white hover:bg-gray-100 px-4 py-2 text-left cursor-pointer">
          Подтвердить
        </button>
        <button className="bg-white hover:bg-gray-100 px-4 py-2 text-left cursor-pointer">
          Назначить
        </button>
      </div>
    </div>
  )
}

export default Parent_task