//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMR.WebAPI.ehr.models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccountPreference
    {
        public int Id { get; set; }
        public Nullable<int> RenderingProviderId { get; set; }
        public string BillingProviderId { get; set; }
        public string FacilityId { get; set; }
        public string PlaceOfServiceId { get; set; }
    }
}