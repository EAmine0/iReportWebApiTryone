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
    public class PatientsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PatientsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string dwhSource = @"Data Source=ICTAPOWERBI;Initial Catalog=ICTA_DWH;Integrated Security=True";
        string cubeSource = @"Provider=SQLNCLI11.1;Data Source=report.icta.fr;Initial Catalog=ICTA_CUBE;Integrated Security=SSPI";



        [HttpGet("patients_status_summary")] //http://localhost:5000/api/Patients/patients_status_summary
        public IEnumerable<Models.Patients.PatientsStatusSummary> GetPatientsStatusSummary()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Patient status total], [Measures].[Patient last status total] } ON COLUMNS, NON EMPTY { ([Patient status].[Patient - status]." +
                "[Patient - status].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])" +
                " WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            List<string> status = new List<string>();
            List<int> status_total = new List<int>();
            List<int> last_status_total = new List<int>();


            while (reader.Read())   // read
            {
                status.Add(reader.GetValue(0).ToString());
                status_total.Add(int.Parse(reader.GetValue(2).ToString()));
                last_status_total.Add(int.Parse(reader.GetValue(3).ToString()));

                
            }


            return Enumerable.Range(0, status.Count).Select(index => new Models.Patients.PatientsStatusSummary
            {
                status = status[index],
                status_total = status_total[index],
                last_status_total = last_status_total[index]


            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("patient_inclusion_period")] //http://localhost:5000/api/Patients/patient_inclusion_period
        public IEnumerable<Models.Patients.PatientInclusionPeriod> GetPatientInclusionPeriod()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Nb Patient] } ON COLUMNS, " +
                "NON EMPTY { ([Curve].[Month].[Month].ALLMEMBERS * [Curve].[Label].[Label].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
                "FROM ( SELECT ( { [Study].[Study name].&[047SIMULTI] } ) ON COLUMNS FROM [Cube ICTA]) " +
                "WHERE ( [Study].[Study name].&[047SIMULTI] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            //AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new AdomdCommand(cubeQuery, con);
            var reader = cmd.ExecuteReader(); //Execute query

            //Console.WriteLine(datatable.Rows.Count);

            List<string> month = new List<string>();
            List<int> included = new List<int>();
            List<int> randomised = new List<int>();
            List<int> theoretical = new List<int>();

            while (reader.Read())   // read
            {
                //included.Add(int.Parse(reader.GetValue(2).ToString()));
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
                //Console.WriteLine(reader.GetValue(3).ToString());
            }

            return Enumerable.Range(0, 18).Select(index => new Models.Patients.PatientInclusionPeriod
            {
            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("patient_recruitment_details")] //http://localhost:5000/api/Patients/patient_recruitment_details
        public IEnumerable<Models.Patients.PatientRecruitmentDetails> GetPatientRecruitmentDetails()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Patient last status total] } ON COLUMNS, NON EMPTY { ([Patient].[Patient - number]." +
                "[Patient - number].ALLMEMBERS * [Patient status].[Patient - status].[Patient - status].ALLMEMBERS * [Patient status].[Patient - status reason]." +
                "[Patient - status reason].ALLMEMBERS * [Status].[Filter].[Date].ALLMEMBERS * [Site].[Site - name].[Site - name].ALLMEMBERS ) } DIMENSION PROPERTIES " +
                "MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA]) WHERE ( [Study].[Study name].&" +
                "[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();


            var cmd = new AdomdCommand(cubeQuery, con);
            var reader = cmd.ExecuteReader();

            List<string> number = new List<string>();
            List<string> lastStatus = new List<string>();
            List<string> statusReason = new List<string>();
            List<string> date = new List<string>();
            List<string> referralParticipant = new List<string>();

            while (reader.Read())   // read
            {
                if (int.Parse(reader.GetValue(14).ToString()) == 1)
                {
                    number.Add(reader.GetValue(0).ToString());
                    lastStatus.Add(reader.GetValue(2).ToString());
                    statusReason.Add(reader.GetValue(4).ToString());
                    if (reader.GetValue(10).ToString() == "" || reader.GetValue(10).ToString() == " " || reader.GetValue(10).ToString() == null)
                    {
                        date.Add(" ");
                    }
                    else
                    {
                        date.Add(DateTime.Parse(reader.GetValue(10).ToString()).ToString("yyyy-MM-dd"));
                    }
                    referralParticipant.Add(reader.GetValue(12).ToString());
                }
               
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
                //Console.WriteLine("10 : " + reader.GetValue(11).ToString());
                //Console.WriteLine("10 : " + reader.GetValue(12).ToString());
                //Console.WriteLine("10 : " + reader.GetValue(13).ToString());
                //Console.WriteLine("10 : " + reader.GetValue(14).ToString());

            }

            foreach (var item in number)
            {
                Console.WriteLine(item.ToString());
            }

            return Enumerable.Range(0, number.Count).Select(index => new Models.Patients.PatientRecruitmentDetails
            {
                number = number[index],
                lastStatus = lastStatus[index],
                statusReason = statusReason[index],
                date = date[index],
                referralParticipant = referralParticipant[index]

            }).ToArray();

        }

        [HttpGet("patient_recruitment_persite")] //http://localhost:5000/api/Patients/patient_recruitment_persite
        public IEnumerable<Models.Patients.PatientRecruitmentPerSite> GetPatientRecruitmentPerSite()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Patient status total] } ON COLUMNS, NON EMPTY { ([Site].[Site - code and name].[Site - code and name].ALLMEMBERS * " +
                "[Patient status].[Patient - status].[Patient - status].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( " +
                "SELECT ( { [Patient status].[Patient - status].[Included], [Patient status].[Patient - status].[Randomised], [Patient status].[Patient - status].[Early discontinued] } ) ON COLUMNS " +
                "FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, " +
                "FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader();


            List<string> label = new List<string>();
            List<string> type = new List<string>();
            List<int> value = new List<int>();

            while (reader.Read())   // read
            {
                label.Add(reader.GetValue(0).ToString());
                type.Add(reader.GetValue(2).ToString());
                value.Add(int.Parse(reader.GetValue(4).ToString()));



                //Console.WriteLine("1 : "+reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }

            return Enumerable.Range(0, label.Count).Select(index => new Models.Patients.PatientRecruitmentPerSite
            {
                label = label[index],
                type = type[index],
                value = value[index]

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("patient_recruitment_percountry")] //http://localhost:5000/api/Patients/patient_recruitment_percountry
        public IEnumerable<Models.Patients.PatientRecruitmentPerCountry> GetPatientRecruitmentPerCountry()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Patient status total] } ON COLUMNS, NON EMPTY { ([Address].[Country].[Country].ALLMEMBERS * [Patient status]." +
                "[Patient - status].[Patient - status].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Patient status].[Patient - status]." +
                "[Included], [Patient status].[Patient - status].[Randomised], [Patient status].[Patient - status].[Early discontinued] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) " +
                "ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader();


            List<string> label = new List<string>();
            List<string> type = new List<string>();
            List<int> value = new List<int>();

            while (reader.Read())   // read
            {
                label.Add(reader.GetValue(0).ToString());
                type.Add(reader.GetValue(2).ToString());
                value.Add(int.Parse(reader.GetValue(4).ToString()));



                //Console.WriteLine("1 : "+reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }

            return Enumerable.Range(0, label.Count).Select(index => new Models.Patients.PatientRecruitmentPerCountry
            {
                label = label[index],
                type = type[index],
                value = value[index]

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("patient_document")] //http://localhost:5000/api/Patients/patient_document
        public IEnumerable<Models.Patients.PatientDocument> GetPatientDocument()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Document total] } ON COLUMNS, NON EMPTY { ([Document].[Document - group type].[Document - group type].ALLMEMBERS * " +
                "[Document].[Document - conform].[Document - conform].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Document].[Document - owner]." +
                "&[Patient] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Document].[Document - owner]." +
                "&[Patient] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader();


            List<string> label = new List<string>();
            List<string> type = new List<string>();
            List<int> value = new List<int>();

            while (reader.Read())   // read
            {
                label.Add(reader.GetValue(0).ToString());
                type.Add(reader.GetValue(2).ToString());
                value.Add(int.Parse(reader.GetValue(4).ToString()));



                //Console.WriteLine("1 : "+reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }

            return Enumerable.Range(0, label.Count).Select(index => new Models.Patients.PatientDocument
            {
                label = label[index],
                type = type[index],
                value = value[index]

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("patient_document_details")] //http://localhost:5000/api/Patients/patient_document_details
        public IEnumerable<Models.Patients.PatientDocumentDetails> GetPatientDocumentDetails()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Document total] } ON COLUMNS, NON EMPTY { ([Site].[Site - code and name].[Site - code and name].ALLMEMBERS * " +
                "[Patient].[Patient - number].[Patient - number].ALLMEMBERS * [Document].[Document - group type].[Document - group type].ALLMEMBERS * [Document]." +
                "[Document - type].[Document - type].ALLMEMBERS * [Document].[Document - conform].[Document - conform].ALLMEMBERS * [Document date].[Document - sending date]." +
                "[Document - sending date].ALLMEMBERS * [Document date].[Document - CRO receipt date].[Document - CRO receipt date].ALLMEMBERS * [Document date]." +
                "[Document - signature date].[Document - signature date].ALLMEMBERS * [Document date].[Document - shipment to sponsor date].[Document - shipment to sponsor date]." +
                "ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Document].[Document - owner].&[Patient] } ) ON COLUMNS " +
                "FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], " +
                "[Document].[Document - owner].&[Patient] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader();

            List<string> siteCodeName = new List<string>();
            List<string> patientNumber = new List<string>();
            List<string> groupType = new List<string>();
            List<string> type = new List<string>();
            List<string> conform = new List<string>();
            List<string> sendingDate = new List<string>();
            List<string> receiptDate = new List<string>();
            List<string> signatureDate = new List<string>();
            List<string> shipmentToSponsorDate = new List<string>();

            while (reader.Read())   // read
            {
                siteCodeName.Add(reader.GetValue(0).ToString());
                patientNumber.Add(reader.GetValue(2).ToString());
                groupType.Add(reader.GetValue(4).ToString());
                type.Add(reader.GetValue(6).ToString());
                conform.Add(reader.GetValue(8).ToString());
                sendingDate.Add(reader.GetValue(10).ToString());
                receiptDate.Add(reader.GetValue(12).ToString());
                signatureDate.Add(reader.GetValue(14).ToString());
                shipmentToSponsorDate.Add(reader.GetValue(16).ToString());



                //Console.WriteLine("1 : "+reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }


            //                public string siteCodeName { get; set; }
            //public string patientNumber { get; set; }
            //public string groupType { get; set; }
            //public string type { get; set; }
            //public string conform { get; set; }
            //public string sendingDate { get; set; }
            //public string receiptDate { get; set; }
            //public string signatureDate { get; set; }
            //public string shipmentToSponsorDate { get; set; }
            return Enumerable.Range(0, siteCodeName.Count).Select(index => new Models.Patients.PatientDocumentDetails
            {
                siteCodeName = siteCodeName[index],
                patientNumber = patientNumber[index],
                groupType = groupType[index],
                type = type[index],
                conform = conform[index],
                sendingDate = sendingDate[index],
                receiptDate = receiptDate[index],
                signatureDate = signatureDate[index],
                shipmentToSponsorDate = shipmentToSponsorDate[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("patient_ae_pertype")] //http://localhost:5000/api/Patients/patient_ae_pertype
        public IEnumerable<Models.Patients.PatientAEPerType> GetPatientAEPerType()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[AE total] } ON COLUMNS, NON EMPTY { ([AE].[AE - type].[AE - type].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, " +
                "MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA]) WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, " +
                "FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader();

            List<string> aeType = new List<string>();
            List<int> value = new List<int>();

            while (reader.Read())   // read
            {
                aeType.Add(reader.GetValue(0).ToString());
                value.Add(int.Parse(reader.GetValue(2).ToString()));



                //Console.WriteLine("1 : "+reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }

            //                public string siteCodeName { get; set; }
            //public string patientNumber { get; set; }
            return Enumerable.Range(0, aeType.Count).Select(index => new Models.Patients.PatientAEPerType
            {
                aeType = aeType[index],
                value = value[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("patient_ae_details")] //http://localhost:5000/api/Patients/patient_ae_details
        public IEnumerable<Models.Patients.PatientAEDetails> GetPatientAEDetails()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[AE total] } ON COLUMNS, NON EMPTY { ([Site].[Site - code and name].[Site - code and name].ALLMEMBERS * " +
                "[Patient].[Patient - number].[Patient - number].ALLMEMBERS * [AE].[AE - type].[AE - type].ALLMEMBERS * [AE].[AE - seriousness type].[AE - seriousness type]." +
                "ALLMEMBERS * [AE].[AE - description].[AE - description].ALLMEMBERS * [AE date].[AE - detection date].[AE - detection date].ALLMEMBERS * [AE date]." +
                "[AE - declaration to CRO date].[AE - declaration to CRO date].ALLMEMBERS * [AE date].[AE - declaration to sponsor date].[AE - declaration to sponsor date]." +
                "ALLMEMBERS * [AE].[AE - closed].[AE - closed].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( " +
                "SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA]) WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, " +
                "FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader();


            List<string> siteCodeName = new List<string>();
            List<string> patientNumber = new List<string>();
            List<string> type = new List<string>();
            List<string> seriousness = new List<string>();
            List<string> description = new List<string>();
            List<string> detectionDate = new List<string>();
            List<string> croDate = new List<string>();
            List<string> sponsorDate = new List<string>();
            List<string> closed = new List<string>();

            while (reader.Read())   // read
            {
                siteCodeName.Add(reader.GetValue(0).ToString());
                patientNumber.Add(reader.GetValue(2).ToString());
                type.Add(reader.GetValue(4).ToString());
                seriousness.Add(reader.GetValue(6).ToString());
                description.Add(reader.GetValue(8).ToString());
                if (reader.GetValue(10).ToString() == "" || reader.GetValue(10).ToString() == " " || reader.GetValue(10).ToString() == null)
                {
                    detectionDate.Add(" ");
                }
                else
                {
                    detectionDate.Add(DateTime.Parse(reader.GetValue(10).ToString()).ToString("yyyy-MM-dd"));
                }
                if (reader.GetValue(12).ToString() == "" || reader.GetValue(12).ToString() == " " || reader.GetValue(12).ToString() == null)
                {
                    croDate.Add(" ");
                }
                else
                {
                    croDate.Add(DateTime.Parse(reader.GetValue(12).ToString()).ToString("yyyy-MM-dd"));
                }
                if (reader.GetValue(14).ToString() == "" || reader.GetValue(14).ToString() == " " || reader.GetValue(14).ToString() == null)
                {
                    sponsorDate.Add(" ");
                }
                else
                {
                    sponsorDate.Add(DateTime.Parse(reader.GetValue(14).ToString()).ToString("yyyy-MM-dd"));
                }
                closed.Add(reader.GetValue(16).ToString());



                //Console.WriteLine("1 : "+reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("6 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }
            //public string siteCodeName { get; set; }
            //public string patientNumber { get; set; }
            //public string type { get; set; }
            //public string seriousness { get; set; }
            //public string description { get; set; }
            //public string detectionDate { get; set; }
            //public string croDate { get; set; }
            //public string sponsorDate { get; set; }
            //public string closed { get; set; }
            return Enumerable.Range(0, siteCodeName.Count).Select(index => new Models.Patients.PatientAEDetails
            {
                siteCodeName = siteCodeName[index],
                patientNumber = patientNumber[index],
                type = type[index],
                seriousness = seriousness[index],
                description = description[index],
                detectionDate = detectionDate[index],
                croDate = croDate[index],
                sponsorDate = sponsorDate[index],
                closed = closed[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

    }
}
