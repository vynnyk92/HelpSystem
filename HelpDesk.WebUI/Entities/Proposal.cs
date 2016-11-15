using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.WebUI.Entities
{
    public class Proposal
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter proposal name")]
        [MaxLength(50, ErrorMessage = "Proposal name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tell me about your problems")]
        [Display(Name ="Problem description")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "Whoa! Man easy 250 char enought")]
        public string Description { get; set; }

        [MaxLength(250, ErrorMessage = "Whoa! Man easy 250 char enought")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Required(ErrorMessage ="Input Status")]
        public int Status { get; set; }

        public int Priority { get; set; }

        public int? ActivId { get; set; }
        public Activ Activ { get; set; }

        [Display(Name ="Error file")]
        public string File { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? ExecutorId { get; set; }
        public User Executor { get; set; }

        public int LifecycleId { get; set; }
        public Lifecycle Lifecycle { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Input Category Name")]
        [Display(Name = "Category Name")]
        [MaxLength(50, ErrorMessage = "Whoa! Man easy 50 char enought")]
        public string Name { get; set; }
    }

    public class Activ
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Input Room Number")]
        [Display(Name = "Room Number")]
        [MaxLength(50, ErrorMessage = "Whoa! Man easy 50 char enought")]
        public string RoomNumber { get; set; }


        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
