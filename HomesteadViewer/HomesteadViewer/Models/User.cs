using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace HomesteadViewer.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50), Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required, StringLength(250)]
        public string EmailAddress { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

        public byte[] AvatarImageFileData { get; set; }

        [StringLength(12)]
        public string Phone { get; set; }

        [StringLength(12)]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(5)]
        public string Extension { get; set; }

        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }

        [ScriptIgnore]
        public virtual ICollection<Exemption> AssignedExemptions { get; set; }

        public User()
        {
            this.AssignedExemptions = new List<Exemption>();
        }
    }
}