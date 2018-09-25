using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.ansi5010
{
    public class Segment
    {
        private static int segmentCount = 0;
        private static int hLevel = 1;
        private static int pLevel = 0;

        public Segment(string segmentID, string loopID = "")
        {
            ID = segmentID;
            Loop = loopID;
            Items = new Dictionary<string, string>();

            int pCount = SegmentBuilder.Segments[ID];

            if (pCount <= 0)
            {
                return;
            }

            for(int i=1; i<=pCount; i++)
            {
                Items.Add(segmentID + i.ToString().PadLeft(2, '0'), String.Empty);
            }
        }

        #region Properties
        public string ID { get; set; }

        public string Loop { get; set; }

        public Dictionary<string, string> Items { get; set; }

        public string this[string key]
        {
            get
            {
                return Items[key];
            }
            set
            {
                Items[key] = value;
            }
        }

        public string Output
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                var keys_sorted = Items.Keys.ToList();
                int skipped = 1;

                keys_sorted.Sort();
                sb.Append(ID);

                foreach (var k in keys_sorted)
                {
                    if (String.IsNullOrEmpty(Items[k]) == true)
                    {
                        skipped++;
                    }
                    else
                    {
                        for (int i = 1; i <= skipped; i++)
                        {
                            sb.Append("*");
                        }

                        sb.Append(Items[k]);
                        skipped = 1;
                    }
                }

                return sb.Append("~")
                    //.Append(X837Config.SeparateNewLine == true ? Environment.NewLine : "")
                    .ToString();
            }
        }

        /*
        public string Output_2
        {
            get
            {
                var keys_sorted = Items.Keys.ToList();
                keys_sorted.Sort();

                X837Writer w = new X837Writer(ID);

                foreach (var k in keys_sorted)
                {
                    w.AddElement(Items[k]);
                }

                return w.Output;
            }
        }
        */

        public static string GetDecimalString(Decimal dec)
        {
            string str = dec.ToString();
            if (str.IndexOf(".00") > 0)
            {
                str = str.Substring(0, str.IndexOf(".00"));
            }
            return str;
        }

        public static string GetDecimalString(Decimal? dec)
        {
            string str = "";
            if (dec != null)
            {
                str = GetDecimalString(dec.Value);
            }

            return str;
        }

        public static string ParseDateTimeToString(DateTime dt)
        {
            return String.Format("{0}{1}{2}",
                            dt.Year.ToString(),
                            dt.Month.ToString().PadLeft(2, '0'),
                            dt.Day.ToString().PadLeft(2, '0')
                            );
        }

        public static int ParentLevel
        {
            get => pLevel;
            set => pLevel = value;
        }

        public static int HierarchyLevel
        {
            get => hLevel;
            set => hLevel = value;
        }
        #endregion

        #region Static Methods
        public static Segment AMT(decimal amt)
        {
            Segment seg = new Segment("AMT");
            seg["AMT01"] = "F5";
            seg["AMT02"] = amt.ToString();

            return seg;
        }

        public static Segment BHT(bool original, string refID, DateTime claimDate, string tranType)
        {
            Segment seg = new Segment("BHT");
            seg["BHT01"] = "0019";
            seg["BHT02"] = (original == true ? "00" : "18");
            seg["BHT03"] = refID;

            seg["BHT04"] = X837Writer.WriteDate(claimDate);
            /*String.Format("{0}{1}{2}",
            claimDate.Year.ToString(),
            claimDate.Month.ToString().PadLeft(2, '0'),
            claimDate.Day.ToString().PadLeft(2, '0'));*/

            seg["BHT05"] = X837Writer.WriteTime(claimDate);
                /*String.Format("{0}{1}",
                claimDate.Hour.ToString().PadLeft(2, '0'),
                claimDate.Minute.ToString().PadLeft(2, '0'));*/

            seg["BHT06"] = tranType;

            segmentCount++;
            return seg;
        }

        // WIP
        public static Segment CAS()
        {
            Segment seg = new Segment("CAS");

            return seg;
        }

        public static Segment CLM(Claim claim, bool signatureOnFile = false)
        {
            Segment seg = new Segment("CLM");
            seg["CLM01"] = claim.Id.ToString().PadLeft(8, '0');
            seg["CLM02"] = GetDecimalString(claim.AmountTotal);
            seg["CLM05"] = claim.PlaceOfService.Code + ":B:1";
            seg["CLM06"] = signatureOnFile == true ? "Y" : "N";
            seg["CLM07"] = claim.AcceptAssignment;
            seg["CLM08"] = "";
            seg["CLM09"] = "Y";

            seg["CLM12"] = claim.SpecialProgram;
            seg["CLM20"] = claim.DelayReason;

            segmentCount++;
            return seg;
        }

        // WIP
        public static Segment CN1()
        {
            Segment seg = new Segment("CN1");

            return seg;
        }

        // WIP
        public static Segment CR1()
        {
            Segment seg = new Segment("CR1");

            return seg;
        }

        // WIP
        public static Segment CR2()
        {
            Segment seg = new Segment("CR2");

            return seg;
        }

        // WIP
        public static Segment CRC()
        {
            Segment seg = new Segment("CRC");

            return seg;
        }

        // WIP
        public static Segment CTP()
        {
            Segment seg = new Segment("CTP");

            return seg;
        }

        public static Segment DMG(DateTime dob, string gender)
        {
            Segment seg = new Segment("DMG");
            seg["DMG01"] = "D8";
            seg["DMG02"] = String.Format("{0}{1}{2}",
                dob.Year.ToString(),
                dob.Month.ToString().PadLeft(2, '0'),
                dob.Day.ToString().PadLeft(2, '0'));
            seg["DMG03"] = gender;

            segmentCount++;
            return seg;
        }

        public static Segment DTP(string qualifier, DateTime dt)
        {
            Segment seg = new Segment("DTP");
            seg["DTP01"] = qualifier;
            seg["DTP02"] = "D8";
            seg["DTP03"] = ParseDateTimeToString(dt);

            return seg;
        }

        public static Segment FRM()
        {
            Segment seg = new Segment("FRM");

            return seg;
        }

        public static Segment GE()
        {
            Segment seg = new Segment("GE");

            return seg;
        }

        public static Segment GS()
        {
            Segment seg = new Segment("GS");

            return seg;
        }

        // WIP
        public static Segment HCP()
        {
            Segment seg = new Segment("HCP");

            return seg;
        }

        // WIP
        public static Segment HI()
        {
            Segment seg = new Segment("HI");

            return seg;
        }

        public static Segment HL(int level, string code, int parent = 0)
        {
            Segment seg = new Segment("HL");
            seg["HL01"] = level.ToString();
            seg["HL02"] = parent > 0 ? parent.ToString() : "";
            seg["HL03"] = code.ToString();
            //seg["HL04"] = code;

            return seg;
        }

        public static Segment IEA(string interCtrl)
        {
            Segment seg = new Segment("IEA");
            seg["IEA01"] = "1";
            seg["IEA01"] = interCtrl;

            return seg;
        }

        // WIP
        public static Segment ISA()
        {
            Segment seg = new Segment("ISA");

            return seg;
        }

        // WIP
        public static Segment K3()
        {
            Segment seg = new Segment("K3");

            return seg;
        }

        // WIP
        public static Segment LIN()
        {
            Segment seg = new Segment("LIN");

            return seg;
        }

        public static Segment LQ(string qualifier, string code)
        {
            Segment seg = new Segment("LQ");
            seg["LQ01"] = qualifier;
            seg["LQ02"] = code;

            return seg;
        }

        public static Segment LX(int counter)
        {
            Segment seg = new Segment("LX");
            seg["LX01"] = counter.ToString();

            return seg;
        }

        // WIP
        public static Segment MEA()
        {
            Segment seg = new Segment("MEA");

            return seg;
        }

        // WIP
        public static Segment MOA()
        {
            Segment seg = new Segment("MOA");

            return seg;
        }

        public static Segment N3(string address1, string address2)
        {
            Segment seg = new Segment("N3");
            seg["N301"] = address1;
            if (String.IsNullOrEmpty(address2) == false)
            {
                seg["N302"] = address2;
            }

            segmentCount++;
            return seg;
        }

        public static Segment N4(string city, string state, string zip)
        {
            Segment seg = new Segment("N4");
            seg["N401"] = city;
            seg["N402"] = state;
            seg["N403"] = zip;

            segmentCount++;
            return seg;
        }

        public static Segment NM1(string entity, string entityType,
            string lastName, string firstName, string middleName, string suffix,
            string codeQualifier, string code)
        {
            Segment seg = new Segment("NM1");
            seg["NM101"] = entity;

            if (entity == "87")
            {
                seg["NM102"] = "2";
            }
            else if (entity == "85")
            {
                seg["NM102"] = entityType;
            }
            if (String.IsNullOrEmpty(lastName) == false && String.IsNullOrEmpty(firstName) == false)
            {
                seg["NM102"] = "1";
                seg["NM103"] = lastName;
                seg["NM104"] = firstName;
                seg["NM105"] = middleName;
                seg["NM107"] = suffix;
            }
            else
            {
                seg["NM102"] = "2";
                seg["NM103"] = lastName;
            }

            seg["NM108"] = codeQualifier;
            seg["NM109"] = code;

            segmentCount++;
            return seg;
        }

        public static Segment NTE(string code, string description)
        {
            Segment seg = new Segment("NTE");
            seg["NTE01"] = code;
            seg["NTE02"] = description;

            return seg;
        }

        // WIP
        public static Segment OI()
        {
            Segment seg = new Segment("OI");

            return seg;
        }

        public static Segment PAT(string relationship, Patient patient = null)
        {
            Segment seg = new Segment("PAT");
            seg["PAT01"] = relationship;

            if (patient.IsDeceased == true)
            {
                seg["PAT05"] = "D8";
                seg["PAT06"] = String.Format("{0}{1}{2}",
                    patient.DateOfDeath.Value.Year.ToString(),
                    patient.DateOfDeath.Value.Month.ToString().PadLeft(2, '0'),
                    patient.DateOfDeath.Value.Day.ToString().PadLeft(2, '0'));
                seg["PAT07"] = "01";
                seg["PAT08"] = patient.Weight.Value.ToString();
            }

            if (patient.IsPregnant == true)
            {
                seg["PAT09"] = "Y";
            }

            segmentCount++;
            return seg;
        }

        public static Segment PER(string code, string name, string phone, string email, string fax = "")
        {
            bool hasPhone = false, hasEmail = false, hasFax = false;
            Segment seg = new Segment("PER");
            seg["PER01"] = code; // IC
            seg["PER02"] = name;

            string id1, id2;
            for (int i = 1; i <= 3; i++)
            {
                id1 = ((i * 2) + 1).ToString().PadLeft(2, '0');
                id2 = ((i * 2) + 2).ToString().PadLeft(2, '0');
                if (String.IsNullOrEmpty(phone) == false && hasPhone == false)
                {
                    seg["PER" + id1] = "TE";
                    seg["PER" + id2] = phone;
                    hasPhone = true;
                    continue;
                }
                if (String.IsNullOrEmpty(email) == false && hasEmail == false)
                {
                    seg["PER" + id1] = "EM";
                    seg["PER" + id2] = email;
                    hasEmail = true;
                    continue;
                }
                if (String.IsNullOrEmpty(fax) == false && hasFax == false)
                {
                    seg["PER" + id1] = "FX";
                    seg["PER" + id2] = fax;
                    hasFax = true;
                    continue;
                }
            }

            segmentCount++;
            return seg;
        }

        public static Segment PRV(string code, string qualifier, string identification)
        {
            Segment seg = new Segment("PRV");
            seg["PRV01"] = code;
            seg["PRV02"] = qualifier;
            seg["PRV03"] = identification;

            segmentCount++;
            return seg;
        }

        // WIP
        public static Segment PS1()
        {
            Segment seg = new Segment("PS1");

            return seg;
        }

        // WIP
        public static Segment PWK()
        {
            Segment seg = new Segment("PWK");

            return seg;
        }

        // WIP
        public static Segment QTY()
        {
            Segment seg = new Segment("QTY");

            return seg;
        }

        public static Segment REF(string qualifier, string identification)
        {
            Segment seg = new Segment("REF");
            seg["REF01"] = qualifier;
            seg["REF02"] = identification;

            segmentCount++;
            return seg;
        }

        // WIP
        public static Segment SBR(
        )
        {
            Segment seg = new Segment("SBR");
            seg["SBR01"] = "P";
            seg["SBR01"] = "P";


            return seg;
        }

        public static Segment SE(string controlNumber)
        {
            Segment seg = new Segment("SE");
            seg["SE01"] = segmentCount.ToString();
            seg["SE02"] = controlNumber;

            return seg;
        }

        public static Segment ST(string controlNumber)
        {
            Segment seg = new Segment("ST");
            seg["ST01"] = "837";
            seg["ST02"] = controlNumber;
            seg["ST03"] = "005010X222A1";

            segmentCount++;
            return seg;
        }

        // WIP
        public static Segment SV1()
        {
            Segment seg = new Segment("SV1");

            return seg;
        }

        // WIP
        public static Segment SV5()
        {
            Segment seg = new Segment("SV5");

            return seg;
        }

        // WIP
        public static Segment SVD()
        {
            Segment seg = new Segment("SVD");

            return seg;
        }

        #endregion
    }
}
