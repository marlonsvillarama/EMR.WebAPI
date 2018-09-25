using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.ansi5010
{
    static class SegmentBuilder
    {
        static SegmentBuilder()
        {
            RetrieveSegmentConfigs();
            //RetrieveSegmentPropertyConfigs();
        }

        public static Dictionary<string, int> Segments { get; set; }

        public static Dictionary<string, SegmentPropertyConfig> SegmentConfigs { get; set; }

        private static void RetrieveSegmentConfigs()
        {
            Segments = new Dictionary<string, int>();
            Segments.Add("AMT", 3);                 // Beginning of Hierarchical Transaction
            Segments.Add("BHT", 6);                 // Beginning of Hierarchical Transaction
            Segments.Add("CAS", 19);                // Beginning of Hierarchical Transaction
            Segments.Add("CLM", 20);                // Beginning of Hierarchical Transaction
            Segments.Add("CN1", 6);                // Beginning of Hierarchical Transaction
            Segments.Add("CR1", 10);                // Beginning of Hierarchical Transaction
            Segments.Add("CR2", 12);                // Beginning of Hierarchical Transaction
            Segments.Add("CR3", 5);                // Beginning of Hierarchical Transaction
            Segments.Add("CRC", 7);                 // Beginning of Hierarchical Transaction
            Segments.Add("CTP", 11);                 // Beginning of Hierarchical Transaction
            Segments.Add("CUR", 21);                // Beginning of Hierarchical Transaction
            Segments.Add("DMG", 11);                // Beginning of Hierarchical Transaction
            Segments.Add("DTP", 3);                 // Beginning of Hierarchical Transaction
            Segments.Add("FRM", 5);                 // Beginning of Hierarchical Transaction
            Segments.Add("GE", 2);                 // Beginning of Hierarchical Transaction
            Segments.Add("GS", 8);                 // Beginning of Hierarchical Transaction
            Segments.Add("HCP", 15);
            Segments.Add("HI", 12);
            Segments.Add("HL", 4);
            Segments.Add("IEA", 2);
            Segments.Add("ISA", 16);
            Segments.Add("K3", 3);
            Segments.Add("LIN", 34);
            Segments.Add("LQ", 2);
            Segments.Add("LX", 1);
            Segments.Add("MEA", 12);
            Segments.Add("MOA", 9);
            Segments.Add("N3", 2);
            Segments.Add("N4", 7);
            Segments.Add("NM1", 12);
            Segments.Add("NTE", 2);
            Segments.Add("OI", 6);
            Segments.Add("PAT", 9);                 // Transaction Set Header
            Segments.Add("PER", 9);
            Segments.Add("PRV", 6);
            Segments.Add("PS1", 3);
            Segments.Add("PWK", 9);
            Segments.Add("QTY", 4);
            Segments.Add("REF", 4);
            Segments.Add("SBR", 9);                 // Transaction Set Header
            Segments.Add("SE", 2);                  // Transaction Set Header
            Segments.Add("ST", 3);                  // Transaction Set Header
            Segments.Add("SV1", 21);                 // Transaction Set Header
            Segments.Add("SV5", 7);                 // Transaction Set Header
            Segments.Add("SVD", 6);                 // Transaction Set Header

            /*
            Segments.Add("1000ANM1", 12);           // Submitter Name
            Segments.Add("1000APER", 9);            // Submitter EDI Contact Information
            Segments.Add("1000BNM1", 12);           // Receiver Name

            Segments.Add("2000AHL", 4);             // Billing Provider Hierarchical Level
            Segments.Add("2000APRV", 6);            // Billing Provider Specialty Information
            Segments.Add("2000ACUR", 21);           // Foreign Currency Information

            Segments.Add("2010AANM1", 12);          // Billing Provider Name
            Segments.Add("2010AAN3", 2);            // Billing Provider Address
            Segments.Add("2010AAN4", 7);            // Billing Provider City, State, Zip
            Segments.Add("2010AAREF", 4);           // Billing Provider Tax Identification | UPIN/License Information
            Segments.Add("2010AAPER", 9);           // Billing Provider Contact Information

            Segments.Add("2010ABNM1", 12);          // Pay-To Address Name
            Segments.Add("2010ABN3", 2);            // Pay-To Address Address
            Segments.Add("2010ABN4", 7);            // Pay-To Address City, State, Zip

            Segments.Add("2010ACNM1", 12);          // Pay-To Plan Name
            Segments.Add("2010ACN3", 2);            // Pay-To Plan Address
            Segments.Add("2010ACN4", 7);            // Pay-To Plan City, State, Zip
            Segments.Add("2010ACREF", 4);           // Pay-To Plan Secondary Information | Tax ID Number

            Segments.Add("2000BHL", 4);             // Subscriber Hierarchical Level
            Segments.Add("2000BSBR", 9);            // Subscriber Information
            Segments.Add("2000BPAT", 9);            // Patient Information
            Segments.Add("2000BNM1", 12);           // Subscriber Name
            Segments.Add("2000BN3", 2);             // Subscriber Address
            Segments.Add("2000BN4", 7);             // Subscriber City, State, Zip

            Segments.Add("2010BADMG", 11);          // Subscriber Demographic Information
            Segments.Add("2010BAREF", 4);           // Subscriber Secondary Information | Property and Casualty Claim Number
            Segments.Add("2010BAPER", 9);           // Property and Casualty Subscriber Contact Information

            Segments.Add("2010BBNM1", 12);          // Payer Name
            Segments.Add("2010BBN3", 2);            // Payer Address
            Segments.Add("2010BBN4", 7);            // Payer City, State, Zip
            Segments.Add("2010BBREF", 4);           // Payer Secondary Information | Billing Provider Secondary Information

            Segments.Add("2000CHL", 4);             // Patient Hierarchical Level
            Segments.Add("2000CPAT", 9);            // Patient Information
            Segments.Add("2010CANM1", 12);          // Patient Name
            Segments.Add("2010CAN3", 2);            // Patient Address
            Segments.Add("2010CAN4", 7);            // Patient City, State, Zip
            Segments.Add("2010CADMG", 11);          // Patient Demographic Information
            Segments.Add("2010CAREF", 4);           // Property and Casualty Claim Number | Patient Identifier
            Segments.Add("2010CAPER", 9);           // Property and Casualty Patient Contact Information

            Segments.Add("2300CLM", 20);            // Claim Information
            Segments.Add("2300DTP", 3);             // Date - Onset of Current Illness or Symptom
                                                    // Initial Treatment Date
                                                    // Last Seen Date
                                                    // Date - Acute Manifesetation
                                                    // Date - Accident
                                                    // Date - Last Menstrual Period
                                                    // Date - Last X-Ray Date
                                                    // Date - Hearing and Vision Prescription Date
                                                    // Date - Disability Dates
                                                    // Date - Last Worked
                                                    // Date - Authorized Return to Work
                                                    // Date - Admission
                                                    // Date - Discharge
                                                    // Date - Assumed and Relinquished Care Dates
                                                    // Date - Property and Casualty Date of First Contact
                                                    // Date - Repricer Recevied Date
            Segments.Add("2300PWK", 9);             // Claim Supplemental Information
            Segments.Add("2300CN1", 6);             // Contact Information
            Segments.Add("2300AMT", 3);             // Patient Amount Paid
            Segments.Add("2300REF", 4);             // Service Authorization Exception Code
                                                    // Mandatory Medicare Crossover Indicator
                                                    // Referral Number
                                                    // Prior Authorization
                                                    // Payer Claim Control Number
                                                    // Clinical Laboratory Improvement Amendment Number
                                                    // Repriced Claim Number
                                                    // Adjusted Repriced Claim Number
                                                    // Investigational Device Exemption Number
                                                    // Claim Identifier for Transmission Intermediaries
                                                    // Medical Record Number
                                                    // Demonstration Project Identifier
                                                    // Care Plan Oversight
            Segments.Add("2300K3", 3);              // File Information
            Segments.Add("2300NTE", 2);             // Claim Note
            Segments.Add("2300CR1", 10);            // Ambulance Transport Information
            Segments.Add("2300CR2", 12);            // Spinal Manipulation Service Information
            Segments.Add("2300CRC", 7);             // Ambulance Certification
                                                    // Patient Condition Information : Vision
                                                    // Homebound Indicator
                                                    // EPSDT Referral
            Segments.Add("2300HI", 12);             // Health Care Diagnosis Code
                                                    // Anesthesia Related Procedure
                                                    // Condition Information
            Segments.Add("2300HCP", 15);            // Claim Pricing/Repricing Information

            Segments.Add("2310ANM1", 12);           // Referring Provider Name
            Segments.Add("2310AREF", 4);            // Referring Provider Secondary Information

            Segments.Add("2310BNM1", 12);           // Rendering Provider Name
            Segments.Add("2310BREF", 4);            // Rendering Provider Secondary Information

            Segments.Add("2310CNM1", 12);           // Service Facility Location Name
            Segments.Add("2310CN3", 2);             // Service Facility Location Address
            Segments.Add("2310CN4", 7);             // Service Facility Location City, State, Zip
            Segments.Add("2310CREF", 4);            // Service Facility Location Secondary Identification
            Segments.Add("2310CPER", 9);            // Service Facility Contact Information

            Segments.Add("2310DNM1", 12);           // Supervising Provider Name
            Segments.Add("2310DREF", 4);            // Supervising Provider Secondary Information

            Segments.Add("2310ENM1", 12);           // Ambulance Pickup Location
            Segments.Add("2310EN3", 2);             // Ambulance Pickup Location Address
            Segments.Add("2310EN4", 7);             // Ambulance Pickup Location City, State, Zip

            Segments.Add("2310FNM1", 12);           // Ambulance Drop-Off Location
            Segments.Add("2310FN3", 2);             // Ambulance Drop-Off Location Address
            Segments.Add("2310FN4", 7);             // Ambulance Drop-Off Location City, State, Zip

            Segments.Add("2320SBR", 9);             // Other Subscriber Information
            Segments.Add("2320CAS", 19);            // Claim Level Adjustments
            Segments.Add("2320AMT", 3);             // Coordination of Benefits (COB) Payer Paid Amount
                                                    // Coordination of Benefits (COB) Total Non-Covered Amount
                                                    // Remaining Patient Liabilitys
            Segments.Add("2320OI", 6);              // Other Insurance Coverage Information
            Segments.Add("2320MOA", 9);             // Outpatient Adjudication Information

            Segments.Add("2330ANM1", 12);           // Other Subscriber Name
            Segments.Add("2330AN3", 2);             // Other Subscriber Address
            Segments.Add("2330AN4", 7);             // Other Subscriber City, State, Zip

            Segments.Add("2330BNM1", 12);           // Other Payer Name
            Segments.Add("2330BN3", 2);             // Other Payer Address
            Segments.Add("2330BN4", 7);             // Other Payer City, State, Zip
            */
        }
    }
}