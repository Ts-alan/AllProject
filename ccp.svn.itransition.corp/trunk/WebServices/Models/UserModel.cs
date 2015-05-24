using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCP.DAL.DataModels;

namespace CCP.WebApi.Models
{
    public class UserModel
    {
        public User User { get; set; }
        public ApproverTier[] ApproverTiers { get; set; }
    }
}