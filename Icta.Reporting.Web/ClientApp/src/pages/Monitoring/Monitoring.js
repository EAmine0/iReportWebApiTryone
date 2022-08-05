import React from 'react'
import Clinical_op from '../Op_dashboard/Clinical_op'
import Dmcrf from '../Op_dashboard/Dmcrf'
import Dmecrf from '../Op_dashboard/Dmecrf'
import Btn_export from '../../components/Btn_export'
import { useState } from 'react'

function Monitoring() {
    const [state, setState] = useState("")

   

    return (
        <>
        <div className='title_page'>
            Sites
        </div>
        <div className='topbar-container'>
            <ul className='topbar-items'>
                <div className='marker'></div>
                <li className='topbar-toggle activer' onClick={() => setState("clinic")}>Selection phase</li>
                <li className='topbar-toggle' onClick={() => setState("dm")}>Initiation phase</li>
                <li className='topbar-toggle' onClick={() => setState("dme")}>Follow-up phase</li> 
                <li className='topbar-toggle' onClick={() => setState("dm")}>Close-out phase</li> 
                <li className='topbar-toggle' onClick={() => setState("dme")}>Details</li> 
            </ul>
            {state === "clinic" && <Clinical_op/>}
            {state === "dm" && <Dmcrf/>}
            {state === "dme" && <Dmecrf/>}
            {state === "dme" && <Dmecrf/>}
            {state === "dme" && <Dmecrf/>}
        </div>
        <Btn_export/>
        </>
    )
}

export default Monitoring
