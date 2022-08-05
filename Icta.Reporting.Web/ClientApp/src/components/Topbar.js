import React from 'react'
import { Link, NavLink } from "react-router-dom"
import './Topbar.css'

const Topbar = (props) => {
    return (
        <>
        <div className='topbar-container'>
            <ul className='topbar-items'>
                <li className='topbar-toggle'>
                {props.test.map((topbaritem) => {
                        return (
                            <li key={topbaritem.id} 
                                className={topbaritem.cName}>
                                <NavLink to={topbaritem.path} style={({ isActive }) => (isActive ? {color:'#003E6E', borderBottom:'2px solid orange'} : {})}>
                                    <span className='title'>{topbaritem.title}</span>
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

export default Topbar
