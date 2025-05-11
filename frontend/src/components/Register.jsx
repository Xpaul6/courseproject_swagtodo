import '../styles/main.css'

function Register() {
  return (
    <>
      <div className="pagebox">
        <h2>Регистрация</h2>
        <form action="" className="flex flex-col mt-5">
          <input
            type="text"
            placeholder="Имя"
            className="p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <input
            type="email"
            placeholder="Эл. почта"
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <div className="mt-5">
            <input type="radio" id="parent" name="role" value="parent" />
            <label htmlFor="parent" className="ml-2">
              Родитель
            </label>
          </div>
          <div className="mt-2">
            <input type="radio" id="child" name="role" value="child" />
            <label htmlFor="child" className="ml-2">
              Ребенок
            </label>
          </div>
          <input
            type="passsword"
            placeholder="Пароль"
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
          <button
            className="mt-5 p-2 border-2 rounded-md border-green-500 transition-all ease-in-out transition-150
              hover:bg-green-500 hover:cursor-pointer hover:text-white"
            onClick={(e) => {
              e.preventDefault();
            }}
          >
            Зарегистрироваться
          </button>
        </form>
      </div>
    </>
  );
}

export default Register;