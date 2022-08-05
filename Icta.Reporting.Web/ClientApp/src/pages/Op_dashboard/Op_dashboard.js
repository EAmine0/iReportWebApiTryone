import React from 'react'
import { useState } from 'react'
import Clinical_op from './Clinical_op'
import Dmcrf from './Dmcrf'
import Dmecrf from './Dmecrf'
import Btn_export from '../../components/Btn_export'
import Sidebar from '../../components/Sidebar'

function Op_dashboard() {
    const [state, setState] = useState("")

    var marker = document.querySelector('.marker')
    var item = document.querySelectorAll('.topbar-toggle')
    function indicator(e){
        marker.style.left = e.offsetLeft+"px";
        marker.style.width = e.offsetWidth+"px";
    }
    item.forEach(link => {
        link.addEventListener('click', (e)=>{
            indicator(e.target)
        })
    })

    function tester(){
        
    }


    return (
        <>
        <div className='title_page'>
            Operational Dashboard
        </div>
        <div className='topbar-container'>
            <ul className='topbar-items'>
                <div className='marker'></div>
                <li className='topbar-toggle activer' onClick={() => setState("clinic")}>Clinical Operational</li>
                <li className='topbar-toggle' onClick={() => setState("dm")}>DM-CRF</li>
                <li className='topbar-toggle' onClick={() => setState("dme")}>DM-eCRF</li> 
            </ul>
            {state === "clinic" && <Clinical_op/>}
            {state === "dm" && <Dmcrf/>}
            {state === "dme" && <Dmecrf/>}
        </div>
        <Btn_export/>
        </>
    )
}

export default Op_dashboard
