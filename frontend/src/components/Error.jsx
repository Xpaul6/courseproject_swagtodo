import { Link } from 'react-router-dom';

function Error({type}) {
  return (
    <div className='mt-40 flex flex-col'>
      <h2 className='text-center my-2'>An error occured</h2>
      <p className='text-center my-2'>Type: {type}</p>
      <Link to='/' className='text-center text-2xl text-blue-500 underline hover:cursor-pointer my-2'>Reload</Link>
    </div>
  );
}

export default Error;