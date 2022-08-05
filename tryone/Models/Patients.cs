using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tryone.Models
{
    public class Patients
    {
        public class PatientsStatusSummary
        {
            public string status { get; set; }
            public int status_total { get; set; }
            public int last_status_total { get; set; }
        }

        public class PatientInclusionPeriod
        {
            public string date { get; set; }
            public int included { get; set; }
            public int randomised { get; set; }
            public int theoretical { get; set; }
        }

        public class PatientRecruitmentDetails
        {
            public string number { get; set; }
            public string lastStatus { get; set; }
            public string statusReason { get; set; }
            public string date { get; set; }
            public string referralParticipant { get; set; }
        }

        public class PatientRecruitmentPerSite
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class PatientRecruitmentPerCountry
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class PatientDocument
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class PatientDocumentDetails
        {
            public string siteCodeName { get; set; }
            public string patientNumber { get; set; }
            public string groupType { get; set; }
            public string type { get; set; }
            public string conform { get; set; }
            public string sendingDate { get; set; }
            public string receiptDate { get; set; }
            public string signatureDate { get; set; }
            public string shipmentToSponsorDate { get; set; }
        }

        public class PatientAEPerType
        {
            public string aeType { get; set; }
            public int value { get; set; }
        }

        public class PatientAEDetails
        {
            public string siteCodeName { get; set; }
            public string patientNumber { get; set; }
            public string type { get; set; }
            public string seriousness { get; set; }
            public string description { get; set; }
            public string detectionDate { get; set; }
            public string croDate { get; set; }
            public string sponsorDate { get; set; }
            public string closed { get; set; }
        }
    }
}
