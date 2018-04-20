//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WikiCharity.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Charity
    {
        public string ABN { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string TownCity { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public string Website { get; set; }
        public string RegisDate { get; set; }
        public string Size { get; set; }
        public Nullable<bool> Tax { get; set; }
        public string Beneficiaries { get; set; }
        public int Id { get; set; }
        public string OtherName { get; set; }
        public Nullable<bool> BRC { get; set; }
        public Nullable<bool> ConductedActivity { get; set; }
        public string MainActivity { get; set; }
        public string Activities { get; set; }
        public string OperateStates { get; set; }
        public string Description { get; set; }
        public Nullable<bool> ABNStatus { get; set; }
        public Nullable<int> StaffFull { get; set; }
        public Nullable<int> StaffPart { get; set; }
        public Nullable<int> StaffCasual { get; set; }
        public Nullable<int> StaffVolun { get; set; }
    }
}