using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.WebUI.Entities
{
    public class Lifecycle
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Opened { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? Distributed { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? Processing { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? Checking { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? Closed { get; set; }
    }

    public enum ProposalStatus
    {
        Opened=1,
        Distributed=2,
        Processing=3,
        Checking=4,
        Closed=5
    }

    public enum ProposalPriority
    {
        Low=1,
        Medium=2,
        High=3,
        Critical=4
    }
}
