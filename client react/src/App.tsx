import React from "react"
import Home from "./pages/Home"
import Axios from 'axios'
import Navbar from "./components/Navbar"
import ProtectedRoute from "./lib/ProtectedRoute"
import AddStudent from "./pages/AddStudent"
import {Routes, Route } from 'react-router-dom';
function App() {

  // const [data, setData] = React.useState("")

  // React.useEffect(()=>{
  //   Axios.get("http://localhost:5271/WeatherForecast")
  //   .then(res => console.log(res))
  // },[])



  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route element={<ProtectedRoute />}>
          <Route path="/addStudent" element={<AddStudent />} />
        </Route>
      </Routes>
    </>
  )
}

export default App
