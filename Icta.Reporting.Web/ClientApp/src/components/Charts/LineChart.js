import React from 'react'
import { useState, useEffect } from 'react';
import axios from 'axios';
import {Chart as ChartJS,ArcElement,LineElement,BarElement,PointElement,BarController,BubbleController,DoughnutController,LineController,PieController,PolarAreaController,RadarController,ScatterController,CategoryScale,LinearScale,LogarithmicScale,RadialLinearScale,TimeScale,TimeSeriesScale,Decimation,Filler,Legend,Title,Tooltip,SubTitle} from 'chart.js';
// import {Chart as ChartJS,CategoryScale,LinearScale,BarElement,Title,Tooltip,Legend} from 'chart.js';
import { Bar, Doughnut, Line, Pie, PolarArea, Radar, Bubble, Scatter } from 'react-chartjs-2';
// ChartJS.register(CategoryScale,LinearScale,BarElement,Title,Tooltip,Legend);
ChartJS.register(ArcElement,LineElement,BarElement,PointElement,BarController,BubbleController,DoughnutController,LineController,PieController,PolarAreaController,RadarController,ScatterController,CategoryScale,LinearScale,LogarithmicScale,RadialLinearScale,TimeScale,TimeSeriesScale,Decimation,Filler,Legend,Title,Tooltip,SubTitle);


const options = {
    indexAxis: 'x',
    elements: {
      bar: {
        borderWidth: 2,
      },
    },
    responsive: true,
    maintainAspectRatio: false,
    intersect: true,
    pointRadius: 0,
    plugins: {
      legend: {
        position: 'bottom',
      },
      // title: {
      //   display: true,
      //   text: 'Lattitude & Longitude of everyone',
      // },
    },
  };

const LineChart =(prop) => {
  const [data, setData] = useState({
    labels:[],
    datasets: [],
  });
  useEffect(()=> {
     const fetchData= async()=> {
         const url = prop.url //https://api.github.com/users/zellwk/repos?sort=pushed https://jsonplaceholder.typicode.com/users  https://localhost:44316/api/values
         const labelSet = []
         const dataSet1 = [];
         const dataSet2 = [];
         const dataSet3 = [];
         
       await fetch(url).then((data) => data.json()).then((res) => {
          for (const val of res) {
              labelSet.push(val.label)
              dataSet1.push(val.firstvalue);  //val.id for Int or parseInt(val.address.geo.lat) for String  val.name  val.score
              dataSet2.push(val.secondvalue)  //val.postId  parseInt(val.address.geo.lng)
              dataSet3.push(val.thirdvalue)
                //val.name ou val.address.zipcode ou val.address.geo.lat
          }
       })

      //----------------------------------------------Ajouter une deuxième API
      //  await fetch(url).then((data) => data.json()).then((res) => {
      //     for (const val of res) {
      //         //dataSet2.push(val.ORDINAL_POSITION)  //val.postId  parseInt(val.address.geo.lng)
      //         // labelSet.push(val.name)  //val.name ou val.address.zipcode ou val.address.geo.lat
      //     }
      //     console.log("arr Data : ", dataSet1, dataSet2)
      // })

      //-----------------------------------------------Dataset
       setData({
        labels: labelSet,
        datasets: [
          {
            label: prop.label1,
            data:dataSet1,
            borderColor: '#01B8AA',
            backgroundColor: 'rgba(75,192,192,0.2)',
            // fill: true,
            // borderRadius: 10,
            // borderWidth: 10,
            hoverBorderWidth: 5,
            //tension: 0.1,
          },
          {
            label: prop.label2,
            data:dataSet2,
            borderColor: '#374649',
            backgroundColor: 'rgba(75,192,192,0.2)',
            // fill: true,
            // borderRadius: 10,
            // borderWidth: 10,
            hoverBorderWidth: 5,
          },
          {
            label: prop.label3,
            data:dataSet3,
            borderColor: '#FD625E',
            backgroundColor: 'rgba(253, 98, 94, 0.5)',
            // fill: true,
            // borderRadius: 10,
            // borderWidth: 10,
            hoverBorderWidth: 5,
          },
        ],
      })

      } //fetchdata
      fetchData(); //on appelle fetchdata


  },[])
  //Bar, Doughnut, Line, Pie, PolarArea, Radar, Bubble, Scatter
 
  return(
      <div style={{width:'100%', height:'35vh'}}>
          <Line data={data} options={options}/> 
      </div>)
}



export default LineChart
