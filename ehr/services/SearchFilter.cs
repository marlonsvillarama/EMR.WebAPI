using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.services
{
    public class SearchFilter
    {
        public SearchFilter(string parms)
        {
            Params = parms;
            Values = Params.Split(new[] { '|' });
            PrepareFilters();
        }

        private void PrepareFilters()
        {
            string s = "";
            int n = 0;

            #region Type
            Type = Values.Length > 0 ? Values[0] : "";
            #endregion

            #region Page Number
            s = Values.Length > 1 ? Values[1] : "";

            if (string.IsNullOrEmpty(s) == false)
            {
                if (int.TryParse(s, out n) == false)
                {
                    n = 0;
                }
            }
            PageNumber = n;
            #endregion

            #region PageSize
            s = Values.Length > 2 ? Values[2] : "";

            if (string.IsNullOrEmpty(s) == false)
            {
                if (int.TryParse(s, out n) == false)
                {
                    n = 10;
                }
            }
            PageSize = n;
            #endregion

            #region LastName
            LastName = Values.Length > 3 ? Values[3] : "";
            #endregion

            #region DateOfBirth
            s = Values.Length > 4 ? Values[4] : "";
            DateOfBirth = ParseDate(s);
            #endregion

            #region DateCreated
            s = Values.Length > 5 ? Values[5] : "";
            DateCreated = ParseDate(s);
            #endregion

            #region MonthStartEnd
            s = Values.Length > 6 ? Values[6] : "";
            MonthStartEnd = new List<DateTime?>();
            AgingPeriod = "";

            if (Type.ToUpper() == "CLAIMS_BYMONTH_SUMMARY")
            {
                string dtFrom, dtTo;
                int m, y, days;
                List<DateTime?> dtRange = new List<DateTime?>();

                if (string.IsNullOrEmpty(s) == true)
                {
                    MonthStartEnd = dtRange;
                }

                try
                {
                    m = s.Length >= 2 ? int.Parse(s.Substring(0, 2)) : DateTime.Today.Month;
                    y = s.Length >= 6 ? int.Parse(s.Substring(2, 4)) : DateTime.Today.Year;

                    days = DateTime.DaysInMonth(y, m);

                    // Start of month
                    dtFrom = "01/" + m + "/" + y;
                    dtRange.Add(DateTime.ParseExact(dtFrom, "dd/MM/yyyy", null));

                    // End of month
                    dtTo = days.ToString().PadLeft(2, '0') + "/" + m + "/" + y;
                    dtRange.Add(DateTime.ParseExact(dtTo, "dd/MM/yyyy", null).AddDays(1));
                }
                catch
                {
                    dtRange = new List<DateTime?>();
                }

                MonthStartEnd = dtRange;
            }
            else if (Type.ToUpper() == "CLAIMS_AGING_SUMMARY")
            {
                AgingPeriod = s.ToUpper();
            }
            #endregion

            #region Supplemental Filters
            s = Values.Length > 7 ? Values[7] : "";

            if (String.IsNullOrEmpty(s) == false)
            {
                int id = 0;
                string[] sfilters = s.Split(new[] { ',' });

                if (sfilters.Length > 0)
                {
                    id = int.Parse(sfilters[0]);
                    if (id >= 0)
                    {
                        FacilityId = id;
                    }
                }

                id = 0;
                if (sfilters.Length > 1)
                {
                    id = int.Parse(sfilters[1]);
                    if (id >= 0)
                    {
                        ProviderId = id;
                    }
                }

                if (sfilters.Length > 2)
                {
                    CPT = sfilters[2];
                }

                decimal de = 0;
                if (sfilters.Length > 3)
                {
                    Deductible = 0;
                    if (string.IsNullOrEmpty(sfilters[3]) == false)
                    {
                        decimal.TryParse(sfilters[3], out de);
                        Deductible = de;
                    }
                }

                if (sfilters.Length > 4)
                {
                    Copay = 0;
                    if (string.IsNullOrEmpty(sfilters[4]) == false)
                    {
                        decimal.TryParse(sfilters[4], out de);
                        Copay = de;
                    }
                }
            }
            #endregion
        }

        private DateTime? ParseDate(string str)
        {
            string s = "", m = "", d = "", y = "";
            DateTime? dt = null;

            if (string.IsNullOrEmpty(str) == true)
            {
                return dt;
            }

            d = str.Length >= 2 ? str.Substring(0, 2) : "";
            m = str.Length >= 4 ? str.Substring(2, 2) : "";
            y = str.Length >= 8 ? str.Substring(4, 4) : "";
            s = d + "/" + m + "/" + y;

            try
            {
                dt = DateTime.ParseExact(s, "dd/MM/yyyy", null);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        #region Properties
        public string Params { get; set; }
        public string[] Values { get; set; }
        public string Type { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateCreated { get; set; }
        public int ProviderId { get; set; }
        public int FacilityId { get; set; }
        public string CPT { get; set; }
        public decimal Deductible { get; set; }
        public decimal Copay { get; set; }
        /*
        public DateTime? DateOfService
        {
            get
            {
                string s;
                s = Values.Length > 6 ? Values[6] : "";
                return ParseDate(s);
            }
        }
        */
        public List<DateTime?> MonthStartEnd { get; set; }
        public string AgingPeriod { get; set; }
        #endregion
    }
}
