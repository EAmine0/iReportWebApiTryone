using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tryone.Models
{
    public class Sites
    {
        //------------------------------Status

        public class SitesStatusSummary
        {
            public string status { get; set; }
            public int status_total { get; set; }
            public int last_status_total { get; set; }
        }

        public class SitesStatusDetails
        {
            public string site { get; set; }
            public string last_status { get; set; }
            public string status_reason { get; set; }
            public string status_date { get; set; }
        }

        //------------------------------Activities
        public class SitesActivitiesMonitoring
        {
            public string division_domain { get; set; }
            public string site { get; set; }
            public string site_status { get; set; }
            public string financial_agreement { get; set; }
            public string selected_date { get; set; }
            public string readyToInclude_date { get; set; }
            public string firstMonitoring_date { get; set; }
            public string lastMonitoring_date { get; set; }
            public string closeOut_date { get; set; }
            public int nb_monitoring { get; set; }
        }

        public class SitesActivitiesPatients
        {
            public string divisionDomain { get; set; }
            public string site { get; set; }
            public string siteStatus { get; set; }
            public string faList { get; set; }
            public string fpfv { get; set; }
            public int included { get; set; }
            public int inRunIn { get; set; }
            public int randomised { get; set; }
            public int onStudyTreatment { get; set; }
            public int enStudyTreatment { get; set; }
            public int followUp { get; set; }
            public int endStudy { get; set; }
            public int earlyDiscontinued { get; set; }
            public int screeningFailure { get; set; }
        }

        public class SitesActivitiesSAE
        {
            public string centreCode { get; set; }
            public int nbSAE { get; set; }
        }

        public class SitesGANTT
        {
            public string country { get; set; }
            public string regulatory_start_date_planned { get; set; }
            public string regulatory_end_date_planned { get; set; }
            public string regulatory_start_date_actual { get; set; }
            public string regulatory_end_date_actual { get; set; }

            public string startup_start_date_planned { get; set; }
            public string startup_end_date_planned { get; set; }
            public string startup_start_date_actual { get; set; }
            public string startup_end_date_actual { get; set; }

            public string coredocs_start_date_planned { get; set; }
            public string coredocs_end_date_planned { get; set; }
            public string coredocs_start_date_actual { get; set; }
            public string coredocs_end_date_actual { get; set; }

            public string siteselection_start_date_planned { get; set; }
            public string siteselection_end_date_planned { get; set; }
            public string siteselection_start_date_actual { get; set; }
            public string siteselection_end_date_actual { get; set; }

            public string initiation_start_date_planned { get; set; }
            public string initiation_end_date_planned { get; set; }
            public string initiation_start_date_actual { get; set; }
            public string initiation_end_date_actual { get; set; }

            public string recruitment_start_date_planned { get; set; }
            public string recruitment_end_date_planned { get; set; }
            public string recruitment_start_date_actual { get; set; }
            public string recruitment_end_date_actual { get; set; }

            public string monitoring_start_date_planned { get; set; }
            public string monitoring_end_date_planned { get; set; }
            public string monitoring_start_date_actual { get; set; }
            public string monitoring_end_date_actual { get; set; }
        }

        public class SitesMilestoneTimeline
        {
            public string phase { get; set; }
            public string milestone { get; set; }
            public string pre_date { get; set; }
            public string date { get; set; }
            public string post_date { get; set; }
        }

        public class SitesDocuments
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class SitesDocumentsDetails
        {
            public string site { get; set; }
            public string type { get; set; }
            public string conform { get; set; }
            public string sendingDate { get; set; }
            public string receiptDate { get; set; }
            public string signatureDate { get; set; }
            public string shipmentToSponsorDate { get; set; }
        }

        public class SitesFAType
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class SitesFADetails
        {
            public string site { get; set; }
            public string type { get; set; }
            public string beneficiaryType { get; set; }
            public string feesType { get; set; }
            public string documentType { get; set; }
            public string sendingDate { get; set; }
            public string receiptDate { get; set; }
            public string signatureDate { get; set; }
            public string shipmentToSponsorDate { get; set; }
        }
    }
    
}
