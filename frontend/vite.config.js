import { defineConfig } from 'vite'
import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => ({
  plugins: [react(), tailwindcss()],
  base: '/',
  preview: {
    port: 80,
    strictPort: true,
    host: true
  },
  server: {
    proxy: {
      '/api': {
        target: mode === 'development' ? 'http://localhost:8080' : 'http://backend:8080',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  },
  build: {
    assetsInlineLimit: 0
  }
}))
