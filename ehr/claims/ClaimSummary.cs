using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class ClaimSummary
    {
        public ClaimSummary()
        {
            Month = 1;
            Year = DateTime.Today.Year;
            TotalAmount = 0;
            TotalClaims = 0;
            AgingPeriod = "";
        }

        #region "Properties"
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.Month);
            }
        }
        public int TotalClaims { get; set; }
        public decimal TotalAmount { get; set; }
        public string AgingPeriod { get; set; }
        #endregion
    }
}