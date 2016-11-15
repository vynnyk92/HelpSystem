using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Login is necessary.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is necessary.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Do you want to save password?")]
        public bool RememberMe { get; set; }
    }
}