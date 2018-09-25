using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.ansi5010
{
    public class BillingProviderSegment
    {
        
        Dictionary<string, string> CONFIG;
        
        public BillingProviderSegment(Provider provider)
        {
            Provider = provider;
        }

        public Provider Provider { get; set; }

        public Segment HL
        {
            get
            {
                Segment seg = new Segment("HL");

                return seg;
            }
        }

        public Segment PRV
        {
            get
            {
                Segment seg = new Segment("PRV");
                seg["PRV01"] = "BI";
                seg["PRV02"] = "PXC";
                seg["PRV03"] = Provider.TaxonomyCode.Code;

                return seg;
            }
        }

        public Segment NM1
        {
            get
            {
                Segment seg = new Segment("NM1");
                seg["NM101"] = "85";
                seg["NM102"] = "1";
                seg["NM103"] = Provider.LastName;
                seg["NM104"] = Provider.FirstName;
                seg["NM105"] = Provider.MiddleName;
                seg["NM107"] = Provider.Suffix;
                seg["NM108"] = "XX";
                seg["NM109"] = Provider.NPI;

                return seg;
            }
        }

        public Segment N3
        {
            get
            {
                return Segment.N3(Provider.Address_1, Provider.Address_2);
            }
        }

        public Segment N4
        {
            get
            {
                return Segment.N4(Provider.City, Provider.State, Provider.Zip);
            }
        }

        public Segment REF_Tax
        {
            get
            {
                Segment seg = new Segment("REF");
                seg["REF01"] = (String.IsNullOrEmpty(Provider.EIN) == false) ? "EI" : "SY";
                seg["REF02"] = (String.IsNullOrEmpty(Provider.EIN) == false) ?
                    Provider.EIN : Provider.SSN;

                return seg;
            }
        }

        public Segment REF_UPIN
        {
            get
            {
                Segment seg = new Segment("REF");
                seg["REF01"] = (String.IsNullOrEmpty(Provider.License) == false) ? "0B" : "1G";
                seg["REF02"] = (String.IsNullOrEmpty(Provider.License) == false) ?
                    Provider.License : Provider.UPIN;

                return seg;
            }
        }

        public Segment PER
        {
            get
            {
                Segment seg = new Segment("PER");
                seg["PER01"] = "IC";
                seg["PER02"] = Provider.FirstName + " " + Provider.LastName;

                for(int i=1; i <= 3; i++)
                {
                    if (String.IsNullOrEmpty(Provider.Phone_1) == false)
                    {
                        seg["PER" + ((i * 2) + 1).ToString().PadLeft(2, '0')] = "TE";
                        seg["PER" + ((i * 2) + 2).ToString().PadLeft(2, '0')] = Provider.Phone_1;
                    }
                    if (String.IsNullOrEmpty(Provider.Email) == false)
                    {
                        seg["PER" + ((i * 2) + 1).ToString().PadLeft(2, '0')] = "EM";
                        seg["PER" + ((i * 2) + 2).ToString().PadLeft(2, '0')] = Provider.Email;
                    }
                    if (String.IsNullOrEmpty(Provider.Fax) == false)
                    {
                        seg["PER" + ((i * 2) + 1).ToString().PadLeft(2, '0')] = "FX";
                        seg["PER" + ((i * 2) + 2).ToString().PadLeft(2, '0')] = Provider.Fax;
                    }
                }

                return seg;
            }
        }

        public String Output
        {
            get
            {
                return

                    // Hierarchical Level
                    HL.Output +

                    // Billing Provider Specialty Information
                    (String.IsNullOrEmpty(CONFIG["2000A_PRV03"]) == true ?
                        String.Empty :
                        PRV.Output
                    ) +

                    // Currency Information - Assume US so return blank
                    String.Empty +

                    // Billing Provider Name
                    NM1.Output;

                    // Billing Provider Contact Information
                    
                    // Billing Provider Address
                    //output += n3.Output;

                    // Billing Provider City, State, Zip Code
                    //output += n4.Output;

                    // Billing Provider Tax Identification
                    //output += refTax.Output;

                    // Billing Provider UPIN/License Information
                    /*if (String.IsNullOrEmpty(nm1.NM109) == true)
                    {
                        output += refUpin.Output;
                    }


                HL hl = new HL(hlevel)
                {
                    HL03 = "20",
                    HL04 = "1"
                };
                output += hl.Output;
                PRV prv = new PRV("BI")
                {
                    PRV02 = "PXC",
                    PRV03 = CONFIG["2000A_PRV03"]
                };
                output += (String.IsNullOrEmpty(CONFIG["2000A_PRV03"]) == true ?
                    String.Empty :
                    prv.Output
                );*/

                //return output;

            }
        }
    }
}