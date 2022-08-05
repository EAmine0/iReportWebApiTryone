import './App.css';
import Header from './components/Header';
import Sidebar from './components/Sidebar';
import './components/Sidebar.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

//codesandbox website
//react-icons.github.io website
//npmjs.com

import Clinical_op from './pages/Op_dashboard/Clinical_op';
import Dmcrf from './pages/Op_dashboard/Dmcrf';
import Dmecrf from './pages/Op_dashboard/Dmecrf';

import Status from './pages/Sites/Status';
import Activities from './pages/Sites/Activities';
import Documents from './pages/Sites/Documents';
import FinancialAgreement from './pages/Sites/FinancialAgreement';

import SelectionPhase from './pages/Monitoring/SelectionPhase';
import InitiationPhase from './pages/Monitoring/InitiationPhase';
import FUPhase from './pages/Monitoring/FUPhase';
import COPhase from './pages/Monitoring/COPhase';
import Details from './pages/Monitoring/Details';
import Ter from './components/ter';
import Ter2 from './components/Ter2';
import { useState } from 'react';

import Op_dashboard from './pages/Op_dashboard/Op_dashboard';
import Sites from './pages/Sites/Sites';
import Monitoring from './pages/Monitoring/Monitoring';

function App() {
  // const [state, setState] = useState(false)

  

  
  // const items = document.querySelectorAll(".topbar-toggle")
  // items.forEach((item) => {
  //     console.log("items : ",item)
  //     item.addEventListener("click", () => {
  //         document.querySelector(".activer").classList.remove("activer")
  //         item.classList.add("activer")
  //     })
  // })
  

  return (
          
          <>
        <div className="App">
          

           {/* <button onClick={() => setState("ter1")}>page1</button>
           <button onClick={() => setState("ter2")}>page2</button>
           {state === "ter1" && <Clinical_op/>}
           {state === "ter2" && <Ter2/>} */}
           
          <BrowserRouter>
            <Header/>
            <Sidebar/>
            <Routes>
              <Route path="/OperationalDashboard/" element={<Op_dashboard />}/>
              <Route path="/Sites/" element={<Sites />}/>
              <Route path="/Monitoring/" element={<Monitoring />}/>
{/*               
              <Route path="/OperationalDashboard/DMCRF" element={<Dmcrf />}/>
              <Route path="/OperationalDashboard/DMeCRF" element={<Dmecrf />}/>

              
              <Route path="/Sites/Activities" element={<Activities />}/>
              <Route path="/Sites/Documents" element={<Documents />}/>
              <Route path="/Sites/FinancialAgreement" element={<FinancialAgreement />}/>

              
              <Route path="/Monitoring/InitiationPhase" element={<InitiationPhase />}/>
              <Route path="/Monitoring/FUPhase" element={<FUPhase />}/>
              <Route path="/Monitoring/COPhase" element={<COPhase />}/>
              <Route path="/Monitoring/Details" element={<Details />}/> */}
            </Routes>
          </BrowserRouter>
          
        </div>
        
        </>
  );
}


              {/* <Route path="/Monitoring" element={<SelectionPhase />}/> */}
export default App;
