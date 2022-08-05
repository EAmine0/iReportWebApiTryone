using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace tryone.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurveController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CurveController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly string dwhSource = "Data Source=ICTAPOWERBI;Initial Catalog=ICTA_DWH;Integrated Security=True";
        string cubeSource = @"Provider=SQLNCLI11.1;Data Source=report.icta.fr;Initial Catalog=ICTA_CUBE;Integrated Security=SSPI";
        //Data Source=ICTAPOWERBI;Initial Catalog=ICTA_DWH;Integrated Security=True



        [HttpGet("testcurve")] //http://localhost:5000/api/Curve/testcurve
        public IEnumerable<Models.Curve.testCurve> GetTest()
        //public JsonResult GetTest2()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Nb Patient] } ON COLUMNS, " +
                "NON EMPTY { ([Curve].[Month].[Month].ALLMEMBERS * [Curve].[Label].[Label].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
                "FROM ( SELECT ( { [Study].[Study name].&[047SIMULTI] } ) ON COLUMNS FROM [Cube ICTA]) " +
                "WHERE ( [Study].[Study name].&[047SIMULTI] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            var cmd = new AdomdCommand(cubeQuery, con);
            var reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine("le reader est : "+reader.GetType());

            List<string> month = new List<string>();
            List<int> included = new List<int>();
            List<int> randomised = new List<int>();
            List<int> theoretical = new List<int>();

            Console.WriteLine("reader + read"+reader.Read());

            if (reader.Read())   // read
            {
                if (reader.GetValue(2).ToString() == "Included")
                {
                    included.Add(int.Parse(reader.GetValue(4).ToString()));
                }
                if (reader.GetValue(2).ToString() == "Randomised")
                {
                    randomised.Add(int.Parse(reader.GetValue(4).ToString()));
                }
                if (reader.GetValue(2).ToString() == "Theoretical")
                {
                    theoretical.Add(int.Parse(reader.GetValue(4).ToString()));
                    month.Add(reader.GetValue(0).ToString());
                }
            }

            return Enumerable.Range(0, 5).Select(index => new Models.Curve.testCurve
            {
                date = month[index],
                included = included[index],
                randomised = randomised[index],
                theoretical = theoretical[index]

            }).ToArray();

            //return new JsonResult(datatable);
        }
    }
}
