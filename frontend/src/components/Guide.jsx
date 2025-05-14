function Guide() {
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