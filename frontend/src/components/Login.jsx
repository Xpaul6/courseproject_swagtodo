import '../styles/main.css'

function Login() {
  return (
    <>
      <div className="pagebox">
        <h2>Вход</h2>
        <form action="" className="flex flex-col mt-5">
          <input
            type="email"
            placeholder="Эл. почта"
            className="mt-5 p-1 rounded-md border border-gray-300 placeholder:text-center"
          />
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
            Войти
          </button>
        </form>
      </div>
    </>
  );
}

export default Login