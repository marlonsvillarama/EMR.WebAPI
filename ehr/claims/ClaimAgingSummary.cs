using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class ClaimAgingSummary
    {
        public ClaimAgingSummary()
        {
            Age = "";
            TotalAmount = 0;
            ClaimCount = 0;
        }

        #region Properties
        public string Age { get; set; }
        public decimal TotalAmount { get; set; }
        public int ClaimCount { get; set; }
        #endregion
    }
}