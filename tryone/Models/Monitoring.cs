using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tryone.Models
{
    public class Monitoring
    {
        public class SelectionPhase
        {
            public int identifiedInvestigators { get; set; }
            public int investigatorsContactedFirstRequest { get; set; }
            public int interestedInvestigators { get; set; }
            public int confidentialityAgreementsReceived { get; set; }
            public int cvReceived { get; set; }
            public int selectedInvestigators { get; set; }
            public int qualifiedInvestigators { get; set; }
            public int investigatorsNOT { get; set; }
            public int plannedPreStudyVisits { get; set; }
            public int performedPreStudyVisits { get; set; }
            public int cancelledPreStudyVisits { get; set; }
            public int closeOutVisitsCancelled { get; set; }
            public int reportToBeDrafted { get; set; }
            public int reportToBeValidated { get; set; }
            public int validatedReport { get; set; }
        }

        public class SelectionPhaseTable
        {
            public string divisionDomain { get; set; }
            public string nameOfThePI { get; set; }
            public string siteStatus { get; set; }
            public string reasonNotSelected { get; set; }
            public string firstRequest { get; set; }
            public string interestedYN { get; set; }
            public string sendingConfAgreement { get; set; }
            public string receivedConfAgreement { get; set; }
            public string receivedCV { get; set; }
            public string visit { get; set; }
            public string reasonCancelled { get; set; }
            public string lastMonitoringStatus { get; set; }
            public string fullName { get; set; }
            public string redactionCR { get; set; }
        }

        public class InitiationPhase
        {
            public int qualifiedSite { get; set; }
            public int plannedSite { get; set; }
            public int performedSite { get; set; }
            public int cancelledMonit { get; set; }
            public int cancelledSiteMonit { get; set; }
            public int crToBeDraft { get; set; }
            public int crToBeValidated { get; set; }
            public int crValidated { get; set; }
        }

        public class InitiationPhaseTable
        {
            public string nameListSecto { get; set; }
            public string centreCodeName { get; set; }
            public string CRA { get; set; }
            public string lastStatus { get; set; }
            public string statusReason { get; set; }
            public string visit { get; set; }
            public string monitNature { get; set; }
            public string lastMonitStatus { get; set; }
            public string fullName { get; set; }
            public string readactionCR { get; set; }
        }

        public class FollowupPhase
        {
            public int initiatedSite { get; set; }
            public int plannedMonit { get; set; }
            public int performedMonit { get; set; }
            public int cancelledMonit { get; set; }
            public int cancelledSiteMonit { get; set; }
            public int includedPatient { get; set; }
            public int crToBeDraft { get; set; }
            public int crToBeValidated { get; set; }
            public int crValidated { get; set; }
        }

        public class FollowupPhaseTable
        {
            public string nameListSecto { get; set; }
            public string centreCodeName { get; set; }
            public string CRA { get; set; }
            public string lastStatus { get; set; }
            public string statusReason { get; set; }
            public string visit { get; set; }
            public string monitNature { get; set; }
            public string lastMonitStatus { get; set; }
            public string fullName { get; set; }
            public string redactionCR { get; set; }
        }

        public class CloseOutPhase
        {
            public int initiatedSite { get; set; }
            public int plannedMonit { get; set; }
            public int performedMonit { get; set; }
            public int cancelledMonit { get; set; }
            public int cancelledSiteMonit { get; set; }
            public int includedPatient { get; set; }
            public int crToBeDraft { get; set; }
            public int crToBeValidated { get; set; }
            public int crValidated { get; set; }
        }

        public class CloseOutPhaseTable
        {
            public string nameListSecto { get; set; }
            public string centreCodeName { get; set; }
            public string CRA { get; set; }
            public string lastStatus { get; set; }
            public string statusReason { get; set; }
            public string visit { get; set; }
            public string monitNature { get; set; }
            public string lastMonitStatus { get; set; }
            public string fullName { get; set; }
            public string redactionCR { get; set; }
        }

        public class Details
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class DetailsTable
        {
            public string code { get; set; }
            public string name { get; set; }
            public string mode { get; set; }
            public string nature { get; set; }
            public string eventDate { get; set; }
            public string eventTime { get; set; }
            public string teamName { get; set; }
            public string status { get; set; }
            public string statusDate { get; set; }
            public string way { get; set; }
        }

        public class ParticipantsDocument
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class ParticipantDocumentDetails
        {
            public string siteCodeName { get; set; }
            public string type { get; set; }
            public string conform { get; set; }
            public string sendingDate { get; set; }
            public string croReceiptDate { get; set; }
            public string signatureDate { get; set; }
            public string shipmentToSponsorDate { get; set; }
        }

        public class OrganizationDocument
        {
            public string label { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }

        public class OrganizationDocumentDetails
        {
            public string siteCodeName { get; set; }
            public string orgName { get; set; }
            public string type { get; set; }
            public string conform { get; set; }
            public string sendingDate { get; set; }
            public string croReceiptDate { get; set; }
            public string signatureDate { get; set; }
            public string shipmentToSponsorDate { get; set; }
        }

    }
}
