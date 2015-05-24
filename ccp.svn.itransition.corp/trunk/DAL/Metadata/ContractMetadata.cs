using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CCP.DAL.Attributes;
using CCP.DAL.Resources;

namespace CCP.DAL.Metadata
{
    public class ContractMetadata
    {
        [Required]
        [StringLength(500, ErrorMessageResourceType = typeof (Res), ErrorMessageResourceName = "StringLenth", MinimumLength = 3)]   
        public string Summary { get; set; }

        [Required]
        [Format]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof (Res), ErrorMessageResourceName = "Range")]
        public int? TDGA { get; set; }

        [Required]
        [Format]
        [Display(Name = "Start Date")]
        // [DateRange(ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "Range")]
        public DateTime? StartDate { get; set; }

        [Required]
        [Format]
        [Display(Name = "End Date")]
       // [DateRange(ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "Range")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Sales Person")]
        public Nullable<long> SalesPersonId { get; set; }

    }
}
