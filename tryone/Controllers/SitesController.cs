using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;

namespace tryone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public SitesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string dwhSource = @"Data Source=ICTAPOWERBI;Initial Catalog=ICTA_DWH;Integrated Security=True";
        string cubeSource = @"Provider=SQLNCLI11.1;Data Source=report.icta.fr;Initial Catalog=ICTA_CUBE;Integrated Security=SSPI";




        [HttpGet("sites_status_summary")] //http://localhost:5000/api/Sites/sites_status_summary
        public IEnumerable<Models.Sites.SitesStatusSummary> GetSitesStatusSummary()
        //public JsonResult GetTest2()
        {

            string cubeQuery = "   SELECT NON EMPTY { [Measures].[Site status total] } ON COLUMNS, " +
                "NON EMPTY { ([Site status].[Site - status].[Site - status].ALLMEMBERS * [Site status].[Site - last status YN].[Site - last status YN].ALLMEMBERS ) } " +
                "DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[VERONE] } ) ON COLUMNS FROM [Cube ICTA]) " +
                "WHERE ( [Study].[Study name].&[VERONE] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> site_status = new List<string>();
            List<int> status_total = new List<int>();
            List<string> last_status_total = new List<string>();

            List<string> vrai_site_status = new List<string>();
            List<int> vrai_status_total = new List<int>();
            List<int> vrai_last_status_total = new List<int>();

            while (reader.Read())   // read
            {
                site_status.Add(reader.GetValue(0).ToString());
                last_status_total.Add(reader.GetValue(2).ToString());
                status_total.Add(int.Parse(reader.GetValue(4).ToString()));
            }

            for (int i = 0; i < site_status.Count; i++)
            {
                if (i >= site_status.Count - 1)
                {
                    //Console.WriteLine("aie aie aie"+site_status[i]);
                    //i = i + 1;
                    vrai_site_status.Add(site_status[i]);
                    vrai_status_total.Add(status_total[i]);
                    vrai_last_status_total.Add(status_total[i]);
                }
                else if (site_status[i] == site_status[i + 1])
                {
                    vrai_site_status.Add(site_status[i]);
                    Console.WriteLine("bizzbizz" + i);
                    vrai_status_total.Add(status_total[i] + status_total[i + 1]);
                    if (last_status_total[i] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i]);
                    }
                    else if (last_status_total[i + 1] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i + 1]);
                    }
                    i = i + 1;
                }
                else if (site_status[i] != site_status[i + 1])
                {
                    vrai_site_status.Add(site_status[i]);
                    vrai_status_total.Add(status_total[i]);
                    if (last_status_total[i] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i]);
                    }
                    else if (last_status_total[i] == "No")
                    {
                        vrai_last_status_total.Add(0);
                    }
                    //vrai_last_status_total.Add(status_total[i]);
                }

            }
            //----
            for (int i = 0; i < site_status.Count; i++)
            {
                Console.WriteLine("site : " + site_status[i]);
            }
            for (int i = 0; i < status_total.Count; i++)
            {
                Console.WriteLine("value site : " + status_total[i]);
            }
            Console.WriteLine("---------------------");
            for (int i = 0; i < vrai_site_status.Count; i++)
            {
                Console.WriteLine("vrai : " + vrai_site_status[i]);
            }
            for (int i = 0; i < vrai_status_total.Count; i++)
            {
                Console.WriteLine("value vrai : " + vrai_status_total[i]);
            }
            for (int i = 0; i < vrai_last_status_total.Count; i++)
            {
                Console.WriteLine("value last vrai : " + vrai_last_status_total[i]);
            }

            return Enumerable.Range(0, vrai_site_status.Count).Select(index => new Models.Sites.SitesStatusSummary
            {
                status = vrai_site_status[index],
                status_total = vrai_status_total[index],
                last_status_total = vrai_last_status_total[index]

                //label = vrai_site_status[index],
                //firstvalue = vrai_status_total[index],
                //secondvalue = vrai_last_status_total[index]

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_status_details")] //http://localhost:5000/api/Sites/sites_status_details
        public IEnumerable<Models.Sites.SitesStatusDetails> GetSitesStatusDetails()
        //public JsonResult GetTest2()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Site last status total] } ON COLUMNS, NON EMPTY { ([Site].[Site - code and name].[Site - code and name].ALLMEMBERS * " +
                "[Site status].[Site - status].[Site - status].ALLMEMBERS * [Site status].[Site - status reason].[Site - status reason].ALLMEMBERS * [Status].[Filter].[Date].ALLMEMBERS ) } " +
                "DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Monitoring date].[Monitoring - status date].[All] } ) ON COLUMNS " +
                "FROM ( SELECT ( { [Site status].[Site - last status YN].&[Yes] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA]))) " +
                "WHERE ( [Study].[Study name].&[TANGO], [Site status].[Site - last status YN].&[Yes], [Monitoring date].[Monitoring - status date].[All] ) CELL PROPERTIES " +
                "VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> site = new List<string>();
            List<string> last_status = new List<string>();
            List<string> status_reason = new List<string>();
            List<string> status_date = new List<string>();

            //List<string> vrai_site_status = new List<string>();
            //List<int> vrai_status_total = new List<int>();
            //List<int> vrai_last_status_total = new List<int>();

            while (reader.Read())   // read 0 2 4 10
            {
                site.Add(reader.GetValue(0).ToString());
                last_status.Add(reader.GetValue(2).ToString());
                status_reason.Add(reader.GetValue(4).ToString());
                status_date.Add(reader.GetValue(10).ToString());

                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(6).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine("8 : " + reader.GetValue(8).ToString());
                //Console.WriteLine("9 : " + reader.GetValue(9).ToString());
                //Console.WriteLine("10 : " + reader.GetValue(10).ToString());
            }


            return Enumerable.Range(0, site.Count).Select(index => new Models.Sites.SitesStatusDetails
            {
                site = site[index],
                last_status = last_status[index],
                status_reason = status_reason[index],
                status_date = status_date[index]

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_activities_monitoring")] //http://localhost:5000/api/Sites/sites_activities_monitoring
        public IEnumerable<Models.Sites.SitesActivitiesMonitoring> GetSitesActivitiesMonitoring()
        //public JsonResult GetTest2()
        {

            string cubeQuery = @"SELECT TOP (1000) [sstNameListSecto],[sstrCentreCodeName],[sstrSiteStatus],[sstrFAList]
      ,[dtSelected]
      ,[dtReady]
      ,[dtFirstMonit]
      ,[dtLastMonit]
      ,[dtFirstMonitClosed]
      ,[countMonit]
  FROM[ICTA_DWH].[dbo].[vw_site_activity] where NomEtude = 'TANGO' order by sstrCentreCodeName";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            //cmd.CommandTimeout = 0;

            List<string> division_domain = new List<string>();
            List<string> site = new List<string>();
            List<string> site_status = new List<string>();
            List<string> financial_agreement = new List<string>();
            List<string> selected_date = new List<string>();
            List<string> readyToInclude_date = new List<string>();
            List<string> firstMonitoring_date = new List<string>();
            List<string> lastMonitoring_date = new List<string>();
            List<string> closeOut_date = new List<string>();
            List<int> nb_monitoring = new List<int>();

            try
            {
                var reader = cmd.ExecuteReader(); //Execute query


                while (reader.Read())   // read 0 2 4 10
                {
                    division_domain.Add(reader.GetValue(0).ToString());
                    site.Add(reader.GetValue(1).ToString());
                    site_status.Add(reader.GetValue(2).ToString());
                    financial_agreement.Add(reader.GetValue(3).ToString());
                    selected_date.Add(reader.GetValue(4).ToString());
                    readyToInclude_date.Add(reader.GetValue(5).ToString());
                    firstMonitoring_date.Add(reader.GetValue(6).ToString());
                    lastMonitoring_date.Add(reader.GetValue(7).ToString());
                    closeOut_date.Add(reader.GetValue(8).ToString());
                    nb_monitoring.Add(int.Parse(reader.GetValue(9).ToString()));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : ", e.Message);
            }
            


            return Enumerable.Range(0, division_domain.Count).Select(index => new Models.Sites.SitesActivitiesMonitoring
            {
                division_domain = division_domain[index],
                site = site[index],
                site_status = site_status[index],
                financial_agreement = financial_agreement[index],
                selected_date = selected_date[index],
                readyToInclude_date = readyToInclude_date[index],
                firstMonitoring_date = firstMonitoring_date[index],
                lastMonitoring_date = lastMonitoring_date[index],
                closeOut_date = closeOut_date[index],
                nb_monitoring = nb_monitoring[index],


            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_activities_patients")] //http://localhost:5000/api/Sites/sites_activities_patients
        public IEnumerable<Models.Sites.SitesActivitiesPatients> GetSitesActivitiesPatients()
        //public JsonResult GetTest2()
        {

            string cubeQuery = @"SELECT [sstNameListSecto]
                                ,[sstrCentreCodeName]
                                ,[sstrSiteStatus]
                                ,[sstrFAList]
                                ,[dt1ereIncl]
                                ,[cumulIncl]
                                ,[cumulInRunIn]
                                ,[cumulRando]
                                ,[cumulOnTtt]
                                ,[cumulEndTtt]
                                ,[cumulFU]
                                ,[cumulEndStudy]
                                ,[cumulEarly]
                                ,[cumulSF]
                                  FROM [ICTA_DWH].[dbo].[vw_site_activity] where NomEtude = 'TANGO' order by sstrCentreCodeName";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();



            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;

            List<string> divisionDomain = new List<string>();
            List<string> site = new List<string>();
            List<string> siteStatus = new List<string>();
            List<string> faList = new List<string>();
            List<string> fpfv = new List<string>();
            List<int> included = new List<int>();
            List<int> inRunIn = new List<int>();
            List<int> randomised = new List<int>();
            List<int> onStudyTreatment = new List<int>();
            List<int> enStudyTreatment = new List<int>();
            List<int> followUp = new List<int>();
            List<int> endStudy = new List<int>();
            List<int> earlyDiscontinued = new List<int>();
            List<int> screeningFailure = new List<int>();

            try
            {
                var reader = cmd.ExecuteReader(); //Execute query


                while (reader.Read())   // read 0 2 4 10
                {
                    divisionDomain.Add(reader.GetValue(0).ToString());
                    site.Add(reader.GetValue(1).ToString());
                    siteStatus.Add(reader.GetValue(2).ToString());
                    faList.Add(reader.GetValue(3).ToString());
                    if (reader.IsDBNull(4))
                    {
                        fpfv.Add(" ");
                    }
                    else
                    {
                        fpfv.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    included.Add(int.Parse(reader.GetValue(5).ToString()));
                    inRunIn.Add(int.Parse(reader.GetValue(6).ToString()));
                    randomised.Add(int.Parse(reader.GetValue(7).ToString()));
                    onStudyTreatment.Add(int.Parse(reader.GetValue(8).ToString()));
                    enStudyTreatment.Add(int.Parse(reader.GetValue(9).ToString()));
                    followUp.Add(int.Parse(reader.GetValue(10).ToString()));
                    endStudy.Add(int.Parse(reader.GetValue(11).ToString()));
                    earlyDiscontinued.Add(int.Parse(reader.GetValue(12).ToString()));
                    screeningFailure.Add(int.Parse(reader.GetValue(13).ToString()));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : ", e.Message);
            }


            //     public string divisionDomain { get; set; }
            //public string site { get; set; }
            //public string siteStatus { get; set; }
            //public string faList { get; set; }
            //public string fpfv { get; set; }
            //public int included { get; set; }
            //public int inRunIn { get; set; }
            //public int randomised { get; set; }
            //public int onStudyTreatment { get; set; }
            //public int enStudyTreatment { get; set; }
            //public int followUp { get; set; }
            //public int endStudy { get; set; }
            //public int earlyDiscontinued { get; set; }
            //public int screeningFailure { get; set; }
            return Enumerable.Range(0, divisionDomain.Count).Select(index => new Models.Sites.SitesActivitiesPatients
            {
                divisionDomain = divisionDomain[index],
                site = site[index],
                siteStatus = siteStatus[index],
                faList = faList[index],
                fpfv = fpfv[index],
                included = included[index],
                inRunIn = inRunIn[index],
                randomised = randomised[index],
                onStudyTreatment = onStudyTreatment[index],
                enStudyTreatment = enStudyTreatment[index],
                followUp = followUp[index],
                endStudy = endStudy[index],
                earlyDiscontinued = earlyDiscontinued[index],
                screeningFailure = screeningFailure[index],


            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_activities_sae")] //http://localhost:5000/api/Sites/sites_activities_sae
        public IEnumerable<Models.Sites.SitesActivitiesSAE> GetSitesActivitiesSAE()
        //public JsonResult GetTest2()
        {

            string cubeQuery = @"SELECT [sstrCentreCode]
                                  ,[nbSAE]
                              FROM [ICTA_DWH].[dbo].[vw_site_activity] where NomEtude = 'TANGO' order by sstrCentreCode";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;

            List<string> centreCode = new List<string>();
            List<int> nbSAE = new List<int>();

            try
            {
                var reader = cmd.ExecuteReader(); //Execute query


                while (reader.Read())   // read 0 2 4 10
                {
                    centreCode.Add(reader.GetValue(0).ToString());
                    nbSAE.Add(int.Parse(reader.GetValue(1).ToString()));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : ", e.Message);
            }


            return Enumerable.Range(0, centreCode.Count).Select(index => new Models.Sites.SitesActivitiesSAE
            {
                centreCode = centreCode[index],
                nbSAE = nbSAE[index],


            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_gantt")] //http://localhost:5000/api/Sites/sites_gantt
        public IEnumerable<Models.Sites.SitesGANTT> GetSitesGANTT()
        //public JsonResult GetTest2()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Duration Planned], [Measures].[Duration Actual] } ON COLUMNS, " +
                "NON EMPTY { ([Address].[Country].[Country].ALLMEMBERS * [Milestone GANTT].[Gantt - milestone phase].[Gantt - milestone phase].ALLMEMBERS * " +
                "[Milestone GANTT].[Gantt - start date planned].[Gantt - start date planned].ALLMEMBERS * [Milestone GANTT].[Gantt - start date actual].[Gantt - start date actual].ALLMEMBERS ) } DIMENSION " +
                "PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Milestone GANTT].[Gantt - milestone level].&[Country] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS " +
                "FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Milestone GANTT].[Gantt - milestone level].&[Country] ) CELL PROPERTIES " +
                "VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> country = new List<string>();

            List<string> regulatory_start_date_planned = new List<string>();
            List<string> regulatory_end_date_planned = new List<string>();
            List<string> regulatory_start_date_actual = new List<string>();
            List<string> regulatory_end_date_actual = new List<string>();

            List<string> startup_start_date_planned = new List<string>();
            List<string> startup_end_date_planned = new List<string>();
            List<string> startup_start_date_actual = new List<string>();
            List<string> startup_end_date_actual = new List<string>();

            List<string> coredocs_start_date_planned = new List<string>();
            List<string> coredocs_end_date_planned = new List<string>();
            List<string> coredocs_start_date_actual = new List<string>();
            List<string> coredocs_end_date_actual = new List<string>();

            List<string> siteselection_start_date_planned = new List<string>();
            List<string> siteselection_end_date_planned = new List<string>();
            List<string> siteselection_start_date_actual = new List<string>();
            List<string> siteselection_end_date_actual = new List<string>();

            List<string> initiation_start_date_planned = new List<string>();
            List<string> initiation_end_date_planned = new List<string>();
            List<string> initiation_start_date_actual = new List<string>();
            List<string> initiation_end_date_actual = new List<string>();

            List<string> recruitment_start_date_planned = new List<string>();
            List<string> recruitment_end_date_planned = new List<string>();
            List<string> recruitment_start_date_actual = new List<string>();
            List<string> recruitment_end_date_actual = new List<string>();

            List<string> monitoring_start_date_planned = new List<string>();
            List<string> monitoring_end_date_planned = new List<string>();
            List<string> monitoring_start_date_actual = new List<string>();
            List<string> monitoring_end_date_actual = new List<string>();



            //List<string> start_date_planned = new List<string>();
            //List<string> end_date_planned = new List<string>();
            //List<string> start_date_actual = new List<string>();
            //List<string> end_date_actual = new List<string>();
            string ash = "18Apr2016";

            DateTime test = DateTime.Parse("18Apr2016");
            Console.WriteLine("ancoer before : " + test);

            Console.WriteLine("before : " + test.ToString("yyyy-MM-dd"));

            DateTime b = test.AddDays(7);

            Console.WriteLine("after : " + b.ToString("yyyy-MM-dd"));

            // country => 0
            // type => 2
            // start_date_planned => 4
            // start_date_actual => 6
            // duration planned => 8
            // duration actual => 9
            int i = 0;

            while (reader.Read())   // read 0 2 4 10
            {

                if(reader.GetValue(2).ToString() == "Start-up")
                {
                    if (reader.GetValue(4).ToString() != "")
                    {
                        country.Add(reader.GetValue(0).ToString());
                        startup_start_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        startup_start_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        startup_start_date_actual.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        startup_start_date_actual.Add(" ");
                    }
                    if (reader.GetValue(4).ToString() != "")
                    {
                        startup_end_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(8).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        startup_end_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        startup_end_date_actual.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(9).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        startup_end_date_actual.Add(" ");
                    }
                }
                if (reader.GetValue(2).ToString() == "Core docs")
                {
                    if (reader.GetValue(4).ToString() != "")
                    {
                        coredocs_start_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        coredocs_start_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        coredocs_start_date_actual.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        coredocs_start_date_actual.Add(" ");
                    }
                    if (reader.GetValue(4).ToString() != "")
                    {
                        coredocs_end_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(8).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        coredocs_end_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        coredocs_end_date_actual.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(9).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        coredocs_end_date_actual.Add(" ");
                    }
                }
                if (reader.GetValue(2).ToString() == "Sites selection")
                {
                    if (reader.GetValue(4).ToString() != "")
                    {
                        siteselection_start_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        siteselection_start_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        siteselection_start_date_actual.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        siteselection_start_date_actual.Add(" ");
                    }
                    if (reader.GetValue(4).ToString() != "")
                    {
                        siteselection_end_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(8).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        siteselection_end_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        siteselection_end_date_actual.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(9).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        siteselection_end_date_actual.Add(" ");
                    }
                }
                if (reader.GetValue(2).ToString() == "Regulatory")
                {
                    if (reader.GetValue(4).ToString() != "")
                    {
                        regulatory_start_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        regulatory_start_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        regulatory_start_date_actual.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        regulatory_start_date_actual.Add(" ");
                    }
                    if (reader.GetValue(4).ToString() != "")
                    {
                        regulatory_end_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(8).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        regulatory_end_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        regulatory_end_date_actual.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(9).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        regulatory_end_date_actual.Add(" ");
                    }
                }
                if (reader.GetValue(2).ToString() == "Initiation")
                {
                    if (reader.GetValue(4).ToString() != "")
                    {
                        initiation_start_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        initiation_start_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        initiation_start_date_actual.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        initiation_start_date_actual.Add(" ");
                    }
                    if (reader.GetValue(4).ToString() != "")
                    {
                        initiation_end_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(8).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        initiation_end_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        initiation_end_date_actual.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(9).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        initiation_end_date_actual.Add(" ");
                    }
                }
                if (reader.GetValue(2).ToString() == "Recruitment")
                {
                    if (reader.GetValue(4).ToString() != "")
                    {
                        recruitment_start_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        recruitment_start_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        recruitment_start_date_actual.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        recruitment_start_date_actual.Add(" ");
                    }
                    if (reader.GetValue(4).ToString() != "")
                    {
                        recruitment_end_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(8).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        recruitment_end_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        recruitment_end_date_actual.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(9).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        recruitment_end_date_actual.Add(" ");
                    }
                }
                if (reader.GetValue(2).ToString() == "Monitoring")
                {
                    if (reader.GetValue(4).ToString() != "")
                    {
                        monitoring_start_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        monitoring_start_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        monitoring_start_date_actual.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        monitoring_start_date_actual.Add(" ");
                    }
                    if (reader.GetValue(4).ToString() != "")
                    {
                        monitoring_end_date_planned.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(8).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        monitoring_end_date_planned.Add(" ");
                    }
                    if (reader.GetValue(6).ToString() != "")
                    {
                        monitoring_end_date_actual.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddDays(int.Parse(reader.GetValue(9).ToString())).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        monitoring_end_date_actual.Add(" ");
                    }
                }


                


                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + i + "/*" +DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));

                //Console.WriteLine("8 : " + i + "/*" + DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                //i = i + 1;
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(6).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine("8 : " + reader.GetValue(8).ToString());
                //Console.WriteLine("9 : " + reader.GetValue(9).ToString());
            }


            return Enumerable.Range(0, country.Count).Select(index => new Models.Sites.SitesGANTT
            {
                country = country[index],
                regulatory_start_date_planned = regulatory_start_date_planned[index],
                regulatory_end_date_planned = regulatory_end_date_planned[index],
                regulatory_start_date_actual = regulatory_start_date_actual[index],
                regulatory_end_date_actual = regulatory_end_date_actual[index],

                startup_start_date_planned = startup_start_date_planned[index],
                startup_end_date_planned = startup_end_date_planned[index],
                startup_start_date_actual = startup_start_date_actual[index],
                startup_end_date_actual = startup_end_date_actual[index],


                coredocs_start_date_planned = coredocs_start_date_planned[index],
                coredocs_end_date_planned = coredocs_end_date_planned[index],
                coredocs_start_date_actual = coredocs_start_date_actual[index],
                coredocs_end_date_actual = coredocs_end_date_actual[index],


                siteselection_start_date_planned = siteselection_start_date_planned[index],
                siteselection_end_date_planned = siteselection_end_date_planned[index],
                siteselection_start_date_actual = siteselection_start_date_actual[index],
                siteselection_end_date_actual = siteselection_end_date_actual[index],

                initiation_start_date_planned = initiation_start_date_planned[index],
                initiation_end_date_planned = initiation_end_date_planned[index],
                initiation_start_date_actual = initiation_start_date_actual[index],
                initiation_end_date_actual = initiation_end_date_actual[index],

                recruitment_start_date_planned = recruitment_start_date_planned[index],
                recruitment_end_date_planned = recruitment_end_date_planned[index],
                recruitment_start_date_actual = recruitment_start_date_actual[index],
                recruitment_end_date_actual = recruitment_end_date_actual[index],


                monitoring_start_date_planned = monitoring_start_date_planned[index],
                monitoring_end_date_planned = monitoring_end_date_planned[index],
                monitoring_start_date_actual = monitoring_start_date_actual[index],
                monitoring_end_date_actual = monitoring_end_date_actual[index]


            }).ToArray();

            //return new JsonResult(datatable);
        }

        //[HttpGet("sites_gantt_site")] //http://localhost:5000/api/Sites/sites_gantt_site
        //public IEnumerable<Models.Sites.SitesGANTT> GetSitesGANTTSite()
        ////public JsonResult GetTest2()
        //{

        //    string cubeQuery = "   SELECT NON EMPTY { [Measures].[Duration Planned], [Measures].[Duration Actual] } ON COLUMNS, " +
        //        "NON EMPTY { ([Address].[Country].[Country].ALLMEMBERS * [Milestone GANTT].[Gantt - milestone phase].[Gantt - milestone phase].ALLMEMBERS * [Milestone GANTT].[Gantt - start date planned]." +
        //        "[Gantt - start date planned].ALLMEMBERS * [Milestone GANTT].[Gantt - start date actual].[Gantt - start date actual].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, " +
        //        "MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Milestone GANTT].[Gantt - milestone level].&[Site] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS " +
        //        "FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Milestone GANTT].[Gantt - milestone level].&[Site] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, " +
        //        "FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

        //    AdomdConnection con = new AdomdConnection(cubeSource);
        //    con.Open();

        //    AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
        //    DataTable datatable = new DataTable();
        //    adapt.Fill(datatable);

        //    AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
        //    AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

        //    Console.WriteLine(datatable.Rows.Count);

        //    List<List<string>> country = new List<List<string>>();

        //    List<List<string>> regulatory_start_date_planned = new List<List<string>>();


        //    string ash = "18Apr2016";

        //    DateTime test = DateTime.Parse("18Apr2016");
        //    Console.WriteLine("ancoer before : " + test);

        //    Console.WriteLine("before : " + test.ToString("yyyy-MM-dd"));

        //    DateTime b = test.AddDays(7);

        //    Console.WriteLine("after : " + b.ToString("yyyy-MM-dd"));

        //    // country => 0
        //    // type => 2
        //    // start_date_planned => 4
        //    // start_date_actual => 6
        //    // duration planned => 8
        //    // duration actual => 9
        //    int i = 0;

        //    while (reader.Read())   // read 0 2 4 10
        //    {


        //        //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
        //        //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
        //        //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
        //        //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
        //        //Console.WriteLine("4 : " + i + "/*" +DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));

        //        //Console.WriteLine("8 : " + i + "/*" + DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
        //        //i = i + 1;
        //        //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
        //        //Console.WriteLine("6 : " + reader.GetValue(6).ToString());
        //        //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
        //        //Console.WriteLine("8 : " + reader.GetValue(8).ToString());
        //        //Console.WriteLine("9 : " + reader.GetValue(9).ToString());
        //    }


        //    return Enumerable.Range(0, country.Count).Select(index => new Models.Sites.SitesGANTT
        //    {
        //        country = country[index["f"]],
        //        regulatory_start_date_planned = regulatory_start_date_planned[index]


        //    }).ToArray();

        //    //return new JsonResult(datatable);
        //}


        [HttpGet("sites_timeline")] //http://localhost:5000/api/Sites/sites_timeline
        public IEnumerable<Models.Sites.SitesMilestoneTimeline> GetSitesTimeline()
        //public JsonResult GetTest2()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Milestone total] } ON COLUMNS, " +
                "NON EMPTY { ([Milestone].[Milestone - phase].[Milestone - phase].ALLMEMBERS * [Milestone].[Milestone - milestone].[Milestone - milestone].ALLMEMBERS * " +
                "[Milestone - actual].[Date].[Date].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Milestone].[Milestone - level].&[Study] } ) " +
                "ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Milestone].[Milestone - level].&[Study] ) " +
                "CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> phase = new List<string>();
            List<string> milestone = new List<string>();
            List<string> pre_date = new List<string>();
            List<string> date = new List<string>();
            List<string> post_date = new List<string>();

            //DateTime b = test.AddDays(7);

            while (reader.Read())   // read 0 2 4 10
            {
                
                if (reader.GetValue(4).ToString() == "Unknown")
                {
                    Console.WriteLine("Unknown found");
                }
                else
                {
                    Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                    phase.Add(reader.GetValue(0).ToString());
                    milestone.Add(reader.GetValue(2).ToString());
                    pre_date.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddMonths(-3).ToString("yyyy-MM-dd"));
                    date.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("dddd, dd MMMM yyyy"));
                    post_date.Add(DateTime.Parse(reader.GetValue(4).ToString()).AddMonths(3).ToString("yyyy-MM-dd"));
                }

                //if (reader.GetValue(4).ToString() == "Unknown")
                //{
                //    Console.WriteLine("Unknown found");
                //}
                //else
                //{
                //    Console.WriteLine("minus date : " + DateTime.Parse(reader.GetValue(4).ToString()).AddMonths(-1).ToString("yyyy-MM-dd"));
                //    Console.WriteLine("date : " + DateTime.Parse(reader.GetValue(4).ToString()).ToString("dddd, dd MMMM yyyy"));
                //    Console.WriteLine("add date : " + DateTime.Parse(reader.GetValue(4).ToString()).AddMonths(1).ToString("yyyy-MM-dd"));
                //}
                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(6).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine("8 : " + reader.GetValue(8).ToString());
                //Console.WriteLine("9 : " + reader.GetValue(9).ToString());
                //Console.WriteLine("10 : " + reader.GetValue(10).ToString());
            }


            return Enumerable.Range(0, date.Count).Select(index => new Models.Sites.SitesMilestoneTimeline
            {
                phase = phase[index],
                milestone = milestone[index],
                pre_date = pre_date[index],
                date = date[index],
                post_date = post_date[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_documents")] //http://localhost:5000/api/Sites/sites_documents
        public IEnumerable<Models.Sites.SitesDocuments> GetSitesDocuments()
        //public JsonResult GetTest2()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Document total] } ON COLUMNS, NON EMPTY { ([Document].[Document - group type].[Document - group type].ALLMEMBERS * " +
                "[Document].[Document - conform].[Document - conform].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Document]." +
                "[Document - owner].&[Site] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], " +
                "[Document].[Document - owner].&[Site] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS ";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> label = new List<string>();
            List<string> type = new List<string>();
            List<int> value = new List<int>();

            //DateTime b = test.AddDays(7);

            while (reader.Read())   // read 0 2 4 10
            {
                label.Add(reader.GetValue(0).ToString());
                type.Add(reader.GetValue(2).ToString());
                value.Add(int.Parse(reader.GetValue(4).ToString()));

                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(6).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine("8 : " + reader.GetValue(8).ToString());
                //Console.WriteLine("9 : " + reader.GetValue(9).ToString());
                //Console.WriteLine("10 : " + reader.GetValue(10).ToString());
            }


            return Enumerable.Range(0, label.Count).Select(index => new Models.Sites.SitesDocuments
            {
                label = label[index],
                type = type[index],
                value = value[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_documents_details")] //http://localhost:5000/api/Sites/sites_documents_details
        public IEnumerable<Models.Sites.SitesDocumentsDetails> GetSitesDocumentsDetails()
        //public JsonResult GetTest2()
        {

            string cubeQuery = @"SELECT TOP (1000) 
      [sstrCentreCodeName]
      ,[sstrDocumentType]
      ,[sstrConformYN]
      ,[dtSending]
      ,[dtCROReceipt]
	  ,[dtSignature]
      ,[dtShipmentToSponsor]
  FROM [ICTA_DWH].[dbo].[vw_site_document] where NomEtude = 'TANGO'  order by sstrCentreCodeName";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            List<string> site = new List<string>();
            List<string> type = new List<string>();
            List<string> conform = new List<string>();
            List<string> sendingDate = new List<string>();
            List<string> receiptDate = new List<string>();
            List<string> signatureDate = new List<string>();
            List<string> shipmentToSponsorDate = new List<string>();



            SqlCommand cmd = new SqlCommand(cubeQuery, con);
            //cmd.CommandTimeout = 0;

            try
            {
                SqlDataReader reader = cmd.ExecuteReader(); //Execute query

                while (reader.Read())   // read 0 2 4 10
                {
                    site.Add(reader.GetValue(0).ToString());
                    type.Add(reader.GetValue(1).ToString());
                    conform.Add(reader.GetValue(2).ToString());
                    if (reader.GetValue(3).ToString() == "" || reader.GetValue(3).ToString() == " " || reader.GetValue(3).ToString() == null)
                    {
                        sendingDate.Add(" ");
                    }
                    else
                    {
                        sendingDate.Add(DateTime.Parse(reader.GetValue(3).ToString()).ToString("yyyy-MM-dd"));
                    }
                    if (reader.GetValue(4).ToString() == "" || reader.GetValue(4).ToString() == " " || reader.GetValue(4).ToString() == null)
                    {
                        receiptDate.Add(" ");
                    }
                    else
                    {
                        receiptDate.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));

                    }
                    if (reader.GetValue(5).ToString() == "" || reader.GetValue(5).ToString() == " " || reader.GetValue(5).ToString() == null)
                    {
                        signatureDate.Add(" ");
                    }
                    else
                    {
                        signatureDate.Add(DateTime.Parse(reader.GetValue(5).ToString()).ToString("yyyy-MM-dd"));

                    }
                    if (reader.GetValue(6).ToString() == "" || reader.GetValue(6).ToString() == " " || reader.GetValue(6).ToString() == null)
                    {
                        shipmentToSponsorDate.Add(" ");
                    }
                    else
                    {
                        shipmentToSponsorDate.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));

                    }


                }
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR : " + e.Message);
            }
            


            return Enumerable.Range(0, site.Count).Select(index => new Models.Sites.SitesDocumentsDetails
            {
                site = site[index],
                type = type[index],
                conform = conform[index],
                sendingDate = sendingDate[index],
                receiptDate = receiptDate[index],
                signatureDate = signatureDate[index],
                shipmentToSponsorDate = shipmentToSponsorDate[index]


            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_fa_type")] //http://localhost:5000/api/Sites/sites_fa_type
        public IEnumerable<Models.Sites.SitesFAType> GetSitesFAType()
        //public JsonResult GetTest2()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Financial agreement site] } ON COLUMNS, " +
                "NON EMPTY { ([Financial agreement].[Financial agreement - type].[Financial agreement - type].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
                "FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA]) WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, " +
                "FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> label = new List<string>();
            List<string> type = new List<string>();
            List<int> value = new List<int>();

            //DateTime b = test.AddDays(7);

            while (reader.Read())   // read 0 2 4 10
            {
                label.Add(reader.GetValue(0).ToString());
                value.Add(int.Parse(reader.GetValue(2).ToString()));




                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(6).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine("8 : " + reader.GetValue(8).ToString());
                //Console.WriteLine("9 : " + reader.GetValue(9).ToString());
                //Console.WriteLine("10 : " + reader.GetValue(10).ToString());
            }


            return Enumerable.Range(0, label.Count).Select(index => new Models.Sites.SitesFAType
            {
                label = label[index],
                type = "fds",
                value = value[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("sites_financial_agremment_details")] //http://localhost:5000/api/Sites/sites_financial_agremment_details
        public IEnumerable<Models.Sites.SitesFADetails> GetSitesFADetails()
        //public JsonResult GetTest2()
        {

            string cubeQuery = @"SELECT TOP (1000) 
      [sstrCentreCodeName]
	  ,[sstrFinancialType]
	  ,[sstrBeneficiaryType]
	  ,[sstrFeesType]
	  ,[sstrDocumentType]
      ,[dtSending]
      ,[dtCROReceipt]
      ,[dtSignature]
      ,[dtShipmentToSponsor]
  FROM [ICTA_DWH].[dbo].[vw_site_financial_agreement] where NomEtude = 'TANGO'  order by sstrCentreCodeName";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            SqlCommand cmd = new SqlCommand(cubeQuery, con);
            //cmd.CommandTimeout = 0;

            List<string> site = new List<string>();
            List<string> type = new List<string>();
            List<string> beneficiaryType = new List<string>();
            List<string> feesType = new List<string>();
            List<string> documentType = new List<string>();
            List<string> sendingDate = new List<string>();
            List<string> receiptDate = new List<string>();
            List<string> signatureDate = new List<string>();
            List<string> shipmentToSponsorDate = new List<string>();

            try
            {
                SqlDataReader reader = cmd.ExecuteReader(); //Execute query

                while (reader.Read())   // read 0 2 4 10
                {
                    site.Add(reader.GetValue(0).ToString());
                    type.Add(reader.GetValue(1).ToString());
                    beneficiaryType.Add(reader.GetValue(2).ToString());
                    feesType.Add(reader.GetValue(3).ToString());
                    documentType.Add(reader.GetValue(4).ToString());
                    if (reader.GetValue(5).ToString() == "" || reader.GetValue(5).ToString() == " " || reader.GetValue(5).ToString() == null)
                    {
                        sendingDate.Add(" ");
                    }
                    else
                    {
                        sendingDate.Add(DateTime.Parse(reader.GetValue(5).ToString()).ToString("yyyy-MM-dd"));
                    }
                    if (reader.GetValue(6).ToString() == "" || reader.GetValue(6).ToString() == " " || reader.GetValue(6).ToString() == null)
                    {
                        receiptDate.Add(" ");
                    }
                    else
                    {
                        receiptDate.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));

                    }
                    if (reader.GetValue(7).ToString() == "" || reader.GetValue(7).ToString() == " " || reader.GetValue(7).ToString() == null)
                    {
                        signatureDate.Add(" ");
                    }
                    else
                    {
                        signatureDate.Add(DateTime.Parse(reader.GetValue(7).ToString()).ToString("yyyy-MM-dd"));

                    }
                    if (reader.GetValue(8).ToString() == "" || reader.GetValue(8).ToString() == " " || reader.GetValue(8).ToString() == null)
                    {
                        shipmentToSponsorDate.Add(" ");
                    }
                    else
                    {
                        shipmentToSponsorDate.Add(DateTime.Parse(reader.GetValue(8).ToString()).ToString("yyyy-MM-dd"));

                    }


                }
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR : " + e.Message);
            }
            

            


            return Enumerable.Range(0, site.Count).Select(index => new Models.Sites.SitesFADetails
            {
                site = site[index],
                type = type[index],
                beneficiaryType = beneficiaryType[index],
                feesType = feesType[index],
                documentType = documentType[index],
                sendingDate = sendingDate[index],
                receiptDate = receiptDate[index],
                signatureDate = signatureDate[index],
                shipmentToSponsorDate = shipmentToSponsorDate[index]


            }).ToArray();

            //return new JsonResult(datatable);
        }

    }
}
