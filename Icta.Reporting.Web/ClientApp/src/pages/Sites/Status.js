import React from 'react'
import "../Pages.css"
import Topbar from '../../components/Topbar'
import { TopbarSites } from '../../components/TopbarData'
import { useState, useEffect } from 'react'
import { Checkbox, Radio } from '@material-ui/core'

function Status() {
    const [name, setName] = useState("world")
    const [phone, setPhone] = useState()
    const [hidden, setHidden] = useState(true);
    const text = "bien vue l'ami"
    const maxLength = 5

    useEffect(()=> {
        document.title = `hello ${name}!`
    })

    return (
        <>
        <div className='title_page'>
            Sites
        </div>
        <Topbar test={TopbarSites}/>
        <div className='box_container'>
            <div className='block'>
                <div className='title_block'>
                    Site status
                </div>
            </div>
            <div className='block'>
                <div className='title_block'>
                    Patient status
                </div>
            </div>
        </div>
        <div className='boxx'>
            <p>Enter your name: <input value={name} onChange={event => setName(event.target.value)}/></p>
            <p>Enter your phone number: <input value={phone} onChange={event => setPhone(event.target.value)}/></p> 
            <p>Hello {name}, and we call you back at this phone number : {phone} </p>
            <span>
                {hidden ? `${text.substr(0, maxLength).trim()} ...  ` : text}
                {hidden ? (
                    <a onClick={() => setHidden(false)}>read more</a>
                ) : (
                    <a onClick={() => setHidden(true)}>  read less</a>
                )}
            </span>
        </div>
        </>
        
    )
}

export default Status
