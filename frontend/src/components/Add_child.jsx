import { useNavigate } from "react-router"

function Add_child() {
  const navigate = useNavigate()

  function AddChild() {
    // TODO:
    navigate('/parent-menu')
  }

  return (
    <>
      <div className="p-5 w-full">
        <h2 className="text-center">Добавить ребенка</h2>
        {/* Main block */}
        <div className="flex sm:flex-row sm:justify-around flex-col mt-10 sm:h-[500px]">
          <div
            className="relative mt-10 sm:mt-0 flex flex-col justify-center items-center p-2 py-10 sm:py-2 sm:pb-20
            sm:w-[400px] border-2 border-blue-400 rounded-md overflow-x-scroll"
          >
            <form action="" className="flex flex-col w-4/5 mx-2">
              <input
                type="text"
                placeholder="Код ребенка (из профиля)"
                className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
              />
              <button
                className="mt-5 p-2 bg-green-500 text-white sm:bg-transparent sm:text-black border-2 rounded-md
                sm:border-green-500 transition-all ease-in-out transition-150 sm:hover:bg-green-500
                hover:cursor-pointer sm:hover:text-white"
                onClick={(e) => {
                  e.preventDefault()
                  AddChild()
                }}
              >
                Добавить
              </button>
            </form>
          </div>
        </div>
      </div>
    </>
  )
}

export default Add_child