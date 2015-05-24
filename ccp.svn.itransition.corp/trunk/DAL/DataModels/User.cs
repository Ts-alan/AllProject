//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCP.DAL.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.SalesPersons = new HashSet<ApproverTier>();
            this.Approvers = new HashSet<ApproverTier>();
            this.Contracts = new HashSet<Contract>();
        }
    
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<long> RoleId { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<long> InternalUserId { get; set; }
    
        public virtual ICollection<ApproverTier> SalesPersons { get; set; }
        public virtual ICollection<ApproverTier> Approvers { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual Role Role { get; set; }
    }
}
