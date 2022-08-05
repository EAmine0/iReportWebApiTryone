import React from 'react'
import Topbar from '../../components/Topbar'
import { TopbarSites } from '../../components/TopbarData'

function FinancialAgreement() {
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
        </>
    )
}

export default FinancialAgreement
