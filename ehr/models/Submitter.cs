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
    
    public partial class Submitter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Submitter()
        {
            this.LastName = "\'\'";
            this.FirstName = "\'\'";
            this.MiddleName = "\'\'";
            this.OrgName = "\'\'";
            this.Identifier = "\'\'";
            this.Phone = "\'\'";
            this.Email = "\'\'";
            this.Fax = "\'\'";
            this.ContactName = "\'\'";
        }
    
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string OrgName { get; set; }
        public string Identifier { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string ContactName { get; set; }
    }
}
