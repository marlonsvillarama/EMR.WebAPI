using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class ClaimHistoryViewModel
    {
        Claim c;

        public ClaimHistoryViewModel(Claim claim)
        {
            c = claim;
        }

        public int Id
        {
            get => c.Id;
        }

        public string DateOfService
        {
            get => c.DateOfService.Value.ToString("M/d/yyyy");
        }

        public string AmountTotal
        {
            get => String.Format("{0:n}", c.AmountTotal);
        }

        public string AmountPayment
        {
            get
            {
                decimal n = 0;
                if (c.Payment != null)
                {
                    n = c.Payment.AmountPayment +
                        c.Payment.AmountCopay +
                        c.Payment.AmountDeductible;
                }

                return String.Format("{0:n}", n);
            }
        }

        public string AmountBalance
        {
            get => String.Format("{0:n}", c.Payment == null ? c.AmountTotal :
                c.AmountTotal - c.Payment.AmountPayment - c.Payment.AmountCopay - c.Payment.AmountDeductible);
        }
    }
}