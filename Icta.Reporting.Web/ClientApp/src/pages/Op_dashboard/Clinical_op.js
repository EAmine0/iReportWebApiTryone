import React from 'react'
import Topbar from '../../components/Topbar'
import { TopbarOpDash } from '../../components/TopbarData';

import BarChart from '../../components/Charts/BarChart';
import DoughnutChart from '../../components/Charts/DoughnutChart';
import LineChart from '../../components/Charts/LineChart';
import PieChart from '../../components/Charts/PieChart';
import Btn_export_data from '../../components/Btn_export_data';

function Clinical_op() {
    return (
        <>
        {/* <div className='title_page'>
            Operational Dashboard
        </div> */}
        {/* <Topbar test={TopbarOpDash}/> */}

        <div className='box_container'>
            <div className='block' style={{width: "35%"}}>
                <div className='title_block'>
                    Site status
                </div>
                <Btn_export_data/>
                <BarChart url='http://localhost:5000/api/OpDashboard/site_status' label1='Status total' label2='Last status total'/>
            </div>
            <div className='block' style={{width: "35%"}}>
                <div className='title_block'>
                    Patient status
                </div>
                <Btn_export_data/>
                <BarChart url='http://localhost:5000/api/OpDashboard/patient_status' label1='Status total' label2='Last status total'/>
            </div>
            <div className='block' style={{width: "76.2%"}}>
                <div className='title_block'>
                    Curve of inclusion
                </div>
                <Btn_export_data/>
                <LineChart url='http://localhost:5000/api/OpDashboard/curve' label1='Included' label2='Randomised' label3='Theoretical'/>
            </div>
            <div className='block' style={{width: "15%"}}>
                <div className='title_block'>
                    Documents
                </div>
                <div>
                    <br/>
                    Received : 1423 <br/>
                    Default unresolved : 223
                    <br/>
                    <br/>
                </div>
                <Btn_export_data/>
                <DoughnutChart url='http://localhost:5000/api/OpDashboard/documents_conformity' legend='Conformity'/>
            </div>
            <div className='block' style={{width: "15%"}}>
                <div className='title_block'>
                    Safety
                </div>
                <Btn_export_data/>
                <DoughnutChart url='http://localhost:5000/api/OpDashboard/documents_conformity' legend='AE/SAE'/>
            </div>
            <div className='block' style={{width: "34%"}}>
                <div className='title_block'>
                    Safety
                </div>
                <Btn_export_data/>
                <BarChart url='http://localhost:5000/api/OpDashboard/patient_status' label1='Status total' label2='Last status total'/>
            </div>
        </div>
        
        </>
        
    )
}

export default Clinical_op
