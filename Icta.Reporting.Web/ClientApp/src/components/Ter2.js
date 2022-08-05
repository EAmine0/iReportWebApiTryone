import React from 'react'
import DoughnutChart from './Charts/DoughnutChart'
import PieChart from './Charts/PieChart'

function Ter2() {
    return (
        <>
        <div className='title_page'>
            Operational Dashboard2
        </div>
        <div className='box_container'>
            <div className='block'>
                <div className='title_block'>
                    Patient status
                </div>
                <DoughnutChart/>
            </div>
            <div className='block'>
                <div className='title_block'>
                    Patient status
                </div>
                <PieChart/>
            </div>
        </div>
        
        </>
        
    )
}

export default Ter2
