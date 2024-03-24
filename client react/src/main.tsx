import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import { ThemeProvider } from "@/components/ui/theme-provider.tsx"
import '@/styles/global.css'

import {BrowserRouter} from "react-router-dom"

ReactDOM.createRoot(document.getElementById('root')!).render(
  <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
    
    <BrowserRouter>
      <App />
    </BrowserRouter>
    
  </ThemeProvider>,
)
