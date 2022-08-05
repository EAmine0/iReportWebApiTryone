import React from 'react'
import { useState } from 'react'
import Clinical_op from '../Op_dashboard/Clinical_op'
import Dmcrf from '../Op_dashboard/Dmcrf'
import Dmecrf from '../Op_dashboard/Dmecrf'
import Btn_export from '../../components/Btn_export'
import Sidebar from '../../components/Sidebar'


function Sites() {
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

    return (
        <>
        <div className='title_page'>
            Sites
        </div>
        <div className='topbar-container'>
            <ul className='topbar-items'>
                <div className='marker'></div>
                <li className='topbar-toggle activer' onClick={() => setState("clinic")}>Status</li>
                <li className='topbar-toggle' onClick={() => setState("dm")}>Activities</li>
                <li className='topbar-toggle' onClick={() => setState("dme")}>Documents</li> 
                <li className='topbar-toggle' onClick={() => setState("dme")}>Financial agreement</li> 
            </ul>
            {state === "clinic" && <Clinical_op/>}
            {state === "dm" && <Dmcrf/>}
            {state === "dme" && <Dmecrf/>}
            {state === "dme" && <Dmecrf/>}
        </div>
        <Btn_export/>
        </>
    )
}

export default Sites
