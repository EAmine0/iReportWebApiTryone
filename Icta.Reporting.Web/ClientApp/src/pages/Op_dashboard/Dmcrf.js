import React from 'react'
import Sidebar from '../../components/Sidebar';
import Topbar from '../../components/Topbar'
import { TopbarOpDash } from '../../components/TopbarData';
import BarChart from '../../components/Charts/BarChart';
import LineChart from '../../components/Charts/LineChart';
import Btn_export_data from '../../components/Btn_export_data';

function Dmcrf() {
    return (
        <>
        {/* <div className='title_page'>
            Operational Dashboard
        </div> */}
        {/* <Topbar test={TopbarOpDash}/> */}
        {/* <Sidebar/> */}
        <div className='box_container'>
            <div className='block'>
                <div className='title_block'>
                    Site status
                </div>
                <Btn_export_data/>
                <BarChart url='http://localhost:5000/api/OpDashboard/site_status' label1='Status total' label2='Last status total'/>
            </div>
            <div className='block'>
                <div className='title_block'>
                    Patient status
                </div>
                <Btn_export_data/>
                <LineChart/>
            </div>
            
        </div>
        </>
        
    )
}

export default Dmcrf
