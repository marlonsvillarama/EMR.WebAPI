using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class PaymentViewModel
    {
        Payment p;

        public PaymentViewModel(Payment payment)
        {
            p = payment;
        }

        public int Id
        {
            get => p.Id;
        }

        public string AmountTotal
        {
            get => String.Format("{0:n}", p.AmountTotal);
        }
    }
}