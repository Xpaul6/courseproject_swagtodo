import { useState, useEffect, useContext } from 'react'
import axios from 'axios'

import { DataContext } from '../../context/DataContext'

function Child({name, id}) {
  const data = useContext(DataContext)

  const [stat, setStat] = useState(0)

  function getStats() {
    axios.get(`/api/tasks/child/${id}/numberofCompleted`, data.headers)
      .then(res => {
        setStat(res.data)
      })
      .catch(err => err.response.data)
  }

  useEffect(() => {
    getStats()
  }, [])

  return (
    <>
      <div className="flex justify-evenly my-2">
        <div className="">{name}</div>
        <div>{stat} выполнено</div>
      </div>
    </>
  )
}

export default Child