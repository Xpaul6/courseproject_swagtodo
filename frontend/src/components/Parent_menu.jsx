import '../styles/main.css'

function Parent_menu() {
  return (
    <>
      <div className="p-5 w-full">
        <h2 className="text-center">Меню родителя</h2>
        <div className="flex sm:flex-row flex-col mt-10 sm:h-[400px]">
          <div className="overflow-x-auto whitespace-nowrap">
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
              <div className="text-white rounded-xl bg-green-500 text-center py-1 px-3 mx-1.5 sm:hover:cursor-pointer">
                +
              </div>
            </div>
          </div>
          <div className="mt-10 sm:mt-0 flex flex-col sm:ml-4 p-2 sm:w-2/5 border border-blue-400 rounded-md">
            <h3 className="text-center my-2 pb-2 border-b-1 border-blue-200">
              Список 1
            </h3>
            <span className="text-center">Задача 1</span>
            <span className="text-center">Задача 2</span>
            <span className="text-center">Задача 3</span>
            <span className="text-center">Задача 4</span>
            <div className="w-full flex flex-row-reverse sm:mt-auto">
              <button className="py-2 px-3.5 text-xl text-white border-2 rounded-xl bg-green-500">
                +
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default Parent_menu