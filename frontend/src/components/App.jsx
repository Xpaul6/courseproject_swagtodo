import { useState, useEffect } from 'react'
import '../styles/App.css'

function App() {
  const [apiText, setApiText] = useState('defaultText');

  // async function getApiText() {
  //   await fetch('/api/hello')
  //     .then((res) => res.json())
  //     .then((data) => {setApiText(data.message)})
  //     .catch((err) => 'Error: ' + err.message)
  // }

  // useEffect(getApiText, [])

  return (
    <>
      <div className='hello'>
        <h1>Header 1</h1>
        <h2>Header 2</h2>
        <h3>Header 3</h3>
      </div>
    </>
  )
}

export default App
