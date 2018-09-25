using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.ansi5010
{
    static class X837Config
    {
        static X837Config()
        {
            Items = new Dictionary<string, string>();
            SeparateNewLine = false;
            RetrieveConfig();
        }

        public static bool SeparateNewLine
        {
            get;
            set;
        }
            
        public static Dictionary<string, string> Items
        {
            get;
            set;
        }

        public static void RetrieveConfig()
        {
            Items["SUBMITTER_NAME"] = "HEALTH CARE RESOURCES";
            Items["RECEIVER_NAME"] = "HEALTH CARE RESOURCES";

            Items["ISA01"] = "00";
            Items["ISA02"] = "";
            Items["ISA03"] = "00";
            Items["ISA04"] = "";
            Items["ISA05"] = "ZZ";
            Items["ISA06"] = "491776";
            Items["ISA07"] = "ZZ";
            Items["ISA08"] = "";
            Items["ISA11"] = "^";
            Items["ISA12"] = "00501";
            Items["ISA14"] = "0";
            Items["ISA15"] = "T";
            Items["ISA16"] = ":";

            Items["GS01"] = "XX";
            Items["GS02"] = "491776";
            Items["GS03"] = "";
            Items["GS07"] = "X";
            Items["GS08"] = "005010X222A1";

            Items["BHT02"] = "00";
            Items["BHT06"] = "CH";

            Items["1000A_NM102"] = "2";
            Items["1000A_NM103"] = "HEALTH CARE RESOURCES, INC";
            Items["1000A_NM104"] = String.Empty;
            Items["1000A_NM105"] = String.Empty;
            Items["1000A_NM108"] = "46";
            Items["1000A_NM109"] = "SUBMITTER_ID";
            
            Items["1000A_PER02"] = "SUBMITTER_ID";
            Items["1000A_PER03"] = "TE";
            Items["1000A_PER04"] = "2018140700";
            Items["1000A_PER05"] = String.Empty;// "EM";
            Items["1000A_PER06"] = String.Empty;// "marlonsvillarama@gmail.com";
            Items["1000A_PER07"] = String.Empty;// "FX";
            Items["1000A_PER08"] = String.Empty;// "1234567890";

            Items["2000A_PRV03"] = String.Empty; // "0000XX3790";

            Items["2010AA_ISPERSON"] = "F";
            Items["2010AA_NM103"] = "2010AA_NM103";
            Items["2010AA_NM104"] = "2010AA_NM104";
            Items["2010AA_NM105"] = "2010AA_NM105";
            Items["2010AA_NM107"] = "2010AA_NM107";
            Items["2010AA_NM108"] = "XX";

            Items["2010AA_N301"] = "570 kinderkamack rd";
            Items["2010AA_N302"] = "BRGY BUNGAD";
            Items["2010AA_N401"] = "QUEZON CITY";
            Items["2010AA_N402"] = "MM";
            Items["2010AA_N403"] = "12345";
            Items["2010AA_N404"] = "PH";
            Items["2010AA_N407"] = "2010AA_NM407";

            Items["2010AA_REF01"] = "EI";
            Items["2010AA_REF02"] = "123456789";
            Items["2010AA_REF01_UPIN"] = "0B";
            Items["2010AA_REF02_UPIN"] = "A12345";

            Items["2010AA_PER02"] = "SIMON RYOO";
            Items["2010AA_PER03"] = "TE";
            Items["2010AA_PER04"] = "6098789232";
            Items["2010AA_PER05"] = "EM";
            Items["2010AA_PER06"] = "MARLONSVILLARAMA@GMAIL.COM";

            Items["2010AB_NM102"] = "2";
            Items["2010AB_NM103"] = "2010AB_NM103";
            Items["2010AB_NM108"] = "PI";
            Items["2010AB_NM109"] = "2010AB_NM109";

            Items["2010AC_NM103"] = "2010AC_NM103";
            Items["2010AC_NM108"] = "PI";
            Items["2010AC_NM109"] = "989898";
            Items["2010AC_N301"] = "MEM 3";
            Items["2010AC_N302"] = "G ENRIQUEZ ST";
            Items["2010AA_REF01"] = "2U";
            Items["2010AA_REF02"] = "123456789";
            Items["2010AC_REF_Tax"] = "2010AC_REF_Tax";
        }


    }
}