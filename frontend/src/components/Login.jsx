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
            className="mt-5 p-2 bg-green-500 text-white sm:bg-transparent sm:text-black border-2 rounded-md
              sm:border-green-500 transition-all ease-in-out transition-150 sm:hover:bg-green-500
              hover:cursor-pointer sm:hover:text-white"
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