function Child_task() {
  return (
    <div className="flex justify-around items-center my-2 relative">
      <div className="w-full mx-10 flex flex-col items-center">
        <div className="w-fit">Task description</div>
        <div className="w-fit">Completion reward</div>
      </div>
      <button className="absolute right-3 p-2 cursor-pointer text-2xl">
        ✓
      </button>
    </div>
  );
}

export default Child_task