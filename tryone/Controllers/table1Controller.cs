using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;


//namespace tryone.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class table1Controller : ControllerBase
//    {
//        private readonly IConfiguration _configuration;

//        public table1Controller(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

        
//        [HttpGet("table1")]
//        public JsonResult GetTable1() // ou object
//        {
//            string query = @"SELECT * FROM [testdb].[dbo].[table1] ";
//            DataTable table = new DataTable();

//            string sqlDataSource = _configuration.GetConnectionString("TestdbAppCon");
//            SqlDataReader myReader;

//            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
//            {
//                myConn.Open();
//                using (SqlCommand myCommand = new SqlCommand(query, myConn))
//                {
//                    myReader = myCommand.ExecuteReader();
//                    table.Load(myReader);

//                    myReader.Close();
//                    myConn.Close();
//                }
//            }
//            return new JsonResult(table);

//            //SELECT * FROM [testdb].[dbo].[table1]
//            //SELECT * from INFORMATION_SCHEMA.COLUMNS
//            //SELECT * FROM [ICTA_DWH].[fact].[CURVE] where etudes_i_id = 1007 and label = 'Theoretical' order by intOrderBy asc //Pour curve of inclusion

//            SqlConnection con = new SqlConnection(@"server=sqldev; database=ZEPHYR; Integrated Security=True;");
//            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [testdb].[dbo].[table1]", con);   
//            DataTable dt = new DataTable();
//            da.Fill(dt);
//            Console.WriteLine($"here table1 : ");
//            return new JsonResult(dt);
//        }

//        [HttpGet("table2")]
//        public IEnumerable<Models.array> GetTable2()
//        {
//            string source = "Data Source=ICTA044;Initial Catalog=testdb;Integrated Security=True";
//            string query = "SELECT * FROM [testdb].[dbo].[table2]";

//            SqlConnection con = new SqlConnection(source);
//            con.Open();
//            SqlDataAdapter adapt = new SqlDataAdapter(query, con); 
//            DataTable datatable = new DataTable();
//            adapt.Fill(datatable);
//            SqlCommand displayCommand = new SqlCommand(query, con);
//            SqlDataReader rd = displayCommand.ExecuteReader();







//            int count = 0;
//            string[] cars = new string[datatable.Columns.Count];

//            //for (int i=0; i< datatable.Columns.Count; i++)
//            //{
//            //    cars[i] = rd.GetName(i).ToString();
//            //    //Console.WriteLine(rd.GetName(i).ToString());
//            //    //Console.WriteLine(cars[i]);
//            //}

//            //for (int i = 0; i < datatable.Columns.Count; i++)
//            //{
//            //    //Console.WriteLine("for avant : " + i);
//            //    while (rd.Read())
//            //    {
//            //        //Object[] test = new Object[rd.FieldCount];
//            //        //Console.WriteLine(" zer : " + rd.GetValues().ToString());
//            //        for (int j = 0; i < 2; j++)
//            //        {
//            //            Console.WriteLine("avant : " + j);
//            //            Console.WriteLine($"{rd.GetValue(j).ToString()}");
//            //            Console.WriteLine("après : " + j);
//            //            count = count + int.Parse(rd.GetValue(2).ToString());
//            //        }

//            //        //i = i + 1;
//            //    }
//            //    //Console.WriteLine("for après : " + i);
//            //}
//            //Console.WriteLine($"Column Name : {rd.GetName(2).ToString()}");

//            //while (rd.Read())
//            //{
//            //    //Object[] test = new Object[rd.FieldCount];
//            //    //Console.WriteLine(" zer : " + rd.GetValues().ToString());

//            //    Console.WriteLine($"{rd.GetValue(1).ToString()} : {rd.GetValue(2).ToString()}");
//            //    count = count + int.Parse(rd.GetValue(2).ToString());

//            //}
//            //Console.WriteLine("Total goals : " + count);
//            //int i = 0;

//            //while (i< 4)
//            //{
//            //    int j = 0;
//            //    while (rd.Read())
//            //    {
//            //        Console.WriteLine("j : "+j);
//            //        j++;

//            //    }
//            //    Console.WriteLine("i : "+i);
//            //    i++;
//            //}

//            Console.WriteLine($"Column Name : {rd.GetName(2).ToString()}");
//            //Console.WriteLine($"Row value : {rd.getvalu}");
            

//            var rng = new Random();
//            //return Enumerable.Range(0, 2).Select(index => new Models.array
//            {
//            }).ToArray();
//        }

//        [HttpGet("site_status")]
//        //public IEnumerable<Models.array> GetTable3()
//        {
//            string source = "Data Source=ICTA044;Initial Catalog=testdb;Integrated Security=True";
//            string query = @"SELECT sum([statusIdentified]) as  Identified
//                                    ,sum([statusRefused]) as  Refused
//                                    ,sum([statusOnFeasibility]) as  OnFeasibility
//                                    ,sum([statusSelected]) as  Selected
//                                    ,sum([statusQualified]) as  Qualified
//                                    ,sum([statusInitiated]) as  Initiated
//                                    ,sum([statusCancelled]) as  Cancelled
//                                    ,sum([statusReadyToInclude]) as  ReadyToInclude
//                                    ,sum([statusActive]) as  Active
//                                FROM[testdb].[dbo].[site_status_test]";

//            SqlConnection con = new SqlConnection(source);
//            con.Open();
//            SqlDataAdapter adapt = new SqlDataAdapter(query, con);
//            DataTable datatable = new DataTable();
//            adapt.Fill(datatable);
//            SqlCommand displayCommand = new SqlCommand(query, con);
//            SqlDataReader rd = displayCommand.ExecuteReader();

//            string[] column_name = new string[datatable.Columns.Count];
//            int[] status_total = new int[datatable.Columns.Count];

//            //----------------------------Afficher toute la table
//            //while (rd.Read())
//            //{
//            //    Object[] column = new Object[datatable.Columns.Count];
//            //    rd.GetValues(column);
//            //    foreach (var item in column)
//            //    {
//            //        Console.Write($"{item} ");
//            //    }
//            //    Console.WriteLine();
//            //}
//            //--------------------------------------------------

            
//            while (rd.Read())
//            {
//                for (int i = 0; i < datatable.Columns.Count; i++)
//                {
//                    column_name[i] = rd.GetName(i).ToString();
//                    status_total[i] = int.Parse(rd.GetValue(i).ToString());
//                }
//            }

//            var rng = new Random();
//            return Enumerable.Range(0, datatable.Columns.Count).Select(index => new Models.array
//            {
//                label = column_name[index],
//                firstvalue = status_total[index],
//                secondvalue = status_total[rng.Next(status_total.Length)]
//            }).ToArray();
//        }
//    }
//}

//int azer = 21;
//string truc = "bien ou bien ";
//Console.WriteLine($"ta vu => {truc} ou {azer} ");
