import { useState } from 'react'

function Profile({type}) {
  const [isOpen, SetIsOpen] = useState(false)

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
              isOpen ? "max-h-24" : "max-h-0"
            }`}
        >
          <button className="bg-white text-center hover:bg-gray-100 px-4 py-2 cursor-pointer">
            Выйти
          </button>
          {type == "child" ? (
            <>
              <div className="bg-white text-center hover:bg-gray-100 px-4 py-2 cursor-pointer">
                Код: 1234
              </div>
            </>
          ) : (
            <></>
          )}
        </div>
      </div>
    </>
  );
}

export default Profile