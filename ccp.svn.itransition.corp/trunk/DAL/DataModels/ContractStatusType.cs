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
    
    public partial class ContractStatusType
    {
        public ContractStatusType()
        {
            this.Contracts = new HashSet<Contract>();
            this.Messages = new HashSet<Message>();
            this.Messages1 = new HashSet<Message>();
        }
    
        public long ContractStatusId { get; set; }
        public string ContractStatusName { get; set; }
        public string ContractStatusTag { get; set; }
    
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Message> Messages1 { get; set; }
    }
}
