using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.ansi5010
{
    public class X837
    {
        private Dictionary<string, string> CONFIG;
        //private List<Claim> allClaims;
        private Batch batch;
        private EHRDB db;

        private string refId;   // FOR TESTING ONLY
        private string controlISA = "0000123456";
        private string controlGS = "542136";
        private string controlST = "000123";
        private DateTime controlDT;

        private List<Subscriber> subscribers;
        private List<Patient> patients;
        //private List<Claim> claims;

        private int hl;
        private int hlprov;
        private int hlsub;
        private int segmentCount;

        #region Private Method
        private bool GetBooleanValue(bool? key)
        {
            return key == null ? false : key.Value;
        }

        private DateTime GetDateTimeValue(DateTime? dateTime)
        {
            return dateTime == null ?
                DateTime.Parse("01/01/1000") : dateTime.Value;
        }
        #endregion

        #region Constructor
        public X837(string dbName)
        {
            CONFIG = X837Config.Items;

            try
            {
                db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbName);
                //Claims = claims;
                //Batch = batch;
                //InterchangeControlNumber = batch.Identifier;
                //GroupControlNumber = batch.Id.ToString().PadLeft(0, '8');
                controlST = (1).ToString().PadLeft(4, '0');
                controlDT = DateTime.Now;
                hl = 0;
                segmentCount = 0;
                refId = "512364879356";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Properties
        public List<Claim> Claims { get; set; }

        public List<PayTo> PayToes { get; set; }

        public Batch Batch { get; set; }

        public string Receiver { get; set; }

        public string Database { get; set; }
        
        public string InterchangeControlNumber { get; set; }

        public string GroupControlNumber { get; set; }

        public string ReferenceID
        {
            get => refId;
        }

        public Segment ISA
        {
            get
            {
                InterchangeControlNumber = Batch.Identifier;

                Segment seg = new Segment("ISA");
                seg.Items["ISA01"] = CONFIG["ISA01"];
                seg.Items["ISA02"] = CONFIG["ISA02"];
                seg.Items["ISA03"] = CONFIG["ISA03"];
                seg.Items["ISA04"] = CONFIG["ISA04"];
                seg.Items["ISA05"] = CONFIG["ISA05"];
                seg.Items["ISA06"] = CONFIG["ISA06"];
                seg.Items["ISA07"] = CONFIG["ISA07"];

                if (String.IsNullOrEmpty(Receiver))
                {
                    seg.Items["ISA08"] = CONFIG["ISA08"];
                }
                else if (Receiver == "NYSDOH")
                {
                    seg.Items["ISA08"] = "EMEDNYBAT";
                }

                seg.Items["ISA09"] = X837Writer.WriteDate(controlDT);
                seg.Items["ISA10"] = X837Writer.WriteTime(controlDT);
                seg.Items["ISA11"] = CONFIG["ISA11"];
                seg.Items["ISA12"] = CONFIG["ISA12"];
                seg.Items["ISA13"] = InterchangeControlNumber;
                seg.Items["ISA14"] = CONFIG["ISA14"];
                seg.Items["ISA15"] = CONFIG["ISA15"];
                seg.Items["ISA16"] = CONFIG["ISA16"];
                return seg;
            }
        }

        public Segment IEA
        {
            get
            {
                Segment seg = new Segment("IEA");
                seg["IEA01"] = "1";
                seg["IEA02"] = InterchangeControlNumber;
                return seg;
            }
        }

        public Segment GS
        {
            get
            {
                GroupControlNumber = Batch.Id.ToString().PadLeft(0, '8');

                Segment seg = new Segment("GS");
                seg.Items["GS01"] = CONFIG["GS01"];
                seg.Items["GS02"] = CONFIG["GS02"];

                if (String.IsNullOrEmpty(Receiver))
                {
                    seg.Items["GS03"] = CONFIG["GS03"];
                }
                else if (Receiver == "NYSDOH")
                {
                    seg.Items["GS03"] = "EMEDNYBAT";
                }

                seg.Items["GS04"] = X837Writer.WriteDate(controlDT);
                seg.Items["GS05"] = X837Writer.WriteTime(controlDT);
                seg.Items["GS06"] = GroupControlNumber;
                seg.Items["GS07"] = CONFIG["GS07"];
                seg.Items["GS08"] = CONFIG["GS08"];
                return seg;
            }
        }

        public Segment GE
        {
            get
            {
                Segment seg = new Segment("GE");
                seg["GE01"] = "1"; // Nunmber of Transaction Sets Included
                seg["GE02"] = GroupControlNumber;
                return seg;
            }
        }

        public Segment ST
        {
            get
            {
                Segment seg = new Segment("ST");
                seg.Loop = "1000A";
                seg.Items["ST01"] = "837";
                seg.Items["ST02"] = controlST;
                seg.Items["ST03"] = CONFIG["GS08"];
                return seg;
            }
        }

        public Segment SE
        {
            get
            {
                Segment seg = new Segment("SE");
                seg.Items["SE01"] = segmentCount.ToString();
                seg.Items["ST02"] = controlST;
                return seg;
            }
        }

        public Segment BHT
        {
            get
            {
                Segment seg = new Segment("BHT");
                seg.Loop = "1000A";
                seg.Items["BHT01"] = "0019";
                seg.Items["BHT02"] = CONFIG["BHT02"];
                seg.Items["BHT03"] = refId;
                seg.Items["BHT04"] = X837Writer.WriteDate(controlDT);
                seg.Items["BHT05"] = X837Writer.WriteTime(controlDT);
                seg.Items["BHT06"] = CONFIG["BHT06"];
                return seg;
            }
        }

        public SubmitterSegment Submitter
        {
            get
            {
                SubmitterSegment submitter = new SubmitterSegment(db);

                return submitter;
            }
        }
        #endregion

        #region Write Methods
        // NOT USED
        private void InitX837()
        {
            string ticks = DateTime.Now.Ticks.ToString();
            //db = new EHRDB();
            controlISA = ticks.Substring(0, 10);
            controlGS = ticks.Substring(10);
            controlST = (1).ToString().PadLeft(4, '0');
            controlDT = DateTime.Now;
        }
        
        // NOT USED
        private void InitClaims()
        {
            subscribers = new List<Subscriber>();
            patients = new List<Patient>();
            //claims = new List<Claim>();
        }

        private void AppendToString(StringBuilder sb, string str)
        {
            sb.Append(str);
            segmentCount++;
        }

        public string WriteX837(List<Claim> claims, bool separator = false)
        {
            X837Config.SeparateNewLine = true;
            Segment st = Segment.ST(controlST);
            Segment bht = Segment.BHT(
                CONFIG["BHT02"] == "00",
                refId,
                DateTime.Now,
                CONFIG["BHT06"]
            );

            StringBuilder sb = new StringBuilder()

                .Append(ISA.Output)

                .Append(GS.Output);

            // Transaction Set Header
            AppendToString(sb, ST.Output);

            // Beginning of Hierarchical Transaction
            AppendToString(sb, BHT.Output);

            // Submitter Name
            sb.Append(Submitter.Submitter.Output);
            segmentCount += Submitter.SegmentCount;

            // Submitter Contact
            AppendToString(sb, Submitter.Contact.Output);

            // Receiver Name
            AppendToString(sb, Submitter.Receiver.Output);

            // Write out all claims billed under groups first
            List<Claim> billedUnderGroup = Claims
                                            .Where(x => x.BillingProvider.Id != x.RenderingProvider.Id)
                                            .ToList();
            
             List<Provider> billingGroups = billedUnderGroup.Select(x => x.BillingProvider)
                                            .Distinct().ToList();

            foreach(Provider billingGroup in billingGroups)
            {
                List<Provider> groupRenderingProviders = billedUnderGroup.Select(x => x.RenderingProvider)
                                                            .Distinct().ToList();
                foreach(Provider renderingProvider in groupRenderingProviders)
                {
                    sb.Append(WriteBiller(billedUnderGroup, billingGroup, renderingProvider));
                }
            }

            // Next, write out all claims billed under individual providers
            List<Claim> billedAsIndividual = Claims
                                            .Where(x => x.BillingProvider.Id == x.RenderingProvider.Id)
                                            .ToList();

            List<Provider> billingProviders = billedAsIndividual.Select(x => x.RenderingProvider)
                                                .Distinct().ToList();

            foreach(Provider billingProvider in billingProviders)
            {
                sb.Append(WriteBiller(billedAsIndividual, billingProvider));
            }

            // Loop #1: Billing Provider
            // -- Loop #2: Subscriber
            // ---- Loop #3: Patient (if different from Subscriber)
            // ------ Loop #4: Claim
            // -------- Loop #5: Claim Line



            /*
                .Append(
                    BillingProvider.HL.Output +
                    (String.IsNullOrEmpty(CONFIG["2000A_PRV03"]) == true ?
                        String.Empty :
                        BillingProvider.Output
                    ) +
                    String.Empty +
                    BillingProvider.NM1.Output +
                    BillingProvider.N3.Output +
                    BillingProvider.N4.Output +
                    BillingProvider.REF_Tax.Output +
                    (String.IsNullOrEmpty(BillingProvider.NM1.NM109) == true ?
                        BillingProvider.REF_UPIN.Output :
                        String.Empty
                    ) +
                    (BillingProvider.PER.Output != Submitter.PER.Output ?
                        BillingProvider.PER.Output :
                        String.Empty
                    )
                )

                .Append(String.Empty

                );*/
            // HL 1

            // Transaction Set Trailer
            AppendToString(sb, SE.Output);

            sb.Append(GE.Output)
                .Append(IEA.Output);

            return sb.ToString().ToUpper();
        }

        private string WriteBiller(List<Claim> filteredClaims, Provider billingProvider, Provider renderingProvider = null)
        {
            StringBuilder sb = new StringBuilder();
            bool isCompany = billingProvider.IsCompany;

            hl++;
            hlprov = hl;

            #region Billing Provider Hierarchy Level
            Segment seg = Segment.HL(hl, "20");
            seg["HL04"] = "1";
            AppendToString(sb, seg.Output);
            #endregion

            #region Billing Provider Specialty Information (only if Person)
            if (isCompany == false)
            {
                seg = Segment.PRV("BI", "PXC", billingProvider.TaxonomyCode.Code);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Billing Provider Name, Address
            seg = Segment.NM1("85",
                isCompany == true ? "2" : "1",
                billingProvider.LastName,
                isCompany == true ? "" : billingProvider.FirstName,
                isCompany == true ? "" : billingProvider.MiddleName,
                isCompany == true ? "" : billingProvider.Suffix,
                "XX",
                billingProvider.NPI
            );
            AppendToString(sb, seg.Output);

            seg = Segment.N3(
                billingProvider.Address_1,
                billingProvider.Address_2
            );
            AppendToString(sb, seg.Output);


            seg = Segment.N4(
                billingProvider.City,
                billingProvider.State,
                billingProvider.Zip
            );
            AppendToString(sb, seg.Output);
            #endregion

            #region Billing Provider Secondary Identification
            if (isCompany == false)
            {
                string taxId, taxType = "EI";
                taxId = String.IsNullOrEmpty(billingProvider.EIN) == true ?
                    billingProvider.SSN : billingProvider.EIN;
                taxType = String.IsNullOrEmpty(billingProvider.EIN) == true ?
                    "SY" : "EI";

                if (String.IsNullOrEmpty(billingProvider.EIN) == false ||
                    String.IsNullOrEmpty(billingProvider.SSN) == false)
                {
                    seg = Segment.REF(taxType, taxId);
                    AppendToString(sb, seg.Output);
                }

                if (String.IsNullOrEmpty(billingProvider.UPIN) == false)
                {
                    seg = Segment.REF("1G", billingProvider.UPIN);
                    AppendToString(sb, seg.Output);
                }

                if (String.IsNullOrEmpty(billingProvider.License) == false)
                {
                    seg = Segment.REF("0B", billingProvider.License);
                    AppendToString(sb, seg.Output);
                }
            }
            else
            {
                seg = Segment.REF("EI", billingProvider.EIN);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Billing Provider Contact Information
            seg = Segment.PER(
                "IC",
                isCompany == true ?
                    billingProvider.LastName : (billingProvider.FirstName + " " + billingProvider.LastName),
                billingProvider.Phone_1,
                billingProvider.Email,
                billingProvider.Fax
            );
            AppendToString(sb, seg.Output);
            #endregion

            #region Pay-To Provider
            List<PayTo> ptList;
            PayTo payTo = null;
            if (isCompany == true)
            {
                ptList = PayToes.Where(x => x.BillingProviderId == billingProvider.Id &&
                                            x.RenderingProviderId == renderingProvider.Id).ToList();
                if (ptList.Count > 0)
                {
                    payTo = ptList.First();
                }
            }
            else
            {
                ptList = PayToes.Where(x => x.BillingProviderId == billingProvider.Id &&
                                            x.RenderingProviderId == billingProvider.Id).ToList();
                if (ptList.Count > 0)
                {
                    payTo = ptList.First();
                }
            }

            if (payTo != null)
            {
                if (
                    billingProvider.Address_1 != payTo.Address_1 ||
                    billingProvider.Address_2 != payTo.Address_2 ||
                    billingProvider.City != payTo.City ||
                    billingProvider.State != payTo.State ||
                    billingProvider.Zip != payTo.Zip
                )
                {
                    // Pay-To Address Name
                    seg = Segment.NM1("87",
                        payTo.IsCompany == true ? "2" : "1",
                        "", "", "", "", "", "");
                    AppendToString(sb, seg.Output);

                    // Pay-To Address - Address
                    seg = Segment.N3(
                        payTo.Address_1,
                        payTo.Address_2
                    );
                    AppendToString(sb, seg.Output);

                    // Pay-To Address - City, State, Zip
                    seg = Segment.N4(
                        payTo.City,
                        payTo.State,
                        payTo.Zip
                    );
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            #region Get all subscribers under billing & rendering provider combination
            List<Subscriber> subscribers = new List<Subscriber>();
            List<Claim> claims = new List<Claim>();

            if (renderingProvider == null)
            {
                // Claims filed as individual providers
                claims = filteredClaims.Where(x => x.BillingProvider.Id == billingProvider.Id &&
                                                x.RenderingProvider.Id == x.BillingProvider.Id)
                                            .ToList();
                subscribers = claims.Select(x => x.PrimarySubscriber).Distinct().ToList();
            }
            else
            {
                // Claims filed under groups
                claims = filteredClaims.Where(x => x.BillingProvider.Id == billingProvider.Id &&
                                                x.RenderingProvider.Id == renderingProvider.Id).ToList();
                subscribers = claims.Select(x => x.PrimarySubscriber).Distinct().ToList();
            }
            #endregion

            #region Write all subscribers
            foreach (Subscriber sub in subscribers)
            {
                sb.Append(WriteSubscriberLoop(claims, sub));
            }
            #endregion

            return sb.ToString();
        }


        private string WriteSubscriberLoop(List<Claim> filteredClaims, Subscriber subscriber)
        {
            StringBuilder sb = new StringBuilder();
            Segment seg;

            #region Get Self claims and dependent claims
            List<Claim> dependentClaims = filteredClaims
                                        .Where(x => x.PrimarySubscriber.Id == subscriber.Id && x.Relationship != "18")
                                        .ToList();

            List<Claim> selfClaims = filteredClaims
                                        .Where(x => x.PrimarySubscriber.Id == subscriber.Id && x.Relationship == "18")
                                        .ToList();
            #endregion

            #region Subscriber Hierarchy Level
            hl++;
            hlsub = hl;

            seg = Segment.HL(hl, "22", hlprov);
            seg["HL04"] = dependentClaims.Count > 0 ? "1" : "0";
            AppendToString(sb, seg.Output);
            #endregion

            #region Write Self claims
            if (selfClaims.Count > 0)
            {
                sb.Append(WriteSubscriber(subscriber));

                foreach(Claim selfClaim in selfClaims)
                {
                    sb.Append(WriteClaimLoop(selfClaim));
                }
            }
            #endregion

            #region Write dependent claims
            if (dependentClaims.Count > 0)
            {
                // Write subscriber when Subscriber != Patient
                sb.Append(WriteSubscriber(subscriber, false));

                foreach(Claim dependentClaim in dependentClaims)
                {
                    sb.Append(WriteClaimLoop(dependentClaim));
                }
            }
            #endregion

            return sb.ToString();
        }

        private string WriteSubscriber(Subscriber subscriber, bool isSelf = true)
        {
            StringBuilder sb = new StringBuilder();
            Segment seg;

            #region Main Subscriber Segment
            seg = Segment.SBR();
            seg["SBR01"] = "P";
            if (isSelf == true)
            {
                seg["SBR02"] = "18";
            }
            seg["SBR03"] = subscriber.GroupNumber;
            seg["SBR04"] = subscriber.GroupName;
            seg["SBR05"] = subscriber.PrimaryPayerType;
            seg["SBR09"] = "CI";
            AppendToString(sb, seg.Output);
            #endregion

            #region Patient Information (only when Subscriber == Patient)
            if (isSelf == true)
            {
                // Patient Information when Patient == Subscriber
                bool isDeceased = subscriber.IsDeceased == null ?
                    false : subscriber.IsDeceased.Value;
                bool isPregnant = subscriber.IsPregnant == null ?
                    false : subscriber.IsPregnant.Value;
                DateTime dt;
                if (
                    (isDeceased == true && subscriber.DateOfDeath != null) ||
                    (subscriber.Weight != null) ||
                    isPregnant == true
                )
                {
                    seg = Segment.PAT("");

                    if (isDeceased == true && subscriber.DateOfDeath != null)
                    {
                        dt = subscriber.DateOfDeath.Value;
                        seg["PAT05"] = "D8";
                        seg["PAT06"] = dt.Month.ToString().PadLeft(2, '0') + "/" +
                            dt.Day.ToString().PadLeft(2, '0') + "/" +
                            dt.Year.ToString();
                    }

                    if (subscriber.Weight != null)
                    {
                        seg["PAT07"] = "01";
                        seg["PAT08"] = subscriber.Weight.Value.ToString();
                    }

                    if (isPregnant == true)
                    {
                        seg["PAT09"] = "Y";
                    }
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            #region Subscriber Name
            seg = Segment.NM1("IL", "1",
                subscriber.LastName,
                subscriber.FirstName,
                subscriber.MiddleName,
                subscriber.Suffix,
                "MI",
                subscriber.PrimaryMemberID
            );
            AppendToString(sb, seg.Output);
            #endregion

            #region Subscriber Address (only when Subscriber == Patient)
            if (isSelf == true)
            {
                seg = Segment.N3(subscriber.Address_1, subscriber.Address_2);
                AppendToString(sb, seg.Output);

                seg = Segment.N4(subscriber.City, subscriber.State, subscriber.Zip);
                AppendToString(sb, seg.Output);

                seg = Segment.DMG(subscriber.DateOfBirth.Value, subscriber.Gender);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Subscriber SSN
            if (String.IsNullOrEmpty(subscriber.SSN) == false)
            {
                seg = Segment.REF("SY", subscriber.SSN);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Payer Information
            Payer primaryPayer = subscriber.PrimaryPayer;
            seg = Segment.NM1("PR", "2", primaryPayer.Name,
                "", "", "", "XV", primaryPayer.PayerId);
            AppendToString(sb, seg.Output);

            seg = Segment.N3(primaryPayer.Address_1, primaryPayer.Address_2);
            AppendToString(sb, seg.Output);

            seg = Segment.N4(primaryPayer.City, primaryPayer.State, primaryPayer.Zip);
            AppendToString(sb, seg.Output);
            #endregion

            return sb.ToString();
        }

        private string WritePatientLoop(Patient patient, string relationship)
        {
            StringBuilder sb = new StringBuilder();
            Segment seg;

            #region Patient Hierarchy Level
            hl++;
            seg = Segment.HL(hl, "23", hlsub);
            AppendToString(sb, seg.Output);
            #endregion

            #region Patient Segment
            seg = Segment.PAT(relationship, patient);
            AppendToString(sb, seg.Output);
            #endregion

            #region Patient Name, Address
            seg = Segment.NM1("QC", "1", 
                patient.LastName, patient.FirstName, patient.MiddleName, patient.Suffix, "", "");
            AppendToString(sb, seg.Output);

            seg = Segment.N3(patient.Address_1, patient.Address_2);
            AppendToString(sb, seg.Output);

            seg = Segment.N4(patient.City, patient.State, patient.Zip);
            AppendToString(sb, seg.Output);
            #endregion

            #region Patient Demographic Information
            seg = Segment.DMG(patient.DateOfBirth.Value, patient.Gender);
            AppendToString(sb, seg.Output);
            #endregion

            return sb.ToString();
        }

        private string WriteClaimLoop(Claim claim)
        {
            StringBuilder sb = new StringBuilder();
            Segment seg;

            #region If patient is not the subscriber, write the Patient information first
            if (claim.Relationship != "18")
            {
                sb.Append(WritePatientLoop(claim.Patient, claim.Relationship));
            }
            #endregion

            #region Claim Segment
            seg = Segment.CLM(claim, true);

            bool autoAccident = false;
            bool otherAccident = false;
            if (claim.Accident != null)
            {
                string acc = "";
                string[] accArr = claim.Accident.Causes.Split(new[] { ',' });

                autoAccident = claim.Accident.Causes.IndexOf("AA") == 0;
                otherAccident = claim.Accident.Causes.IndexOf("OA") == 0;
                acc += accArr[0];
                acc += ":" + (accArr.Length > 1 ? accArr[1] : "");

                if (autoAccident == true)
                {
                    acc += "::" + claim.Accident.State +
                        (claim.Accident.Country != "US" ? (":" + claim.Accident.Country) : "");
                    seg["CLM11"] = acc;
                }
            }
            AppendToString(sb, seg.Output);
            #endregion

            #region Date of Accident
            if (claim.Accident != null &&
                (autoAccident == true || otherAccident == true)
            )
            {
                seg = Segment.DTP("439", claim.Accident.Date.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Write Claim Dates
            if (claim.Dates != null)
            {
                sb.Append(WriteClaimDates(claim.Dates));
            }
            #endregion

            #region Claim Supplemental Information
            if (claim.Supplemental != null)
            {
                seg = Segment.PWK();
                seg["PWK01"] = claim.Supplemental.AttachmentType;
                seg["PWK02"] = claim.Supplemental.TransmissionType;

                if (
                    seg["PWK02"] == "BM" ||
                    seg["PWK02"] == "EL" ||
                    seg["PWK02"] == "EM" ||
                    seg["PWK02"] == "FX" ||
                    seg["PWK02"] == "FT"
                )
                {
                    seg["PWK06"] = "AC";
                    seg["PWK07"] = claim.Supplemental.AttachmentNumber;
                }

                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Contract Information
            // Currently not for HIPAA use
            #endregion

            #region Amount Paid
            if (claim.AmountCopay != null)
            {
                if (claim.AmountCopay.Value > 0)
                {
                    seg = Segment.AMT(claim.AmountCopay.Value);
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            #region Reference Information
            if (claim.Reference != null)
            {
                sb.Append(WriteClaimReference(claim.Reference));
            }
            #endregion

            #region Claim Notes
            if (String.IsNullOrEmpty(claim.NoteType) == false &&
                String.IsNullOrEmpty(claim.Notes) == false
            )
            {
                seg = Segment.NTE(claim.NoteType, claim.Notes);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Ambulance Information
            if (claim.Ambulance != null)
            {
                ClaimAmbulance amb = claim.Ambulance;
                seg = Segment.CR1();

                if (amb.Weight != null)
                {
                    seg["CR101"] = "LB";
                    seg["CR102"] = amb.Weight.Value.ToString();
                }

                seg["CR104"] = amb.Reason;
                seg["CR105"] = "DH";
                seg["CR106"] = amb.Quantity.Value.ToString();
                seg["CR109"] = String.IsNullOrEmpty(amb.RoundTrip) == false ? amb.RoundTrip : "";
                seg["CR110"] = String.IsNullOrEmpty(amb.Stretcher) == false ? amb.Stretcher : "";
                AppendToString(sb, seg.Output);

                seg = Segment.CRC();
                seg["CRC01"] = "07";
                seg["CRC02"] = amb.CertResponse;
                seg["CRC03"] = amb.CertCode_1;
                seg["CRC04"] = amb.CertCode_2;
                seg["CRC05"] = amb.CertCode_3;
                seg["CRC06"] = amb.CertCode_4;
                seg["CRC07"] = amb.CertCode_5;
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Spinal Information
            if (claim.Spinal != null)
            {
                ClaimSpinal spin = claim.Spinal;
                seg = Segment.CR2();
                seg["CR208"] = spin.Code;
                seg["CR210"] = String.IsNullOrEmpty(spin.Description_1) == false ? spin.Description_1 : "";
                seg["CR211"] = String.IsNullOrEmpty(spin.Description_2) == false ? spin.Description_2 : "";
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region HomeBound Indicator
            if (claim.HomeBound == true)
            {
                seg = Segment.CRC();
                seg["CRC01"] = "75";
                seg["CRC01"] = "Y";
                seg["CRC01"] = "IH";
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Diagnosis Codes
            if (String.IsNullOrEmpty(claim.DiagnosisCodes) == false)
            {
                List<string> diags = claim.DiagnosisCodes.Split(new[] { ',' }).ToList();

                seg = Segment.HI();
                for (int i = 0, n = diags.Count; i < n; i++)
                {
                    seg["HI" + i.ToString().PadLeft(2, '0')] =
                        (i == 0 ? "ABK" : "ABF") + ":" + diags[i];
                }
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Condition Information
            List<string> parts, subparts;

            if (claim.Condition != null)
            {
                #region Vision Indicator
                if (String.IsNullOrEmpty(claim.Condition.Vision) == false)
                {
                    seg = Segment.CRC();

                    parts = claim.Condition.Vision.Split(new[] { ':' }).ToList();
                    seg["CRC01"] = parts[0];
                    seg["CRC02"] = parts[1];

                    subparts = parts[2].Split(new[] { ',' }).ToList();
                    for (int i=0, n=subparts.Count; i<n; i++)
                    {
                        seg["CRC" + (i + 3).ToString().PadLeft(2, '0')] = subparts[i];
                    }
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region EPSDT
                if (String.IsNullOrEmpty(claim.Condition.EPSDT) == false)
                {
                    seg = Segment.CRC();

                    parts = claim.Condition.Vision.Split(new[] { ':' }).ToList();
                    seg["CRC01"] = "ZZ";
                    seg["CRC02"] = parts[0] == "Y" ? "Y" : "N";

                    subparts = parts[1].Split(new[] { ',' }).ToList();
                    for (int i = 0, n = subparts.Count; i < n; i++)
                    {
                        seg["CRC" + (i + 3).ToString().PadLeft(2, '0')] = subparts[i];
                    }
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region Anesthesia
                if (String.IsNullOrEmpty(claim.Condition.Anesthesia) == false)
                {
                    seg = Segment.HI();

                    parts = claim.Condition.Vision.Split(new[] { ':' }).ToList();
                    seg["HI01"] = "BP:" + parts[0];
                    
                    if (parts.Count > 1)
                    {
                        seg["HI02"] = "BO:" + parts[1];
                    }

                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region Claim Conditions
                if (String.IsNullOrEmpty(claim.Condition.Conditions) == false)
                {
                    parts = claim.Condition.Conditions.Split(new[] { ',' }).ToList();
                    seg = Segment.HI();

                    for (int i=0, n=parts.Count; i<n; i++)
                    {
                        seg["HI" + (i + 1).ToString().PadLeft(2, '0')] = "BG:" + parts[i];
                    }

                    AppendToString(sb, seg.Output);
                }
                #endregion
            }
            #endregion

            #region Repricing Information
            if (claim.Repricing != null)
            {
                ClaimRepricing rpr = claim.Repricing;
                seg = Segment.HCP();
                seg["HCP01"] = rpr.Methodology;
                seg["HCP02"] = rpr.AllowedAmount.Value.ToString();

                if (rpr.AllowedAmount != null)
                {
                    if (rpr.AllowedAmount.Value > 0)
                    {
                        seg["HCP03"] = rpr.SavingAmount.Value.ToString();
                    }
                }

                seg["HCP04"] = rpr.OrgIdentifier;

                if (rpr.Rate != null)
                {
                    if (rpr.Rate.Value > 0)
                    {
                        seg["HCP05"] = rpr.Rate.Value.ToString();
                    }
                }

                seg["HCP06"] = rpr.AmbulatoryGroup;

                if (rpr.AmbulatoryAmount != null)
                {
                    if (rpr.AmbulatoryAmount.Value > 0)
                    {
                        seg["HCP07"] = rpr.AmbulatoryAmount.Value.ToString();
                    }
                }

                seg["HCP13"] = (String.IsNullOrEmpty(rpr.RejectReason) == true) ?
                    String.Empty : rpr.RejectReason;

                seg["HCP14"] = (String.IsNullOrEmpty(rpr.PolicyCompliance) == true) ?
                    String.Empty : rpr.PolicyCompliance;

                seg["HCP15"] = (String.IsNullOrEmpty(rpr.Exception) == true) ?
                    String.Empty : rpr.Exception;

                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Referring Provider
            Provider referring = claim.ReferringProvider;
            if (referring != null)
            {
                seg = Segment.NM1("DN", "1",
                    referring.LastName, referring.FirstName, referring.MiddleName,
                    referring.Suffix, "XX", referring.NPI
                );
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Rendering Provider
            Provider rendering = claim.RenderingProvider;
            seg = Segment.NM1("82", "2",
                rendering.LastName, rendering.FirstName, rendering.MiddleName,
                rendering.Suffix, "XX", rendering.NPI
            );
            AppendToString(sb, seg.Output);

            seg = Segment.PRV("PE", "PXC", rendering.TaxonomyCode.Code);
            AppendToString(sb, seg.Output);

            if (String.IsNullOrEmpty(rendering.License) == false)
            {
                seg = Segment.REF("0B", rendering.License);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Facility
            Facility facility = claim.Facility;
                
            #region Facility Name
            seg = Segment.NM1("77", "2",
                facility.Name, "", "", "",
                "XX", facility.NPI
            );
            AppendToString(sb, seg.Output);
            #endregion

            #region Facility Address
            seg = Segment.N3(facility.Address_1, facility.Address_2);
            AppendToString(sb, seg.Output);

            seg = Segment.N4(
                facility.City,
                facility.State,
                facility.Zip
            );
            AppendToString(sb, seg.Output);
            #endregion

            #region Facility Contact Information
            seg = Segment.PER("IC", facility.ContactName,
                facility.Phone, facility.Email, facility.Fax
            );
            #endregion
            AppendToString(sb, seg.Output);
            #endregion

            // SUPERVISING PROVIDER - NOT YET IMPLEMENTED
            // AMBULANCE PICK-UP/DROP-OFF - NOT YET IMPLEMENTED
            // OTHER SUBSCRIBER - NOT YET IMPLEMENTED
            // OTHER PAYER - NOT YET IMPLEMENTED
            // CLAIM LEVEL ADJUSTMENTS - NOT YET IMPLEMENTED

            #region Claim Amounts
            if (claim.Amounts != null)
            {
                ClaimAmount amt = claim.Amounts;

                #region Payer Paid Amount
                if (amt.PayerPaid != null)
                {
                    if (amt.PayerPaid.Value > 0)
                    {
                        seg = Segment.AMT(amt.PayerPaid.Value);
                        seg["AMT01"] = "D";
                        AppendToString(sb, seg.Output);
                    }
                }
                #endregion

                #region Total Non-Covered Amount
                if (amt.NonCovered != null)
                {
                    if (amt.NonCovered.Value > 0)
                    {
                        seg = Segment.AMT(amt.NonCovered.Value);
                        seg["AMT01"] = "A8";
                        AppendToString(sb, seg.Output);
                    }
                }
                #endregion

                #region Remaining Patient Liability
                if (amt.PatientLiability != null)
                {
                    if (amt.PatientLiability.Value > 0)
                    {
                        seg = Segment.AMT(amt.PatientLiability.Value);
                        seg["AMT01"] = "EAF";
                        AppendToString(sb, seg.Output);
                    }
                }
                #endregion
            }
            #endregion

            List<ClaimLine> claimLines = claim.ClaimLines.OrderBy(x => x.OrderLine).ToList();

            foreach(ClaimLine line in claimLines)
            {
                sb.Append(WriteClaimLineLoop(line));
            }

            return sb.ToString();
        }

        private string WriteClaimDates(ClaimDate dates)
        {
            StringBuilder sb = new StringBuilder();
            Segment seg;

            #region Onset of Current Illness
            if (dates.OnsetOfCurrent != null)
            {
                seg = Segment.DTP("431", dates.OnsetOfCurrent.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Initial Treatment Date
            if (dates.InitialTreatment != null)
            {
                seg = Segment.DTP("454", dates.InitialTreatment.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Last Seen Date
            if (dates.LastSeen != null)
            {
                seg = Segment.DTP("304", dates.LastSeen.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Acute Manifestation Date
            if (dates.Acute != null)
            {
                seg = Segment.DTP("453", dates.Acute.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Last Menstrual Period
            if (dates.LastMenstrual != null)
            {
                seg = Segment.DTP("484", dates.LastMenstrual.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Last X-Ray Date
            if (dates.LastXRay != null)
            {
                seg = Segment.DTP("455", dates.LastXRay.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Hearing and Vision Prescription Date
            if (dates.HearingVision != null)
            {
                seg = Segment.DTP("471", dates.HearingVision.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Disability Dates
            if (dates.DisabilityStart != null && dates.DisabilityEnd != null)
            {
                seg = Segment.DTP("314", dates.DisabilityStart.Value);
                seg["DTP02"] = "RD8";
                AppendToString(sb, seg.Output);
            }
            else if (dates.DisabilityStart != null && dates.DisabilityEnd == null)
            {
                seg = Segment.DTP("360", dates.DisabilityStart.Value);
                AppendToString(sb, seg.Output);
            }
            else if (dates.DisabilityStart == null && dates.DisabilityEnd != null)
            {
                seg = Segment.DTP("361", dates.DisabilityEnd.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Date Last Worked
            if (dates.LastWorked != null)
            {
                seg = Segment.DTP("297", dates.LastWorked.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Authorized Return To Work
            if (dates.ReturnToWork != null)
            {
                seg = Segment.DTP("296", dates.ReturnToWork.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Admission Date
            if (dates.Admission != null)
            {
                seg = Segment.DTP("435", dates.Admission.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Discharge Date
            if (dates.Discharge != null)
            {
                seg = Segment.DTP("096", dates.Discharge.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Assumed/Relinquished Care Dates
            if (dates.Assumed != null)
            {
                seg = Segment.DTP("090", dates.Assumed.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Property and Casualty Date of First Contact
            if (dates.Property != null)
            {
                seg = Segment.DTP("444", dates.Property.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Repricer Received Date
            if (dates.Repricer != null)
            {
                seg = Segment.DTP("050", dates.Repricer.Value);
                AppendToString(sb, seg.Output);
            }
            #endregion

            return sb.ToString();
        }

        private string WriteClaimReference(ClaimReference cref)
        {
            StringBuilder sb = new StringBuilder();
            Segment seg;

            #region Service Authorization Exception Code
            if (String.IsNullOrEmpty(cref.AuthorizationException) == false)
            {
                seg = Segment.REF("4N", cref.AuthorizationException);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Mandatory Medicare Crossover Indicator
            if (String.IsNullOrEmpty(cref.MandatoryMedicare) == false)
            {
                seg = Segment.REF("F5", cref.MandatoryMedicare);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Mammography Certification Number
            if (String.IsNullOrEmpty(cref.Mammography) == false)
            {
                seg = Segment.REF("EW", cref.Mammography);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Referral Number
            if (String.IsNullOrEmpty(cref.Referral) == false)
            {
                seg = Segment.REF("9F", cref.Referral);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Prior Authorization
            if (String.IsNullOrEmpty(cref.Prior) == false)
            {
                seg = Segment.REF("G1", cref.Prior);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Payer Claim Control Number
            if (String.IsNullOrEmpty(cref.ClaimControl) == false)
            {
                seg = Segment.REF("F8", cref.ClaimControl);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region CLIA
            if (String.IsNullOrEmpty(cref.CLIA) == false)
            {
                seg = Segment.REF("X4", cref.CLIA);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Repriced Claim Number
            if (String.IsNullOrEmpty(cref.Repriced) == false)
            {
                seg = Segment.REF("9A", cref.Repriced);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Adjusted Repriced Claim Number
            if (String.IsNullOrEmpty(cref.AdjustedRepriced) == false)
            {
                seg = Segment.REF("9C", cref.AdjustedRepriced);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Investigational Device Exemption Number
            if (String.IsNullOrEmpty(cref.InvestigationalDevice) == false)
            {
                seg = Segment.REF("LX", cref.InvestigationalDevice);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Claim Identifier for Transmission Intermediaries
            if (String.IsNullOrEmpty(cref.Transmission) == false)
            {
                seg = Segment.REF("D9", cref.Transmission);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Medical Record Number
            if (String.IsNullOrEmpty(cref.MedicalRecord) == false)
            {
                seg = Segment.REF("EA", cref.MedicalRecord);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Demonstration Project Identifier
            if (String.IsNullOrEmpty(cref.Demonstration) == false)
            {
                seg = Segment.REF("P4", cref.Demonstration);
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Care Plan Oversight
            if (String.IsNullOrEmpty(cref.CarePlanOversight) == false)
            {
                seg = Segment.REF("1J", cref.CarePlanOversight);
                AppendToString(sb, seg.Output);
            }
            #endregion

            return sb.ToString();
        }

        private string WriteClaimLineLoop(ClaimLine line)
        {
            StringBuilder sb = new StringBuilder();
            Segment seg;

            #region LX: Service Line Number
            seg = new Segment("LX");
            seg["LX01"] = line.OrderLine.ToString();
            AppendToString(sb, seg.Output);
            #endregion

            #region Date of Service
            seg = new Segment("DTP");
            seg["DTP01"] = "472";
            seg["DTP02"] = "RD8";
            seg["DTP03"] = Segment.ParseDateTimeToString(line.StartDate);
            seg["DTP04"] = Segment.ParseDateTimeToString(line.EndDate);
            AppendToString(sb, seg.Output);
            #endregion

            #region SV1: Profesional Service
            seg = new Segment("SV1");
            seg["SV101"] = "HC:" + line.CPT;
            if (string.IsNullOrEmpty(line.Modifier) == false)
            {
                string[] mods = line.Modifier.Split(new[] { ',' });
                foreach(string m in mods)
                {
                    seg["SV101"] += ":" + m;
                }
            }
            seg["SV102"] = Segment.GetDecimalString(line.Amount);
            seg["SV103"] = line.Unit;
            seg["SV104"] = line.Quantity.ToString();
            seg["SV107"] = ParsePointer(line.Pointer);
            seg["SV109"] = line.IsEmergency == true ? "Y" : String.Empty;
            seg["SV111"] = line.EPSDT == true ? "Y" : String.Empty;
            seg["SV112"] = line.FamilyPlanning == true ? "Y" : String.Empty;
            seg["SV115"] = line.CopayExempt == true ? "0" : String.Empty;
            AppendToString(sb, seg.Output);
            #endregion

            #region Claim Line Durables
            ClaimLineDurable dur = line.Durable;
            if (dur != null)
            {
                #region SV5: Durable Medical Equipment Service
                seg = new Segment("SV5");
                seg["seg01"] = "HC:" + dur.Procedure;
                seg["seg02"] = "DA";
                seg["seg03"] = dur.Quantity.ToString();
                seg["seg04"] = (dur.Rental == null ? "" : dur.Rental.Value.ToString());
                seg["seg05"] = (dur.Purchase == null ? "" : dur.Purchase.Value.ToString());
                seg["seg06"] = dur.Frequency;
                AppendToString(sb, seg.Output);
                #endregion

                #region  PWK: Durable Medical Equipment Certificate
                seg = new Segment("PWK");
                seg["PWK01"] = "CT";
                seg["PWK02"] = dur.Transmission;
                AppendToString(sb, seg.Output);
                #endregion

                #region CR3: Durable Medical Equipment Certification
                seg = new Segment("CR3");
                seg["CR301"] = dur.CertCode;
                seg["CR302"] = "MO";
                seg["CR303"] = (dur.CertQuantity == null ? String.Empty : dur.CertQuantity.Value.ToString());
                AppendToString(sb, seg.Output);
                #endregion
            }
            #endregion

            #region PWK: Line Supplemental Information
            ClaimLineAttachment att = line.Attachment;
            if (att != null)
            {
                seg = new Segment("PWK");
                seg["PWK01"] = att.Type;
                seg["PWK02"] = att.Transmission;

                if (
                    seg["PWK02"] == "BM" ||
                    seg["PWK02"] == "EL" ||
                    seg["PWK02"] == "EM" ||
                    seg["PWK02"] == "FX" ||
                    seg["PWK02"] == "FT"
                )
                {
                    seg["PWK05"] = "AC";
                    seg["PWK06"] = att.Code;
                }
                AppendToString(sb, seg.Output);
            }
            #endregion

            #region Ambulance
            ClaimLineAmbulance amb = line.Ambulance;
            if (amb != null)
            {
                #region CR1: Ambulance Transport Information
                seg = new Segment("CR1");
                seg["CR101"] = "LB";
                seg["CR102"] = amb.Weight.ToString();
                seg["CR104"] = amb.Reason;
                seg["CR105"] = "DH";
                seg["CR106"] = (amb.Quantity == null ? String.Empty : amb.Quantity.Value.ToString());
                seg["CR109"] = amb.RoundTrip;
                seg["CR110"] = amb.Stretcher;
                AppendToString(sb, seg.Output);
                #endregion

                #region CRC: Ambulance Certification
                seg = new Segment("CRC");
                seg["CRC01"] = "07";
                seg["CRC02"] = amb.CertResponse;
                seg["CRC03"] = amb.CertCode_1;
                seg["CRC04"] = amb.CertCode_2;
                seg["CRC05"] = amb.CertCode_3;
                seg["CRC06"] = amb.CertCode_4;
                seg["CRC07"] = amb.CertCode_5;
                AppendToString(sb, seg.Output);
                #endregion

                #region  QTY: Ambulance Patient Count
                seg = new Segment("QTY");
                seg["QTY01"] = "PT";
                seg["QTY02"] = amb.PatientCount.ToString();
                AppendToString(sb, seg.Output);
                #endregion
            }
            #endregion

            #region MEA: Test Result
            ClaimLineDialysis dia = line.Dialysis;
            if (dia != null)
            {
                if (String.IsNullOrEmpty(dia.Code) == false &&
                    String.IsNullOrEmpty(dia.Qualifier) == false)
                {
                    seg = new Segment("MEA");
                    seg["MEA01"] = dia.Code;
                    seg["MEA02"] = dia.Qualifier;
                    seg["MEA03"] = dia.Value;
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            #region CN1: Contract Information
            ClaimLineContract con = line.Contract;
            if (con != null)
            {
                if (String.IsNullOrEmpty(con.Type) == false)
                {
                    seg = new Segment("CN1");
                    seg["CN101"] = con.Type;
                    seg["CN102"] = con.Amount == null ? String.Empty : con.Amount.Value.ToString();
                    seg["CN103"] = con.Percent == null ? String.Empty : con.Percent.Value.ToString();
                    seg["CN104"] = con.Code;
                    seg["CN105"] = con.Discount == null ? String.Empty: con.Discount.Value.ToString();
                    seg["CN106"] = con.Version;
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            #region Claim Line Supplementals
            ClaimLineSupplemental sup = line.Supplemental;
            if (line.Supplemental != null)
            {
                #region CRC: Hospice Employee Indicator
                if (String.IsNullOrEmpty(sup.HospiceCode) == false)
                {
                    seg = new Segment("CRC");
                    seg["CRC01"] = "70";
                    seg["CRC02"] = sup.HospiceCode;
                    seg["CRC03"] = "65";
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region QTY: Obstetric Anesthesia Additional Units
                if (sup.ObsAnesUnits > 0)
                {
                    seg = new Segment("QTY");
                    seg["QTY01"] = "FL";
                    seg["QTY02"] = (sup.ObsAnesUnits == null ? "" : sup.ObsAnesUnits.Value.ToString());
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region AMT: Sales Tax Amount
                if (sup.SalesTaxAmount != null)
                {
                    seg = new Segment("AMT");
                    seg["AMT01"] = "T";
                    seg["AMT02"] = sup.SalesTaxAmount.Value.ToString();
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region AMT: Postage Claimed Amount
                if (sup.PostageAmount != null)
                {
                    seg = new Segment("AMT");
                    seg["AMT01"] = "F4";
                    seg["AMT02"] = sup.PostageAmount.Value.ToString();
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region K3 : File Information
                if (String.IsNullOrEmpty(sup.FileInfo) == false)
                {
                    seg = new Segment("K3");
                    seg["K301"] = sup.FileInfo;
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region NTE: Line Note
                if (String.IsNullOrEmpty(sup.NoteCode) == false)
                {
                    seg = new Segment("NTE");
                    seg["NTE01"] = sup.NoteCode;
                    seg["NTE02"] = sup.NoteDescription;
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region NTE: Third Party Organization Notes
                if (String.IsNullOrEmpty(sup.ThirdPartyCode) == false)
                {
                    seg = new Segment("NTE");
                    seg["NTE01"] = sup.ThirdPartyCode;
                    seg["NTE02"] = sup.ThirdPartyDescription;
                    AppendToString(sb, seg.Output);
                }
                #endregion

                #region PS1: Purchased Service Information
                if (String.IsNullOrEmpty(sup.PurchaseCode) == false)
                {
                    seg = new Segment("PS1");
                    seg["PS101"] = sup.PurchaseCode;
                    seg["PS101"] = sup.PurchaseAmount == null ? String.Empty : sup.PurchaseAmount.Value.ToString();
                    AppendToString(sb, seg.Output);
                }
                #endregion
            }
            #endregion

            #region HCP: Line Pricing/Repricing Information
            ClaimLinePricing pri = line.Pricing;
            if (pri != null)
            {
                if (String.IsNullOrEmpty(pri.Method) == false)
                {
                    seg = new Segment("HCP");
                    seg["HCP01"] = pri.Method;
                    seg["HCP02"] = pri.AmountAllowed == null ? String.Empty : pri.AmountAllowed.Value.ToString();
                    seg["HCP03"] = pri.AmountSavings == null ? String.Empty : pri.AmountSavings.Value.ToString();
                    seg["HCP04"] = pri.OrgID;
                    seg["HCP05"] = pri.Rate == null ? String.Empty : pri.Rate.Value.ToString();
                    seg["HCP06"] = pri.AmbGroup;
                    seg["HCP07"] = pri.AmbGroupAmount == null ? String.Empty : pri.AmbGroupAmount.Value.ToString();
                    seg["HCP09"] = pri.ProductQualifier;
                    seg["HCP10"] = pri.HCPCS;
                    seg["HCP11"] = pri.Unit;
                    seg["HCP12"] = pri.Quantity == null ? String.Empty : pri.Quantity.Value.ToString();
                    seg["HCP13"] = pri.RejectReason;
                    seg["HCP14"] = pri.ComplianceCode;
                    seg["HCP15"] = pri.ExceptionCode;
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            #region Claim Line Drug Information
            ClaimLineDrug dru = line.Drug;
            if (dru != null)
            {
                #region LIN: Drug Identification
                seg = new Segment("LIN");
                seg["LIN02"] = dru.Qualifier;
                seg["LIN03"] = dru.Code;
                AppendToString(sb, seg.Output);
                #endregion

                #region REF: Prescription or Compound Drug Association Number
                seg = new Segment("CTP");
                seg["CTP04"] = dru.Quantity == null ? String.Empty : dru.Quantity.Value.ToString();
                seg["CTP05"] = dru.Unit;
                AppendToString(sb, seg.Output);

                if (String.IsNullOrEmpty(dru.PrescriptionCode) == false)
                {
                    seg = GetREF(dru.PrescriptionCode, dru.PrescriptionNumber);
                    AppendToString(sb, seg.Output);
                }
                #endregion
            }
            #endregion

            #region CRC: Condition Indicator/Durable Medical Equipment
            ClaimLineDMERC dme = line.DMERC;
            if (dme != null)
            {
                if (String.IsNullOrEmpty(dme.DMERC_Code) == false &&
                    String.IsNullOrEmpty(dme.DMERC_Response) == false)
                {
                    seg = new Segment("CRC");
                    seg["CRC01"] = "60";
                    seg["CRC02"] = dme.DMERC_Response;
                    seg["CRC03"] = dme.DMERC_Code;
                    seg["CRC04"] = dme.DMERC_Code2;
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            #region Claim Line Dates
            if (line.Dates != null)
            {
                DateTime val;

                // DTP: Prescription Date
                val = line.Dates.Prescription.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("471", val));
                }

                // DTP: Certification/Recertification Revision Date
                val = line.Dates.Certification.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("607", val));
                }

                // DTP: Begin Therapy Date
                val = line.Dates.BeginTherapy.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("463", val));
                }

                // DTP: Last Certification Date
                val = line.Dates.LastCertification.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("461", val));
                }

                // DTP: Last Seen Date
                val = line.Dates.LastSeen.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("304", val));
                }

                // DTP: Test Date
                val = line.Dates.BeginTherapy.Value;
                if (val != null)
                {
                    // Code = 738 or 739
                    sb.Append(WriteDTP("738", val));
                }

                // DTP: Shipped Date
                val = line.Dates.Shipped.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("011", val));
                }

                // DTP: Last X-ray Date
                val = line.Dates.LastXRay.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("455", val));
                }

                // DTP: Initial Treatment Date
                val = line.Dates.InitialTreatment.Value;
                if (val != null)
                {
                    sb.Append(WriteDTP("454", val));
                }
            }
            #endregion

            #region Claim Line Reference
            if (line.Reference != null)
            {
                // REF: Repriced Line Item Reference Number
                if (String.IsNullOrEmpty(line.Reference.Repriced) == false)
                {
                    seg = GetREF("9B", line.Reference.Repriced);
                    AppendToString(sb, seg.Output);
                }

                // REF: Adjusted Repriced Line Item Reference Number
                if (String.IsNullOrEmpty(line.Reference.AdjustedRepriced) == false)
                {
                    seg = GetREF("9D", line.Reference.AdjustedRepriced);
                    AppendToString(sb, seg.Output);
                }

                // REF: Prior Authorization
                if (String.IsNullOrEmpty(line.Reference.Prior) == false)
                {
                    seg = GetREF("G1", line.Reference.Prior);
                    if (String.IsNullOrEmpty(line.Reference.PriorIdentifier) == false)
                    {
                        seg["REF04"] = "2U:" + line.Reference.PriorIdentifier;
                    }

                    AppendToString(sb, seg.Output);
                }

                // REF: Line Item Control Number
                if (String.IsNullOrEmpty(line.Reference.ControlNumber) == false)
                {
                    seg = GetREF("6R", line.Reference.ControlNumber);
                    AppendToString(sb, seg.Output);
                }

                // REF: Mammography Certification Number
                if (String.IsNullOrEmpty(line.Reference.Mammography) == false)
                {
                    seg = GetREF("EW", line.Reference.Mammography);
                    AppendToString(sb, seg.Output);
                }

                // REF: CLIA
                if (String.IsNullOrEmpty(line.Reference.CLIA) == false)
                {
                    seg = GetREF("X4", line.Reference.CLIA);
                    AppendToString(sb, seg.Output);
                }

                // REF: Referring CLIA Facility
                if (String.IsNullOrEmpty(line.Reference.CLIAFacility) == false)
                {
                    seg = GetREF("F4", line.Reference.CLIAFacility);
                    AppendToString(sb, seg.Output);
                }

                // REF: Immunization Number
                if (String.IsNullOrEmpty(line.Reference.Immunization) == false)
                {
                    seg = GetREF("BT", line.Reference.Immunization);
                    AppendToString(sb, seg.Output);
                }

                // REF: Referral Number
                if (String.IsNullOrEmpty(line.Reference.Referral) == false)
                {
                    seg = GetREF("9F", line.Reference.Referral);
                    AppendToString(sb, seg.Output);
                }
            }
            #endregion

            return sb.ToString();
        }

        private string ParsePointer(string ptr)
        {
            StringBuilder sb = new StringBuilder();

            if (String.IsNullOrEmpty(ptr) == true)
            {
                return sb.ToString();
            }

            for(int i=0, n=ptr.Length; i<n; i++)
            {
                sb.Append(ptr[i]).Append(i < (n - 1) ? ":" : "");
            }

            return sb.ToString();
        }

        private string WriteDTP(string code, DateTime dt)
        {
            Segment seg = new Segment("DTP");
            seg["DTP01"] = code;
            seg["DTP02"] = "D8";
            seg["DTP03"] = String.Format("{0}{1}{2}",
                dt.Year.ToString(),
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0')
                );
            segmentCount++;
            return seg.Output;
        }

        private Segment GetREF(string qualifier, string identifier)
        {
            Segment seg = new Segment("REF");
            seg["REF01"] = qualifier;
            seg["REF02"] = identifier;
            return seg;
        }

        #endregion
    }
}