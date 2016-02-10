using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
namespace HomesteadViewer.Models
{
    [Table("AssignedExemptions")]
    public class AssignedExemption
    {
        [Key]
        public int ID { get; set; }
        public int ExemptionID { get; set; }
        public int UserID { get; set; }
    }
}