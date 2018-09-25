using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class ClaimViewModel
    {
        Claim c;

        public ClaimViewModel(Claim claim)
        {
            c = claim;
        }
        
        public int Id
        {
            get => c.Id;
        }

        public string FirstName
        {
            get => c.PrimarySubscriber.FirstName;
        }

        public string LastName
        {
            get => c.PrimarySubscriber.LastName;
        }

        public string SubscriberName
        {
            get => c.PrimarySubscriber.LastName + ", " + c.PrimarySubscriber.FirstName;
        }

        public string PrimaryPayerName
        {
            get => c.PrimaryPayer.Name;
        }

        public string PrimaryMemberId
        {
            get => c.PrimaryPayerMemberID;
        }

        public string DateOfService
        {
            get => c.DateOfService.Value.ToString("M/d/yyyy");
        }

        public string Diagnosis
        {
            get => c.DiagnosisCodes;
        }

        public string Provider
        {
            get => String.Format("{0}, {1}", c.RenderingProvider.LastName, c.RenderingProvider.FirstName);
        }

        public string Facility
        {
            get => c.Facility.Name;
        }

        public string Group
        {
            get => c.BillingProvider.LastName;
        }

        public string AmountTotal
        {
            get => String.Format("{0:n}", c.AmountTotal);
        }

        public int PrimarySubscriberId
        {
            get => c.PrimarySubscriber == null ? -1 : c.PrimarySubscriberId.Value;
        }
    }
}