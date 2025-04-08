import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [apiText, setApiText] = useState('');

  async function getApiText() {
    await fetch('/api/hello')
      .then((res) => res.json())
      .then((data) => {setApiText(data.message)})
      .catch((err) => 'Error: ' + err.message)
  }

  useEffect(getApiText, [])

  return (
    <>
      <div className='hello'>{apiText}</div>
    </>
  )
}

export default App
