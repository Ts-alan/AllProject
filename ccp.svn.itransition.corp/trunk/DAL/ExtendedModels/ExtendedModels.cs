using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCP.DAL.Metadata;

namespace CCP.DAL.DataModels
{
    [MetadataType(typeof(UserMetadata))]
   public partial class User
    {
        private bool _isSalesPerson;

        public bool IsSalesPerson
        {
            get {
                _isSalesPerson = Approvers.Any();
                return _isSalesPerson;
            }
            set { _isSalesPerson = value; }
        }
    }
    
    [MetadataType(typeof(ContractMetadata))]
    public partial class Contract
    {
        
    }

}
