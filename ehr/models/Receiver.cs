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
    
    public partial class Receiver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Receiver()
        {
            this.Name = "\'\'";
            this.Identifier = "\'\'";
            this.Active = true;
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
