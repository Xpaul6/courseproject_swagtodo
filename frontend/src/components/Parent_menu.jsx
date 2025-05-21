import '../styles/main.css'

function Parent_menu() {
  return (
    <>
      <div className="p-5 w-full">
        <h2 className="text-center">Меню родителя</h2>
        <div className="flex sm:flex-row sm:justify-around flex-col mt-10 sm:h-[400px] sm:ml-6">
          <div className="overflow-x-auto whitespace-nowrap sm:overflow:visible sm:whitespace-normal">
            <div
              className="flex flex-row sm:flex-col justify-evenly min-w-full border-b-2 pb-1 sm:pb-0 sm:border-2 sm:h-full
              border-gray-300 sm:rounded-md"
            >
              <div className="underline text-blue-600 mx-1.5 text-center flex align-center leading-8 sm:hover:cursor-pointer">
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
          <div className="relative mt-10 sm:mt-0 flex flex-col sm:ml-15 p-2 pt-0 sm:w-2/5 border-2 border-blue-400 rounded-md overflow-x-scroll">
            <h3 className="sticky top-0 text-center py-2 pb-2 border-b-1 border-blue-200 bg-white">
              Список 1
            </h3>
            <span className="text-center">Задача 1</span>
            <span className="text-center">Задача 2</span>
            <span className="text-center">Задача 3</span>
            <span className="text-center">Задача 4</span>
            <button className="sticky bottom-1 right-1 ml-auto mt-auto py-2 px-3.5 w-min text-xl text-white border-2
              rounded-xl bg-green-500 sm:cursor-pointer">
              +
            </button>
          </div>
          <div className="relative mt-10 sm:mt-0 flex flex-col p-2 pt-0 sm:w-2/6 border-2 border-indigo-400 rounded-md sm:ml-15 sm:mr-6">
            <h3 className="sticky text-center mb-2 pt-2 pb-2 border-b-1 border-indigo-200">
              Статистика
            </h3>
            <div className="flex justify-evenly">
              <div className="">Имя Фамилия</div>
              <div>2/10</div>
            </div>
            <button className="sticky bottom-1 right-1 ml-auto mt-auto py-2 px-3.5 w-min text-xl text-white border-2
              rounded-xl bg-green-500 sm:cursor-pointer">
              +
            </button>
          </div>
        </div>
      </div>
    </>
  );
}

export default Parent_menu