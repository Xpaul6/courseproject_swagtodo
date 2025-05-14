import { createContext, useContext, useEffect, useState } from 'react';

export const GuideContext = createContext();

export const GuideProvider = ({ children }) => {
  const [path, setPath] = useState("/")

  useEffect(() => {
    const handleKeyDown = (event) => {
      if (event.key === 'F1' || event.keyCode === 112) {
        event.preventDefault();
        window.open('/guide')
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  return (
    <GuideContext.Provider value={{ path, setPath }}>
      {children}
    </GuideContext.Provider>
  );
};

export const useGuide = () => useContext(GuideContext);