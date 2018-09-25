using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.ansi5010
{
    static class X837Lists
    {


        public static Dictionary<string, string> PatientRelationship
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "01", "Spouse" },
                    { "18", "Self" },
                    { "19", "Child" },
                    { "20", "Employee" },
                    { "21", "Unknown" },
                    { "39", "Organ Donor" },
                    { "40", "Cadaver Donor" },
                    { "53", "Life Partner" },
                    { "G8", "Other Relationship" }
                };
            }
        }

        public static Dictionary<string, string> TransactionSetPurpose
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "00", "Original" },
                    { "18", "Reissue" }
                };
            }
        }

        public static Dictionary<string, string> AttachmentType
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "03", "Report Justifying Treatment Beyond Utilization Guidelines" },
                    { "04", "Drugs Administered" },
                    { "05", "Treatment Diagnosis" },
                    { "06", "Initial Assessment" },
                    { "07", "Functional Goals" },
                    { "08", "Plan of Treatment" },
                    { "09", "Progress Report" },
                    { "10", "Continued Treatment" },
                    { "11", "Chemical Analysis" },
                    { "13", "Certified Test Report" },
                    { "15", "Jusitification for Admission" },
                    { "21", "Recovery Plan" },
                    { "A3", "Allergies/Sensitivity Document" },
                    { "A4", "Autopsy Report" },
                    { "AM", "Ambulance Certification" },
                    { "AS", "Admission Summary" },
                    { "B2", "Prescription" },
                    { "B3", "Physician Order" },
                    { "B4", "Referral Form" },
                    { "BR", "Benchmark Testing Results" },
                    { "BS", "Baseline" },
                    { "BT", "Blanket Test Results" },
                    { "CB", "Chiropractic Justification" },
                    { "CK", "Consent Form(s)" },
                    { "CT", "Certification" },
                    { "D2", "Drug Profile Document" },
                    { "DA", "Dental Models" },
                    { "DB", "Durable Medical Equipment Prescription" },
                    { "DG", "Diagnosis Report" },
                    { "DJ", "Discharge Monitoring Report" },
                    { "DS", "Discharge Summary" },
                    { "EB", "Explanation of Benefits" },
                    { "HC", "Health Certificate" },
                    { "HR", "Health Clinic Records" },
                    { "IR", "State School Immunization Records" },
                    { "LA", "Laboratory Results" },
                    { "M1", "Medical Record Attachment" },
                    { "MT", "Models" },
                    { "NN", "Nursing Notes" },
                    { "OB", "Operative Note" },
                    { "OC", "Oxygen Content Averaging Report" },
                    { "OD", "Orders and Treatments Document" },
                    { "OE", "Objective Physical Examination" },
                    { "OX", "Oxygen Therapy Certification" },
                    { "OZ", "Support Data for Claim" },
                    { "P4", "Pathology Report" },
                    { "P5", "Patient Medical History Document" },
                    { "PE", "Parenteral or Enteral Certification" },
                    { "PN", "Physical Therapy Notes" },
                    { "PO", "Prosthetics or Orthotics Certification" },
                    { "PQ", "Paramedical Results" },
                    { "PY", "Phycisian's Report" },
                    { "PZ", "Phycisal Therapy Certification" },
                    { "RB", "Radiology Films" },
                    { "RR", "Radiology Reports" },
                    { "RT", "Report of Tests and Analysis Report" },
                    { "SG", "Symptoms Document" },
                    { "V5", "Death Notification" },
                    { "XP", "Photographs" }
                };
            }
        }

        public static Dictionary<string, string> AttachmentTransmission
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "AA", "Available on Request at Provider Site" },
                    { "BM", "By Mail" },
                    { "EL", "Electronically Only" },
                    { "EM", "E-Mail" },
                    { "FT", "File Transfer" },
                    { "FX", "By Fax" }
                };
            }
        }
         
        public static Dictionary<string, string> DurableTransmission
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "AB", "Previousy Submitted to Payer" },
                    { "AD", "Certification Included in this Claim" },
                    { "AF", "Narrative Segment Included in this Claim" },
                    { "AG", "No Documentation is Required" },
                    { "NS", "Not Speified" }
                };
            }
        }

        public static Dictionary<string, string> TransactionTypes
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "31", "Subrogation Demand" },
                    { "CH", "Chargeable" },
                    { "RP", "Reporting" },
                };
            }
        }

        public static Dictionary<string, string> InsuranceTypeCode
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "12", "Medicare Secondary Working Aged Beneficiary or Spouse with Employer Group Health Plan" },
                    { "13", "Medicare Secondary End-Stage Renal Disease Beneficiary in the Mandated Coordination Period with an Employer's Group Health Plan" },
                    { "14", "Medicare Secondary, No-fault Insurance including Auto is Primary" },
                    { "15", "Medicare Secondary Worker's Compensation" },
                    { "16", "Medicare Secondary Public Health Service (PHS) or Other Federal Agency" },
                    { "41", "Medicare Secondary Black Lung" },
                    { "42", "Medicare Secondary Veteran's Administration" },
                    { "43", "Medicare Secondary Disabled Beneficiary Under Age 65 with Large Group Health Plan (LGHP)" },
                    { "47", "Medicare Secondary, Other Liability Insurance is Primary" },
                };
            }
        }

        public static Dictionary<string, string> ClaimFilingIndicatorCode
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "11", "Other Non-Federal Programs" },
                    { "12", "Preferred Provider Organization (PPO)" },
                    { "13", "Point of Service (POS)" },
                    { "14", "Exclusive Provider Organization (EPO)" },
                    { "15", "Indemnity Insurance" },
                    { "16", "Health Maintenance Organization (HMO) Medicare Risk" },
                    { "17", "Dental Maintenance Organization" },
                    { "AM", "Automobile Medical" },
                    { "BL", "Blue Cross/Blue Shield" },
                    { "CH", "Champus" },
                    { "CI", "Commercial Insurance Co." },
                    { "DS", "Disability" },
                    { "FI", "Federal Employees Program" },
                    { "HM", "Health Maintenance Organization" },
                    { "LM", "Liability Medical" },
                    { "MA", "Medicare Part A" },
                    { "MB", "Medicare Part B" },
                    { "MC", "Medicaid" },
                    { "OF", "Other Federal Program" },
                    { "TV", "Title V" },
                    { "VA", "Veterans Affairs Plan" },
                    { "WC", "Worker's Compensation Health Claim" },
                    { "ZZ", "Mutually Defined" },
                };
            }
        }

    }
}