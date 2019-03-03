using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class ClaimMonthlySummary
    {
        public ClaimMonthlySummary()
        {

        }

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
    }
}