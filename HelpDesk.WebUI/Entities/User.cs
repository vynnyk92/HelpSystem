using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.WebUI.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter a Name")]
        [Display(Name ="First Name and Last Name")]
        [MaxLength(50, ErrorMessage ="Your name is out of control")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Login")]
        [MaxLength(50, ErrorMessage = "Your Login is out of control")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [MinLength(4, ErrorMessage = "Password must be longer 4 characters")]
        [MaxLength(50, ErrorMessage = "Password is too big.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(50, ErrorMessage = "Your position is too long")]
        [Required(ErrorMessage ="Position is necessary!")]
        public string Position { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Input Department Name")]
        [Display(Name = "Department Name")]
        [MaxLength(50, ErrorMessage = "Whoa! Man easy 50 char enought")]
        public string Name { get; set; }

    }

    
}
