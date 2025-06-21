import './App.css'
import Menu from './components/layout/Menu'
import NavBar from './components/layout/NavBar'
import { Router } from './Routes'

function App() {

  return (

    <>

      <Menu />
      <NavBar />
      <div className='flex justify-center h-[88vh]'>

        <div className="w-full h-full px-8 md:w-3/4 md:px-0 overflow-y-scroll md:overflow-y-hidden">
          <Router />
        </div>

      </div>
    </>

  )
}

export default App
