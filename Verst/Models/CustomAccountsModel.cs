using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Verst.Models
{
    public class Registration
    {
        public int Counter;
        public string Im;
        public string Fam;
        public string Login;
        public string Passw;
    }

    public class LogOn
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Passw { get; set; }
    }
}