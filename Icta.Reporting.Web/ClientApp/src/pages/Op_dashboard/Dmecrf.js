import React from 'react'
import Topbar from '../../components/Topbar'
import { TopbarOpDash } from '../../components/TopbarData';
import DoughnutChart from '../../components/Charts/DoughnutChart';
import PieChart from '../../components/Charts/PieChart';
import Btn_export_data from '../../components/Btn_export_data';

function Dmecrf() {
    return (
        <>
        {/* <div className='title_page'>
            Operational Dashboard
        </div>
        <Topbar test={TopbarOpDash}/> */}
        <div className='box_container'>
            <div className='block'>
                <div className='title_block'>
                    Patient status
                </div>
                <Btn_export_data/>
                <DoughnutChart/>
            </div>
            <div className='block'>
                <div className='title_block'>
                    Patient status
                </div>
                <Btn_export_data/>
                <PieChart/>
            </div>
        </div>
        </>
        
    )
}

export default Dmecrf
