using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tryone.Models
{
    public class OpDashboard
    {
        //------------------------------Clinical Operational

        public class SiteIdentifiedPerCountry
        {
            public string country { get; set; }
            public int site_identified { get; set; }

        }

        public class Sites
        {
            public int total_value { get; set; }
            public int potential_value { get; set; }

        }

        public class Patients
        {
            public int total_value { get; set; }
            public int potential_value { get; set; }

        }

        public class SiteStatus
        {
            public string label { get; set; }
            public int status_total { get; set; }
            public int last_status_total { get; set; }

        }

        public class PatientStatus
        {
            public string label { get; set; }
            public int status_total { get; set; }
            public int last_status_total { get; set; }

        }

        public class CurveOfInclusion
        {
            public string date { get; set; }
            public int included { get; set; }
            public int randomised { get; set; }
            public int theoretical { get; set; }
        }

        public class Monitoring
        {
            public string nature { get; set; }
            public string mode { get; set; }
            public int last_status { get; set; }
            public float avg_monitoring_per_site { get; set; }

        }

        public class Documents
        {
            public string no_yes { get; set; }
            public int value { get; set; }
            public int received { get; set; }
            public int default_unresolved { get; set; }
        }

        public class Safety
        {
            public string initial_followup { get; set; }
            public int value { get; set; }
            public int value2 { get; set; }
            public int ack_not_received { get; set; }
        }

        public class Safety_table
        {
            public string ae_type { get; set; }
            public float per_site { get; set; }
            public float per_patient { get; set; }
        }

        //------------------------------DM-CRF

        public class Visits
        {
            public float entered { get; set; }
            public float cleaned { get; set; }
        }

        public class PatientCleaned
        {
            public int cleaned { get; set; }
        }

        public class DataManagementQueries
        {
            public string label { get; set; }
            public int issued { get; set; }
            public int closed { get; set; }
            public int sent { get; set; }
            public int received { get; set; }
            public int completed { get; set; }
            public int confirmed { get; set; }
            public int resolved { get; set; }

        }

        public class DMCRFQueries
        {
            public string label { get; set; }

            public int value { get; set; }
        }

        public class PatientPerMandatoryConsultation
        {
            public string label { get; set; }
            public int na { get; set; }
            public int incomplete { get; set; }
            public int complete { get; set; }
            public int dea { get; set; }
            public int deb { get; set; }
            public int clean { get; set; }

        }

        //------------------------------DM-eCRF

        public class DMeCRF_Visits
        {
            public float data_entry { get; set; }
            public float signed { get; set; }
        }

        public class DMeCRF_PatientSigned
        {
            public int signed { get; set; }
        }

        public class DMeCRFQueries
        {
            public string label { get; set; }

            public int value { get; set; }
        }
        public class DMeCRF_DataManagementQueries
        {
            public string label { get; set; }
            public int issued { get; set; }
            public int closed { get; set; }
            public int sent { get; set; }
            public int received { get; set; }
            public int completed { get; set; }
            public int confirmed { get; set; }
            public int resolved { get; set; }

        }

        public class DMeCRF_PatientPerMandatoryConsultation
        {
            public string label { get; set; }
            public int expected { get; set; }
            public int in_progress { get; set; }
            public int data_entry { get; set; }
            public int signed { get; set; }

        }


















        public string label { get; set; }
        public int firstvalue { get; set; }
        public int secondvalue { get; set; }
        public int thirdvalue { get; set; }
        public int fourthvalue { get; set; }
        public float value5 { get; set; }
        public float value6 { get; set; }
        public int fifthvalue { get; set; }
        public int sixthvalue { get; set; }
        public int seventhvalue { get; set; }

        public string azer0 { get; set; }
        public int azer1 { get; set; }
        public int azer2 { get; set; }
        public int azer3 { get; set; }
        public int azer4 { get; set; }
        public int azer5 { get; set; }

    }




}
