import React from 'react'
import "./Sidebar.css"
import { Link, NavLink } from "react-router-dom"
import { SidebarData } from './SidebarData'
import { SidebarData2 } from './SidebarData'
import { TopbarOpDash } from './TopbarData'

function Sidebar() {
    return (
        <>
        <div className='sidebar-container'>
            <ul className='sidebar-items'>
                <li className='sidebar-toggle'>
                    {SidebarData.map((sidebaritem) => {
                        return (
                            <li key={sidebaritem.id} 
                                className={sidebaritem.cName}>
                                <NavLink to={sidebaritem.path} style={({ isActive }) => (isActive ? {color:'#003E6E', borderRight:'3px solid #003E6E'} : {})}>
                                    <span className='icon'>{sidebaritem.icon}</span>
                                    <span className='title'>{sidebaritem.title}</span>
                                </NavLink>
                                
                            </li>
                        );
                    })}
                </li>
            </ul>
        </div>
        </>
    )
}

export default Sidebar
