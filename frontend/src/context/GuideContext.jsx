import { createContext, useContext, useEffect } from 'react';

export const GuideContext = createContext();

export const GuideProvider = ({ children }) => {
  useEffect(() => {
    const handleKeyDown = (event) => {
      if (event.key === 'F1' || event.keyCode === 112) {
        event.preventDefault();
        window.location.href = '/guide'
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  return (
    <GuideContext.Provider value={{}}>
      {children}
    </GuideContext.Provider>
  );
};

export const useGuide = () => useContext(GuideContext);