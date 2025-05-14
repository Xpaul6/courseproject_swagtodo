import { useContext } from "react"
import { GuideContext } from "../context/GuideContext"

function DefineLocation(path) {
  console.log(path)
  try {
    if (path === "/") return "/guidePage/glavnoe_menyu_sajta.htm"
  } catch(e) {
    console.log(e)
  } finally {
    return "/guidePage/glavnoye_menyu_saita.htm"
  }
}

function Guide() {
  const { path, setPath } = useContext(GuideContext)

  return (
    <div style={{ height: '100vh', width: '100%' }}>
      <iframe
        src="/guidePage/index.htm"
        style={{ border: 'none', width: '100%', height: '100%' }}
        title="User Guide"
        sandbox="allow-same-origin allow-scripts allow-popups allow-forms"
      />
    </div>
  )
}

export default Guide