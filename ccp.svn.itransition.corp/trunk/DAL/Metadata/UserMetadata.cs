using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCP.DAL.Attributes;
using CCP.DAL.Resources;

namespace CCP.DAL.Metadata
{
    public class UserMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "StringLenth", MinimumLength = 3)]  
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "StringLenth", MinimumLength = 3)]  
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "Required")]
        [StringLength(35, ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "StringLenth", MinimumLength = 3)]
        [EmailAddress]
        //[UniqueEmail]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof (Res), ErrorMessageResourceName = "RoleRequired")]
        public long? RoleId { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Res), ErrorMessageResourceName = "Required")]
        //public long? InternalUserId { get; set; }
    }
}
