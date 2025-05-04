import { Link } from 'react-router-dom';

function Error() {
  return (
    <div>
      <h2>An error occured</h2>
      <Link to='/'>Reload</Link>
    </div>
  );
}

export default Error;