function Child_task({description, id, status, reward, handleComplete}) {
  return (
    <div className="flex justify-around items-center my-2 relative">
      <div className="w-full mx-10 flex flex-col items-center">
        <div className="w-fit">{description}</div>
        <div className="w-fit">Награда: {reward}</div>
      </div>
      <button
        className={`absolute right-3 p-2 text-3xl ${
          status == "ongoing" ? "text-green-500 cursor-pointer" : ""
        }`}
        onClick={() => {
          status == "ongoing" ? handleComplete(id) : {};
        }}
      >
        {status == "ongoing" ? "✓" : "..."}
      </button>
    </div>
  );
}

export default Child_task