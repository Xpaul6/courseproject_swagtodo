import { Link } from 'react-router-dom';

function Error({type}) {
  return (
    <div>
      <h2>An error occured</h2>
      <p>Type: {type}</p>
      <Link to='/'>Reload</Link>
    </div>
  );
}

export default Error;