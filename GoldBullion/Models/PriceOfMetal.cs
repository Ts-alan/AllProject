//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GoldBullion.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PriceOfMetal
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public double Value { get; set; }
        public int ListOfMetaL_Id { get; set; }
    
        public virtual ListOfMetal ListOfMetal { get; set; }
    }
}
