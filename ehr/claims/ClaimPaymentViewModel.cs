using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class ClaimPaymentViewModel
    {
        Claim c;

        public ClaimPaymentViewModel(Claim claim)
        {
            c = claim;
        }

        public int Id
        {
            get => c.Id;
        }

        public int PaymentId
        {
            get => c.Payment == null ? -1 : c.Payment.Id;
        }

        public string DateOfService
        {
            get => c.DateOfService.Value.ToString("M/d/yyyy");
        }

        public string AmountTotal
        {
            get => String.Format("{0:n}", c.AmountTotal);
        }

        public string AmountBalance
        {
            get => String.Format("{0:n}", c.AmountBalance);
        }
    }
}