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
    public class MonitoringController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MonitoringController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string dwhSource = @"Data Source=ICTAPOWERBI;Initial Catalog=ICTA_DWH;Integrated Security=True";
        string cubeSource = @"Provider=SQLNCLI11.1;Data Source=report.icta.fr;Initial Catalog=ICTA_CUBE;Integrated Security=SSPI";



        [HttpGet("monitoring_selection_phase")] //http://localhost:5000/api/Monitoring/monitoring_selection_phase
        public IEnumerable<Models.Monitoring.SelectionPhase> GetMonitoringSelectionPhase()
        {

            string cubeQuery = @"SELECT 
                                  [nbIdentifiedSite]/*1*/
                                  ,[nbFirstRequest]/*2*/
                                  ,[nbSiteInterested]
                                  ,[nbConfAgreement]/*4*/
                                  ,[nbCV]
                                  ,[nbSiteSelected]
                                  ,[nbSiteQualified]
                                  ,[nbSiteNotInterested]
                                  ,[nbPlannedMonit]/*9*/
                                  ,[nbPerformedMonit]/*10*/
                                  ,[nbCancelledMonit]
                                  ,[nbCancelledSiteMonit]
                                  ,[nbCRToBeDraft]/*13*/
                                  ,[nbCRToBeValidated]
                                  ,[nbCRValidated]
                              FROM[ICTA_DWH].[dbo].[vw_monitoring_selection] where NomEtude = 'TANGO'";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();

            int identifiedInvestigators = 0;
            int investigatorsContactedFirstRequest = 0;
            int interestedInvestigators = 0;
            int confidentialityAgreementsReceived = 0;
            int cvReceived = 0;

            int selectedInvestigators = 0;
            int qualifiedInvestigators = 0;
            int investigatorsNOT = 0;

            int plannedPreStudyVisits = 0;
            int performedPreStudyVisits = 0;

            int cancelledPreStudyVisits = 0;
            int closeOutVisitsCancelled = 0;

            int reportToBeDrafted = 0;
            int reportToBeValidated = 0;
            int validatedReport = 0;

            while (reader.Read())   // read 0 2 4 10
            {

                identifiedInvestigators += int.Parse(reader.GetValue(0).ToString());
                investigatorsContactedFirstRequest += int.Parse(reader.GetValue(1).ToString());
                interestedInvestigators += int.Parse(reader.GetValue(2).ToString());
                confidentialityAgreementsReceived += int.Parse(reader.GetValue(3).ToString());
                cvReceived += int.Parse(reader.GetValue(4).ToString());

                selectedInvestigators += int.Parse(reader.GetValue(5).ToString());
                qualifiedInvestigators += int.Parse(reader.GetValue(6).ToString());
                investigatorsNOT += int.Parse(reader.GetValue(7).ToString());

                plannedPreStudyVisits += int.Parse(reader.GetValue(8).ToString());
                performedPreStudyVisits += int.Parse(reader.GetValue(9).ToString());

                cancelledPreStudyVisits += int.Parse(reader.GetValue(10).ToString());
                closeOutVisitsCancelled += int.Parse(reader.GetValue(11).ToString());

                reportToBeDrafted += int.Parse(reader.GetValue(12).ToString());
                reportToBeValidated += int.Parse(reader.GetValue(13).ToString());
                validatedReport += int.Parse(reader.GetValue(14).ToString());



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


            return Enumerable.Range(0, 1).Select(index => new Models.Monitoring.SelectionPhase
            {
                identifiedInvestigators = identifiedInvestigators,
                investigatorsContactedFirstRequest = investigatorsContactedFirstRequest,
                interestedInvestigators = interestedInvestigators,
                confidentialityAgreementsReceived = confidentialityAgreementsReceived,
                cvReceived = cvReceived,
                selectedInvestigators = selectedInvestigators,
                qualifiedInvestigators = qualifiedInvestigators,
                investigatorsNOT = investigatorsNOT,
                plannedPreStudyVisits = plannedPreStudyVisits,
                performedPreStudyVisits = performedPreStudyVisits,
                cancelledPreStudyVisits = cancelledPreStudyVisits,
                closeOutVisitsCancelled = closeOutVisitsCancelled,
                reportToBeDrafted = reportToBeDrafted,
                reportToBeValidated = reportToBeValidated,
                validatedReport = validatedReport,

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_selection_phase_table")] //http://localhost:5000/api/Monitoring/monitoring_selection_phase_table
        public IEnumerable<Models.Monitoring.SelectionPhaseTable> GetMonitoringSelectionPhaseTable()
        {

            string cubeQuery = @"SELECT [sstNameListSecto]
                                  ,[sstrName]
	                              ,[lastStatus]
	                              ,[statusReasonNotSelected]
	                              ,[dtFirstRequest]
	                              ,[interestedYN]
	                              ,[dtSendingConfAgreement]
                                  ,[dtReceivedConfAgreement]
                                  ,[dtReceivedCV]
	                              ,[dtVisit]
	                              ,[statusReasonCancelled]
	                              ,[sstrLastMonitoringStatus]
                                  ,[PERSO_FullName]
                                  ,[dtRedactionCR]
                              FROM [ICTA_DWH].[dbo].[vw_monitoring_selection_details] where NomEtude = 'TANGO' order by sstrName ";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            List<string> divisionDomain = new List<string>();
            List<string> nameOfThePI = new List<string>();
            List<string> siteStatus = new List<string>();
            List<string> reasonNotSelected = new List<string>();
            List<string> firstRequest = new List<string>();
            List<string> interestedYN = new List<string>();
            List<string> sendingConfAgreement = new List<string>();
            List<string> receivedConfAgreement = new List<string>();
            List<string> receivedCV = new List<string>();
            List<string> visit = new List<string>();
            List<string> reasonCancelled = new List<string>();
            List<string> lastMonitoringStatus = new List<string>();
            List<string> fullName = new List<string>();
            List<string> redactionCR = new List<string>();

            while (reader.Read())   // read 0 2 4 10
            {
                //if (reader.IsDBNull(0))
                //{
                //    Console.WriteLine("bbb0");
                //}
                //else
                //{
                //    Console.WriteLine("aaa0 : " + reader.GetValue(0).ToString());
                //}

                //if (reader.IsDBNull(1))
                //{
                //    Console.WriteLine("bbb1");
                //}
                //else
                //{
                //    Console.WriteLine("aaa1 : " + reader.GetValue(1).ToString());
                //}

                //if (reader.IsDBNull(2))
                //{
                //    Console.WriteLine("bbb2");
                //}
                //else
                //{
                //    Console.WriteLine("aaa2 : " + reader.GetValue(2).ToString());
                //}

                //if (reader.IsDBNull(3))
                //{
                //    Console.WriteLine("bbb3");
                //}
                //else
                //{
                //    Console.WriteLine("aaa3 : " + reader.GetValue(3).ToString());
                //}

                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                Console.WriteLine("5 : " + reader.GetValue(5).ToString());
                Console.WriteLine("6 : " + reader.GetValue(6).ToString());
                Console.WriteLine("7 : " + reader.GetValue(7).ToString());
                Console.WriteLine("8 : " + reader.GetValue(8).ToString());
                Console.WriteLine("9 : " + reader.GetValue(9).ToString());
                Console.WriteLine("10 : " + reader.GetValue(10).ToString());
                Console.WriteLine("11 : " + reader.GetValue(11).ToString());
                Console.WriteLine("12 : " + reader.GetValue(12).ToString());
                Console.WriteLine("13 : " + reader.GetValue(13).ToString());
                if (reader.IsDBNull(0))
                {
                    divisionDomain.Add(" ");
                }
                else
                {
                    divisionDomain.Add(reader.GetValue(0).ToString());
                }

                if (reader.IsDBNull(1))
                {
                    nameOfThePI.Add(" ");
                }
                else
                {
                    nameOfThePI.Add(reader.GetValue(1).ToString());
                }

                if (reader.IsDBNull(2))
                {
                    siteStatus.Add(" ");
                }
                else
                {
                    siteStatus.Add(reader.GetValue(2).ToString());
                }

                if (reader.IsDBNull(3))
                {
                    reasonNotSelected.Add(" ");
                }
                else
                {
                    reasonNotSelected.Add(reader.GetValue(3).ToString());
                }

                if (reader.IsDBNull(4))
                {
                    firstRequest.Add(" ");
                }
                else
                {
                    firstRequest.Add(DateTime.Parse(reader.GetValue(4).ToString()).ToString("yyyy-MM-dd"));
                }

                if (reader.IsDBNull(5))
                {
                    interestedYN.Add(" ");
                }
                else
                {
                    interestedYN.Add(reader.GetValue(5).ToString());
                }

                if (reader.IsDBNull(6))
                {
                    sendingConfAgreement.Add(" ");
                }
                else
                {
                    sendingConfAgreement.Add(DateTime.Parse(reader.GetValue(6).ToString()).ToString("yyyy-MM-dd"));

                }

                if (reader.IsDBNull(7))
                {
                    receivedConfAgreement.Add(" ");
                }
                else
                {
                    receivedConfAgreement.Add(DateTime.Parse(reader.GetValue(7).ToString()).ToString("yyyy-MM-dd"));

                }

                if (reader.IsDBNull(8))
                {
                    receivedCV.Add(" ");
                }
                else
                {
                    receivedCV.Add(DateTime.Parse(reader.GetValue(8).ToString()).ToString("yyyy-MM-dd"));

                }

                if (reader.IsDBNull(9))
                {
                    visit.Add(" ");
                }
                else
                {
                    visit.Add(DateTime.Parse(reader.GetValue(9).ToString()).ToString("yyyy-MM-dd"));

                }

                if (reader.IsDBNull(10))
                {
                    reasonCancelled.Add(" ");
                }
                else
                {
                    reasonCancelled.Add(reader.GetValue(10).ToString());
                }

                if (reader.IsDBNull(11))
                {
                    lastMonitoringStatus.Add(" ");
                }
                else
                {
                    lastMonitoringStatus.Add(reader.GetValue(11).ToString());
                }

                if (reader.IsDBNull(12))
                {
                    fullName.Add(" ");
                }
                else
                {
                    fullName.Add(reader.GetValue(12).ToString());
                }

                if (reader.IsDBNull(13))
                {
                    redactionCR.Add(" ");
                }
                else
                {
                    redactionCR.Add(DateTime.Parse(reader.GetValue(13).ToString()).ToString("yyyy-MM-dd"));

                }
            }

            Console.WriteLine("compteur : " + divisionDomain.Count);

            return Enumerable.Range(0, divisionDomain.Count).Select(index => new Models.Monitoring.SelectionPhaseTable
            {
                divisionDomain = divisionDomain[index],
                nameOfThePI = nameOfThePI[index],
                siteStatus = siteStatus[index],
                reasonNotSelected = reasonNotSelected[index],
                firstRequest = firstRequest[index],
                interestedYN = interestedYN[index],
                sendingConfAgreement = sendingConfAgreement[index],
                receivedConfAgreement = receivedConfAgreement[index],
                receivedCV = receivedCV[index],
                visit = visit[index],
                reasonCancelled = reasonCancelled[index],
                lastMonitoringStatus = lastMonitoringStatus[index],
                fullName = fullName[index],
                redactionCR = redactionCR[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_initiation_phase")] //http://localhost:5000/api/Monitoring/monitoring_initiation_phase
        public IEnumerable<Models.Monitoring.InitiationPhase> GetMonitoringInitiationPhase()
        {

            string cubeQuery = @"SELECT [nbQualifiedSite]
		                                ,[nbPlannedMonit]
		                                ,[nbPerformedMonit]
		                                ,[nbCancelledMonit]
		                                ,[nbCancelledSiteMonit]
		                                ,[nbCRToBeDraft]
		                                ,[nbCRToBeValidated]
                                        ,[nbCRValidated]
                                  FROM [ICTA_DWH].[dbo].[vw_monitoring_initiation] where NomEtude = 'TANGO'";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            int qualifiedSite = 0;
            int plannedSite = 0;
            int performedSite = 0;
            int cancelledMonit = 0;
            int cancelledSiteMonit = 0;
            int crToBeDraft = 0;
            int crToBeValidated = 0;
            int crValidated = 0;


            while (reader.Read())   // read 0 2 4 10
            {

                qualifiedSite += int.Parse(reader.GetValue(0).ToString());
                plannedSite += int.Parse(reader.GetValue(1).ToString());
                performedSite += int.Parse(reader.GetValue(2).ToString());
                cancelledMonit += int.Parse(reader.GetValue(3).ToString());
                cancelledSiteMonit += int.Parse(reader.GetValue(4).ToString());
                crToBeDraft += int.Parse(reader.GetValue(5).ToString());
                crToBeValidated += int.Parse(reader.GetValue(6).ToString());
                crValidated += int.Parse(reader.GetValue(7).ToString());



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

            return Enumerable.Range(0, 1).Select(index => new Models.Monitoring.InitiationPhase
            {
                qualifiedSite = qualifiedSite,
                plannedSite = plannedSite,
                performedSite = performedSite,
                cancelledMonit = cancelledMonit,
                cancelledSiteMonit = cancelledSiteMonit,
                crToBeDraft = crToBeDraft,
                crToBeValidated = crToBeValidated,
                crValidated = crValidated,
            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_initiation_phase_table")] //http://localhost:5000/api/Monitoring/monitoring_initiation_phase_table
        public IEnumerable<Models.Monitoring.InitiationPhaseTable> GetMonitoringInitiationPhaseTable()
        {

            string cubeQuery = @"  SELECT 
		                            [sstNameListSecto]
		                            ,[sstrCentreCodeName]
		                            ,[CRA]
		                            ,[lastStatus]
		                            ,[statusReason]
		                            ,[dtVisit]
		                            ,[sstrMonitNature]
		                            ,[sstrLastMonitoringStatus]
		                            ,[PERSO_FullName]
		                            ,[dtRedactionCR]
                              FROM [ICTA_DWH].[dbo].[vw_monitoring_initiation_details] where [NomEtude] = 'TANGO' order by sstrCentreCodeName ";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            List<string> nameListSecto = new List<string>();
            List<string> centreCodeName = new List<string>();
            List<string> CRA = new List<string>();
            List<string> lastStatus = new List<string>();
            List<string> statusReason = new List<string>();
            List<string> visit = new List<string>();
            List<string> monitNature = new List<string>();
            List<string> lastMonitStatus = new List<string>();
            List<string> fullName = new List<string>();
            List<string> readactionCR = new List<string>();

            while (reader.Read())   // read 0 2 4 10
            {

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
                //Console.WriteLine("11 : " + reader.GetValue(11).ToString());
                //Console.WriteLine("12 : " + reader.GetValue(12).ToString());
                //Console.WriteLine("13 : " + reader.GetValue(13).ToString());
                if (reader.IsDBNull(0))
                {
                    nameListSecto.Add(" ");
                }
                else
                {
                    nameListSecto.Add(reader.GetValue(0).ToString());
                }

                if (reader.IsDBNull(1))
                {
                    centreCodeName.Add(" ");
                }
                else
                {
                    centreCodeName.Add(reader.GetValue(1).ToString());
                }

                if (reader.IsDBNull(2))
                {
                    CRA.Add(" ");
                }
                else
                {
                    CRA.Add(reader.GetValue(2).ToString());
                }

                if (reader.IsDBNull(3))
                {
                    lastStatus.Add(" ");
                }
                else
                {
                    lastStatus.Add(reader.GetValue(3).ToString());
                }

                if (reader.IsDBNull(4))
                {
                    statusReason.Add(" ");
                }
                else
                {
                    statusReason.Add(reader.GetValue(4).ToString());
                }

                if (reader.IsDBNull(5))
                {
                    visit.Add(" ");
                }
                else
                {
                    visit.Add(DateTime.Parse(reader.GetValue(5).ToString()).ToString("yyyy-MM-dd"));
                }

                if (reader.IsDBNull(6))
                {
                    monitNature.Add(" ");
                }
                else
                {
                    monitNature.Add(reader.GetValue(6).ToString());
                }

                if (reader.IsDBNull(7))
                {
                    lastMonitStatus.Add(" ");
                }
                else
                {
                    lastMonitStatus.Add(reader.GetValue(7).ToString());

                }

                if (reader.IsDBNull(8))
                {
                    fullName.Add(" ");
                }
                else
                {
                    fullName.Add(reader.GetValue(8).ToString());

                }

                if (reader.IsDBNull(9))
                {
                    readactionCR.Add(" ");
                }
                else
                {
                    readactionCR.Add(DateTime.Parse(reader.GetValue(9).ToString()).ToString("yyyy-MM-dd"));

                }


            }

            //                public string nameListSecto { get; set; }
            //public string centreCodeName { get; set; }
            //public string CRA { get; set; }
            //public string lastStatus { get; set; }
            //public string visit { get; set; }
            //public string monitNature { get; set; }
            //public string lastMonitStatus { get; set; }
            //public string fullName { get; set; }
            //public string readactionCR { get; set; }
            return Enumerable.Range(0, nameListSecto.Count).Select(index => new Models.Monitoring.InitiationPhaseTable
            {
                nameListSecto = nameListSecto[index],
                centreCodeName = centreCodeName[index],
                CRA = CRA[index],
                lastStatus = lastStatus[index],
                statusReason = statusReason[index],
                visit = visit[index],
                monitNature = monitNature[index],
                lastMonitStatus = lastMonitStatus[index],
                fullName = fullName[index],
                readactionCR = readactionCR[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_followup_phase")] //http://localhost:5000/api/Monitoring/monitoring_followup_phase
        public IEnumerable<Models.Monitoring.FollowupPhase> GetMonitoringFollowupPhase()
        {

            string cubeQuery = @"SELECT 
                                [nbInitiatedSite]
                                ,[nbPlannedMonit]
                                ,[nbPerformedMonit]
                                ,[nbCancelledMonit]
                                ,[nbCancelledSiteMonit]
                                ,[nbIncludedPatient]
                                ,[nbCRToBeDraft]
                                ,[nbCRToBeValidated]
                                ,[nbCRValidated]
                                  FROM [ICTA_DWH].[dbo].[vw_monitoring_follow-up] where NomEtude = 'TANGO'";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            int initiatedSite = 0;
            int plannedMonit = 0;
            int performedMonit = 0;
            int cancelledMonit = 0;
            int cancelledSiteMonit = 0;
            int includedPatient = 0;
            int crToBeDraft = 0;
            int crToBeValidated = 0;
            int crValidated = 0;


            while (reader.Read())   // read 0 2 4 10
            {

                initiatedSite += int.Parse(reader.GetValue(0).ToString());
                plannedMonit += int.Parse(reader.GetValue(1).ToString());
                performedMonit += int.Parse(reader.GetValue(2).ToString());
                cancelledMonit += int.Parse(reader.GetValue(3).ToString());
                cancelledSiteMonit += int.Parse(reader.GetValue(4).ToString());
                includedPatient += int.Parse(reader.GetValue(5).ToString());
                crToBeDraft += int.Parse(reader.GetValue(6).ToString());
                crToBeValidated += int.Parse(reader.GetValue(7).ToString());
                crValidated += int.Parse(reader.GetValue(8).ToString());



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
            //                public int initiatedSite { get; set; }
            //public int plannedMonit { get; set; }
            //public int performedMonit { get; set; }
            //public int cancelledMonit { get; set; }
            //public int cancelledSiteMonit { get; set; }
            //public int includedPatient { get; set; }
            //public int crToBeDraft { get; set; }
            //public int crToBeValidated { get; set; }
            //public int crValidated { get; set; }
            return Enumerable.Range(0, 1).Select(index => new Models.Monitoring.FollowupPhase
            {
                initiatedSite = initiatedSite,
                plannedMonit = plannedMonit,
                performedMonit = performedMonit,
                cancelledMonit = cancelledMonit,
                cancelledSiteMonit = cancelledSiteMonit,
                includedPatient = includedPatient,
                crToBeDraft = crToBeDraft,
                crToBeValidated = crToBeValidated,
                crValidated = crValidated,
            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_followup_phase_table")] //http://localhost:5000/api/Monitoring/monitoring_followup_phase_table
        public IEnumerable<Models.Monitoring.FollowupPhaseTable> GetMonitoringFollowupPhaseTable()
        {

            string cubeQuery = @" SELECT 
                                  [sstNameListSecto]
	                              ,[sstrCentreCodeName]
	                              ,[CRA]
	                              ,[lastStatus]
	                              ,[statusReason]
	                              ,[dtVisit]
	                              ,[sstrMonitNature]
	                              ,[sstrLastMonitoringStatus]
	                              ,[PERSO_FullName]
	                              ,[dtRedactionCR]
                              FROM [ICTA_DWH].[dbo].[vw_monitoring_follow-up_details] where NomEtude = 'TANGO' order by sstrCentreCodeName";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            List<string> nameListSecto = new List<string>();
            List<string> centreCodeName = new List<string>();
            List<string> CRA = new List<string>();
            List<string> lastStatus = new List<string>();
            List<string> statusReason = new List<string>();
            List<string> visit = new List<string>();
            List<string> monitNature = new List<string>();
            List<string> lastMonitStatus = new List<string>();
            List<string> fullName = new List<string>();
            List<string> redactionCR = new List<string>();

            while (reader.Read())   // read 0 2 4 10
            {

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
                //Console.WriteLine("11 : " + reader.GetValue(11).ToString());
                //Console.WriteLine("12 : " + reader.GetValue(12).ToString());
                //Console.WriteLine("13 : " + reader.GetValue(13).ToString());
                if (reader.IsDBNull(0))
                {
                    nameListSecto.Add(" ");
                }
                else
                {
                    nameListSecto.Add(reader.GetValue(0).ToString());
                }

                if (reader.IsDBNull(1))
                {
                    centreCodeName.Add(" ");
                }
                else
                {
                    centreCodeName.Add(reader.GetValue(1).ToString());
                }

                if (reader.IsDBNull(2))
                {
                    CRA.Add(" ");
                }
                else
                {
                    CRA.Add(reader.GetValue(2).ToString());
                }

                if (reader.IsDBNull(3))
                {
                    lastStatus.Add(" ");
                }
                else
                {
                    lastStatus.Add(reader.GetValue(3).ToString());
                }

                if (reader.IsDBNull(4))
                {
                    statusReason.Add(" ");
                }
                else
                {
                    statusReason.Add(reader.GetValue(4).ToString());
                }

                if (reader.IsDBNull(5))
                {
                    visit.Add(" ");
                }
                else
                {
                    visit.Add(DateTime.Parse(reader.GetValue(5).ToString()).ToString("yyyy-MM-dd"));
                }

                if (reader.IsDBNull(6))
                {
                    monitNature.Add(" ");
                }
                else
                {
                    monitNature.Add(reader.GetValue(6).ToString());
                }

                if (reader.IsDBNull(7))
                {
                    lastMonitStatus.Add(" ");
                }
                else
                {
                    lastMonitStatus.Add(reader.GetValue(7).ToString());

                }

                if (reader.IsDBNull(8))
                {
                    fullName.Add(" ");
                }
                else
                {
                    fullName.Add(reader.GetValue(8).ToString());

                }

                if (reader.IsDBNull(9))
                {
                    redactionCR.Add(" ");
                }
                else
                {
                    redactionCR.Add(DateTime.Parse(reader.GetValue(9).ToString()).ToString("yyyy-MM-dd"));

                }


            }

            //                public string nameListSecto { get; set; }
            //public string centreCodeName { get; set; }
            //public string CRA { get; set; }
            //public string lastStatus { get; set; }
            //public string visit { get; set; }
            //public string monitNature { get; set; }
            //public string lastMonitStatus { get; set; }
            //public string fullName { get; set; }
            //public string readactionCR { get; set; }
            return Enumerable.Range(0, nameListSecto.Count).Select(index => new Models.Monitoring.FollowupPhaseTable
            {
                nameListSecto = nameListSecto[index],
                centreCodeName = centreCodeName[index],
                CRA = CRA[index],
                lastStatus = lastStatus[index],
                statusReason = statusReason[index],
                visit = visit[index],
                monitNature = monitNature[index],
                lastMonitStatus = lastMonitStatus[index],
                fullName = fullName[index],
                redactionCR = redactionCR[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_closeout_phase")] //http://localhost:5000/api/Monitoring/monitoring_closeout_phase
        public IEnumerable<Models.Monitoring.CloseOutPhase> GetMonitoringCloseOutPhase()
        {

            string cubeQuery = @"SELECT 
                                [nbInitiatedSite]
                                ,[nbPlannedMonit]
                                ,[nbPerformedMonit]
                                ,[nbCancelledMonit]
                                ,[nbCancelledSiteMonit]
                                ,[nbIncludedPatient]
                                ,[nbCRToBeDraft]
                                ,[nbCRToBeValidated]
                                ,[nbCRValidated]
                                  FROM [ICTA_DWH].[dbo].[vw_monitoring_close-out] where NomEtude = 'TANGO'";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            int initiatedSite = 0;
            int plannedMonit = 0;
            int performedMonit = 0;
            int cancelledMonit = 0;
            int cancelledSiteMonit = 0;
            int includedPatient = 0;
            int crToBeDraft = 0;
            int crToBeValidated = 0;
            int crValidated = 0;


            while (reader.Read())   // read 0 2 4 10
            {

                initiatedSite += int.Parse(reader.GetValue(0).ToString());
                plannedMonit += int.Parse(reader.GetValue(1).ToString());
                performedMonit += int.Parse(reader.GetValue(2).ToString());
                cancelledMonit += int.Parse(reader.GetValue(3).ToString());
                cancelledSiteMonit += int.Parse(reader.GetValue(4).ToString());
                includedPatient += int.Parse(reader.GetValue(5).ToString());
                crToBeDraft += int.Parse(reader.GetValue(6).ToString());
                crToBeValidated += int.Parse(reader.GetValue(7).ToString());
                crValidated += int.Parse(reader.GetValue(8).ToString());



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
            //                public int initiatedSite { get; set; }
            //public int plannedMonit { get; set; }
            //public int performedMonit { get; set; }
            //public int cancelledMonit { get; set; }
            //public int cancelledSiteMonit { get; set; }
            //public int includedPatient { get; set; }
            //public int crToBeDraft { get; set; }
            //public int crToBeValidated { get; set; }
            //public int crValidated { get; set; }
            return Enumerable.Range(0, 1).Select(index => new Models.Monitoring.CloseOutPhase
            {
                initiatedSite = initiatedSite,
                plannedMonit = plannedMonit,
                performedMonit = performedMonit,
                cancelledMonit = cancelledMonit,
                cancelledSiteMonit = cancelledSiteMonit,
                includedPatient = includedPatient,
                crToBeDraft = crToBeDraft,
                crToBeValidated = crToBeValidated,
                crValidated = crValidated,
            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_closeout_phase_table")] //http://localhost:5000/api/Monitoring/monitoring_closeout_phase_table
        public IEnumerable<Models.Monitoring.CloseOutPhaseTable> GetMonitoringCloseOutPhaseTable()
        {

            string cubeQuery = @"SELECT [sstNameListSecto]
	                              ,[sstrCentreCodeName]
	                              ,[CRA]
	                              ,[lastStatus]
	                              ,[statusReason]
	                              ,[dtVisit]
	                              ,[sstrMonitNature]
	                              ,[sstrLastMonitoringStatus]
	                              ,[PERSO_FullName]
	                              ,[dtRedactionCR]
                              FROM [ICTA_DWH].[dbo].[vw_monitoring_close-out_details] where NomEtude = 'FIRE' order by sstrCentreCodeName";

            SqlConnection con = new SqlConnection(dwhSource);
            con.Open();

            //SqlDataAdapter adapt = new SqlDataAdapter(cubeQuery, con);
            //DataTable datatable = new DataTable();
            //adapt.Fill(datatable);

            var cmd = new SqlCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            List<string> nameListSecto = new List<string>();
            List<string> centreCodeName = new List<string>();
            List<string> CRA = new List<string>();
            List<string> lastStatus = new List<string>();
            List<string> statusReason = new List<string>();
            List<string> visit = new List<string>();
            List<string> monitNature = new List<string>();
            List<string> lastMonitStatus = new List<string>();
            List<string> fullName = new List<string>();
            List<string> redactionCR = new List<string>();

            while (reader.Read())   // read 0 2 4 10
            {

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
                //Console.WriteLine("11 : " + reader.GetValue(11).ToString());
                //Console.WriteLine("12 : " + reader.GetValue(12).ToString());
                //Console.WriteLine("13 : " + reader.GetValue(13).ToString());
                if (reader.IsDBNull(0))
                {
                    nameListSecto.Add(" ");
                }
                else
                {
                    nameListSecto.Add(reader.GetValue(0).ToString());
                }

                if (reader.IsDBNull(1))
                {
                    centreCodeName.Add(" ");
                }
                else
                {
                    centreCodeName.Add(reader.GetValue(1).ToString());
                }

                if (reader.IsDBNull(2))
                {
                    CRA.Add(" ");
                }
                else
                {
                    CRA.Add(reader.GetValue(2).ToString());
                }

                if (reader.IsDBNull(3))
                {
                    lastStatus.Add(" ");
                }
                else
                {
                    lastStatus.Add(reader.GetValue(3).ToString());
                }

                if (reader.IsDBNull(4))
                {
                    statusReason.Add(" ");
                }
                else
                {
                    statusReason.Add(reader.GetValue(4).ToString());
                }

                if (reader.IsDBNull(5))
                {
                    visit.Add(" ");
                }
                else
                {
                    visit.Add(DateTime.Parse(reader.GetValue(5).ToString()).ToString("yyyy-MM-dd"));
                }

                if (reader.IsDBNull(6))
                {
                    monitNature.Add(" ");
                }
                else
                {
                    monitNature.Add(reader.GetValue(6).ToString());
                }

                if (reader.IsDBNull(7))
                {
                    lastMonitStatus.Add(" ");
                }
                else
                {
                    lastMonitStatus.Add(reader.GetValue(7).ToString());

                }

                if (reader.IsDBNull(8))
                {
                    fullName.Add(" ");
                }
                else
                {
                    fullName.Add(reader.GetValue(8).ToString());

                }

                if (reader.IsDBNull(9))
                {
                    redactionCR.Add(" ");
                }
                else
                {
                    redactionCR.Add(DateTime.Parse(reader.GetValue(9).ToString()).ToString("yyyy-MM-dd"));

                }


            }

            //                public string nameListSecto { get; set; }
            //public string centreCodeName { get; set; }
            //public string CRA { get; set; }
            //public string lastStatus { get; set; }
            //public string visit { get; set; }
            //public string monitNature { get; set; }
            //public string lastMonitStatus { get; set; }
            //public string fullName { get; set; }
            //public string readactionCR { get; set; }
            return Enumerable.Range(0, nameListSecto.Count).Select(index => new Models.Monitoring.CloseOutPhaseTable
            {
                nameListSecto = nameListSecto[index],
                centreCodeName = centreCodeName[index],
                CRA = CRA[index],
                lastStatus = lastStatus[index],
                statusReason = statusReason[index],
                visit = visit[index],
                monitNature = monitNature[index],
                lastMonitStatus = lastMonitStatus[index],
                fullName = fullName[index],
                redactionCR = redactionCR[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_details")] //http://localhost:5000/api/Monitoring/monitoring_details
        public IEnumerable<Models.Monitoring.Details> GetMonitoringDetails()
        {

            string cubeQuery = @" SELECT NON EMPTY { [Measures].[Monitoring last status] } ON COLUMNS, NON EMPTY { ([Monitoring].[Monitoring - nature].[Monitoring - nature].ALLMEMBERS * [Monitoring status].
[Monitoring - status].[Monitoring - status].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS 
FROM [Cube ICTA]) WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            var cmd = new AdomdCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            List<string> label = new List<string>();
            List<string> type = new List<string>();
            List<int> value = new List<int>();

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

            }

            return Enumerable.Range(0, label.Count).Select(index => new Models.Monitoring.Details
            {
                label = label[index],
                type = type[index],
                value = value[index]

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring_details_table")] //http://localhost:5000/api/Monitoring/monitoring_details_table
        public IEnumerable<Models.Monitoring.DetailsTable> GetMonitoringDetailsTable()
        {

            string cubeQuery = @"  SELECT NON EMPTY { [Measures].[Monitoring status total] } ON COLUMNS, NON EMPTY { ([Site].[Site - code].[Site - code].ALLMEMBERS * [Site].[Site - name].[Site - name].ALLMEMBERS * 
[Monitoring].[Monitoring - mode].[Monitoring - mode].ALLMEMBERS * [Monitoring].[Monitoring - nature].[Monitoring - nature].ALLMEMBERS * [Monitoring date].[Monitoring - event date].[Monitoring - event date].ALLMEMBERS * 
[Monitoring date].[Monitoring - event time].[Monitoring - event time].ALLMEMBERS * [Monitoring team member].[Monitoring team - name].[Monitoring team - name].ALLMEMBERS * [Monitoring status].[Monitoring - status].
[Monitoring - status].ALLMEMBERS * [Monitoring date].[Monitoring - status date].[Monitoring - status date].ALLMEMBERS * [Monitoring].[Monitoring - way].[Monitoring - way].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, 
MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA]) WHERE ( [Study].[Study name].&[TANGO] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, 
FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            var cmd = new AdomdCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            List<string> code = new List<string>();
            List<string> name = new List<string>();
            List<string> mode = new List<string>();
            List<string> nature = new List<string>();
            List<string> eventDate = new List<string>();
            List<string> eventTime = new List<string>();
            List<string> teamName = new List<string>();
            List<string> status = new List<string>();
            List<string> statusDate = new List<string>();
            List<string> way = new List<string>();

            while (reader.Read())   // read 0 2 4 10
            {
                code.Add(reader.GetValue(0).ToString());
                name.Add(reader.GetValue(2).ToString());
                mode.Add(reader.GetValue(4).ToString());
                nature.Add(reader.GetValue(6).ToString());
                eventDate.Add(reader.GetValue(8).ToString());
                eventTime.Add(reader.GetValue(10).ToString());
                teamName.Add(reader.GetValue(12).ToString());
                status.Add(reader.GetValue(14).ToString());
                statusDate.Add(reader.GetValue(16).ToString());
                way.Add(reader.GetValue(18).ToString());

                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());

            }

            return Enumerable.Range(0, code.Count).Select(index => new Models.Monitoring.DetailsTable
            {
                code = code[index],
                name = name[index],
                mode = mode[index],
                nature = nature[index],
                eventDate = eventDate[index],
                eventTime = eventTime[index],
                teamName = teamName[index],
                status = status[index],
                statusDate = statusDate[index],
                way = way[index],


            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("participants_documents")] //http://localhost:5000/api/Monitoring/participants_documents
        public IEnumerable<Models.Monitoring.ParticipantsDocument> GetMonitoringParticipantsDocument()
        {

            string cubeQuery = @" SELECT NON EMPTY { [Measures].[Document total] } ON COLUMNS, NON EMPTY { ([Document].[Document - group type].[Document - group type].ALLMEMBERS * 
    [Document].[Document - conform].[Document - conform].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Document].[Document - owner].&
    [Participant] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Document].[Document - owner].&
    [Participant] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            var cmd = new AdomdCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();



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
            }


            return Enumerable.Range(0, label.Count).Select(index => new Models.Monitoring.ParticipantsDocument
            {
                label = label[index],
                type = type[index],
                value = value[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("participants_documents_details")] //http://localhost:5000/api/Monitoring/participants_documents_details
        public IEnumerable<Models.Monitoring.ParticipantDocumentDetails> GetMonitoringParticipantsDocumentDetails()
        {

            string cubeQuery = @" SELECT NON EMPTY { [Measures].[Document total] } ON COLUMNS, NON EMPTY { ([Site].[Site - code and name].[Site - code and name].ALLMEMBERS * [Organization].
[Organization - name].[Organization - name].ALLMEMBERS * [Document].[Document - type].[Document - type].ALLMEMBERS * [Document].[Document - conform].[Document - conform].ALLMEMBERS * 
[Document - sending].[Filter].[Date].ALLMEMBERS * [Document date].[Document - CRO receipt date].[Document - CRO receipt date].ALLMEMBERS * [Document - signature].[Filter].[Date].ALLMEMBERS * 
[Document - shipment to sponsor].[Filter].[Date].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Document].[Document - owner].&[Participant] } ) 
ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Document].[Document - owner].&[Participant] ) CELL PROPERTIES 
VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            var cmd = new AdomdCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();



            List<string> siteCodeName = new List<string>();
            List<string> type = new List<string>();
            List<string> conform = new List<string>();
            List<string> sendingDate = new List<string>();
            List<string> croReceiptDate = new List<string>();
            List<string> signatureDate = new List<string>();
            List<string> shipmentToSponsorDate = new List<string>();

            //DateTime b = test.AddDays(7);

            while (reader.Read())   // read 0 2 4 10
            {
                if (reader.IsDBNull(0)) { siteCodeName.Add(" "); }
                else { siteCodeName.Add(reader.GetValue(0).ToString()); }

                if (reader.IsDBNull(4)) { type.Add(" "); }
                else { type.Add(reader.GetValue(4).ToString()); }

                if (reader.IsDBNull(6)) { conform.Add(" "); }
                else { conform.Add(reader.GetValue(6).ToString()); }


                if (reader.IsDBNull(12)) { sendingDate.Add(" "); }
                else { sendingDate.Add(reader.GetValue(12).ToString()); }

                if (reader.IsDBNull(14)) { croReceiptDate.Add(" "); }
                else { croReceiptDate.Add(reader.GetValue(14).ToString()); }

                if (reader.IsDBNull(20)) { signatureDate.Add(" "); }
                else { signatureDate.Add(reader.GetValue(20).ToString()); }

                if (reader.IsDBNull(26)) { shipmentToSponsorDate.Add(" "); }
                else { shipmentToSponsorDate.Add(reader.GetValue(26).ToString()); }


                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(6).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(7).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(8).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(9).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(10).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(11).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(12).ToString());
            }

            //                public string siteCodeName { get; set; }
            //public string orgName { get; set; }
            //public string type { get; set; }
            //public string conform { get; set; }
            //public string sendingDate { get; set; }
            //public string croReceiptDate { get; set; }
            //public string signatureDate { get; set; }
            //public string shipmentToSponsorDate { get; set; }
            return Enumerable.Range(0, siteCodeName.Count).Select(index => new Models.Monitoring.ParticipantDocumentDetails
            {
                siteCodeName = siteCodeName[index],
                type = type[index],
                conform = conform[index],
                sendingDate = sendingDate[index],
                croReceiptDate = croReceiptDate[index],
                signatureDate = signatureDate[index],
                shipmentToSponsorDate = shipmentToSponsorDate[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("organization_documents")] //http://localhost:5000/api/Monitoring/organization_documents
        public IEnumerable<Models.Monitoring.OrganizationDocument> GetMonitoringOrganizationDocument()
        {

            string cubeQuery = @"  SELECT NON EMPTY { [Measures].[Document total] } ON COLUMNS, NON EMPTY { ([Document].[Document - group type].[Document - group type].ALLMEMBERS * [Document].[Document - conform].
[Document - conform].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Document].[Document - owner].&[Organization] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].
&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Document].[Document - owner].&[Organization] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, 
FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            var cmd = new AdomdCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();



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
            }


            return Enumerable.Range(0, label.Count).Select(index => new Models.Monitoring.OrganizationDocument
            {
                label = label[index],
                type = type[index],
                value = value[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("organization_documents_details")] //http://localhost:5000/api/Monitoring/organization_documents_details
        public IEnumerable<Models.Monitoring.OrganizationDocumentDetails> GetMonitoringOrganizationDocumentDetails()
        {

            string cubeQuery = @"  SELECT NON EMPTY { [Measures].[Document total] } ON COLUMNS, NON EMPTY { ([Site].[Site - code and name].[Site - code and name].ALLMEMBERS * 
[Organization].[Organization - name].[Organization - name].ALLMEMBERS * [Document].[Document - type].[Document - type].ALLMEMBERS * [Document].[Document - conform].[Document - conform].ALLMEMBERS * 
[Document - sending].[Filter].[Date].ALLMEMBERS * [Document date].[Document - CRO receipt date].[Document - CRO receipt date].ALLMEMBERS * [Document - signature].[Filter].[Date].ALLMEMBERS * 
[Document - shipment to sponsor].[Filter].[Date].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Document].[Document - owner].&[Organization] } ) 
ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[TANGO] } ) ON COLUMNS FROM [Cube ICTA])) WHERE ( [Study].[Study name].&[TANGO], [Document].[Document - owner].&[Organization] ) CELL PROPERTIES 
VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            var con = new AdomdConnection(cubeSource);
            con.Open();

            var cmd = new AdomdCommand(cubeQuery, con);
            cmd.CommandTimeout = 0;
            var reader = cmd.ExecuteReader();


            
            List<string> siteCodeName = new List<string>();
            List<string> orgName = new List<string>();
            List<string> type = new List<string>();
            List<string> conform = new List<string>();
            List<string> sendingDate = new List<string>();
            List<string> croReceiptDate = new List<string>();
            List<string> signatureDate = new List<string>();
            List<string> shipmentToSponsorDate = new List<string>();

            //DateTime b = test.AddDays(7);

            while (reader.Read())   // read 0 2 4 10
            {
                if (reader.IsDBNull(0)) { siteCodeName.Add(" "); }
                else { siteCodeName.Add(reader.GetValue(0).ToString()); }

                if (reader.IsDBNull(2)) { orgName.Add(" "); }
                else { orgName.Add(reader.GetValue(2).ToString()); }

                if (reader.IsDBNull(4)) { type.Add(" "); }
                else { type.Add(reader.GetValue(4).ToString()); }

                if (reader.IsDBNull(6)) { conform.Add(" "); }
                else { conform.Add(reader.GetValue(6).ToString()); }


                if (reader.IsDBNull(12)) { sendingDate.Add(" "); }
                else { sendingDate.Add(reader.GetValue(12).ToString()); }

                if (reader.IsDBNull(14)) { croReceiptDate.Add(" "); }
                else { croReceiptDate.Add(reader.GetValue(14).ToString()); }

                if (reader.IsDBNull(20)) { signatureDate.Add(" "); }
                else { signatureDate.Add(reader.GetValue(20).ToString()); }

                if (reader.IsDBNull(26)) { shipmentToSponsorDate.Add(" "); }
                else { shipmentToSponsorDate.Add(reader.GetValue(26).ToString()); }


                //Console.WriteLine("0 : " + reader.GetValue(0).ToString());
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString());
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString());
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(5).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(6).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(7).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(8).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(9).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(10).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(11).ToString());
                //Console.WriteLine("4 : " + reader.GetValue(12).ToString());
            }

            //                public string siteCodeName { get; set; }
            //public string orgName { get; set; }
            //public string type { get; set; }
            //public string conform { get; set; }
            //public string sendingDate { get; set; }
            //public string croReceiptDate { get; set; }
            //public string signatureDate { get; set; }
            //public string shipmentToSponsorDate { get; set; }
            return Enumerable.Range(0, siteCodeName.Count).Select(index => new Models.Monitoring.OrganizationDocumentDetails
            {
                siteCodeName = siteCodeName[index],
                orgName = orgName[index],
                type = type[index],
                conform = conform[index],
                sendingDate = sendingDate[index],
                croReceiptDate = croReceiptDate[index],
                signatureDate = signatureDate[index],
                shipmentToSponsorDate = shipmentToSponsorDate[index],

            }).ToArray();

            //return new JsonResult(datatable);
        }
    }
}
