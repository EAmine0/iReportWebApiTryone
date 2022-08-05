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
    plugins: {
      legend: {
        position: 'right',
      },
      title: {
        display: true,
        text: 'Lattitude & Longitude of everyone',
      },
    },
  };

const PieChart =() => {
    const [data, setData] = useState({
        labels:['Sunday','Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        datasets: [
          {
            label: 'Dataset 1',
            data:[],
            borderColor: 'rgb(255, 99, 132)',
            backgroundColor: 'rgba(25, 90, 13, 0.5)',
          },
          {
            label: 'Dataset 2',
            data:[],
            borderColor: 'rgb(53, 162, 235)',
            backgroundColor: 'rgba(53, 162, 235, 0.5)',
          },
        ],
      });
    useEffect(()=> {
       const fetchData= async()=> {
           const url = 'https://jsonplaceholder.typicode.com/users' //https://api.github.com/users/zellwk/repos?sort=pushed https://jsonplaceholder.typicode.com/users
           const labelSet = []
           const dataSet1 = [];
           const dataSet2 = [];
         await fetch(url).then((data)=> {
             console.log("Api data", data)
             const res = data.json();
             return res
         }).then((res) => {
             console.log("ressss", res)
            for (const val of res) {
                dataSet1.push(parseInt(val.address.geo.lat));  //val.id for Int or parseInt(val.address.geo.lat) for String
                dataSet2.push(parseInt(val.address.geo.lng))  //val.postId  parseInt(val.address.geo.lng)
                labelSet.push(val.username)  //val.name ou val.address.zipcode ou val.address.geo.lat
            }
            setData({
                // labels:['Sunday','Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', "jdkljf", "jlkkj"],
                labels: labelSet,
                datasets: [
                  {
                    label: 'Lattitude',
                    data:dataSet1,
                    borderColor: 'rgba(75,192,192,1)',
                    backgroundColor: 'rgba(75,192,192,0.2)',
                    fill: true, //Pour Line
                    cutout: '5%',        //
                    circumference: 180,  //  Pour Doughnut ou Pie
                    rotation: 270,      //
                    // borderRadius: 10,
                    // borderWidth: 10,
                    hoverBorderWidth: 5,
                  },
                  {
                    label: 'Longitude',
                    data:dataSet2,
                    borderColor: '#742774',
                    backgroundColor: 'rgba(75,192,192,0.2)',
                    fill: true, //Pour Line
                    // cutout: '75%',        //
                    circumference: 180,  //  Pour Doughnut ou Pie afin d'avoir un half-doughnut ou sinon voir "reactjs chartjs gauge" sur @
                    rotation: 270,      //
                    // borderRadius: 10,
                    hoverBorderWidth: 5,
                  },
                ],
              })
            console.log("arrData", dataSet1, dataSet2)
         }).catch(e => {
                console.log("error", e)
            })
        }
        
        fetchData();
    },[])
    //Bar, Doughnut, Line, Pie, PolarArea, Radar, Bubble, Scatter
   
    return(
        <div style={{width:'100%', height:'40vh'}}>
            {
                console.log("data", data)
            }
            <Pie data={data} options={options}/> 
         </div>)
}


export default PieChart
