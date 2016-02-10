using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using AutoMapper;
using System;

namespace HomesteadViewer.Models
{
    public enum ExemptionStatus
    {
        Approved,
        Pending,
        NotWorked
    }

    [Table("Exemptions")]
    public class Exemption
    {
        [Key]
        public int ID { get; set; }
        public int QuedExemptionId { get; set; }

        [UIHint("Status")]
        public ExemptionStatus Status { get; set; }

        [ScriptIgnore]
        public virtual ICollection<User> AssignedUsers { get; set; }



        public string QuickRefID { get; set; }
        public int? Driver_License { get; set; }
        public DateTime? Birth_Date { get; set; }
        public DateTime? SpouseBirthDate { get; set; }
        public DateTime? DateFirstOccupied { get; set; }
        public string OwnsProperty { get; set; }
        public string Transfer_Tax { get; set; }
        public DateTime? DeedDate { get; set; }
        public string SellerName { get; set; }
        public string ExemptionYear { get; set; }
        public string ESignature { get; set; }
        public string OwnerName { get; set; }
        public string PartyName { get; set; }
        public string HS_CODE { get; set; }
        public DateTime? HS_Eff_Date { get; set; }
        public string HS_ETR_YR { get; set; }
        public string OA_Code { get; set; }
        public DateTime? OA_Eff_Date { get; set; }
        public string OA_ETR_YR { get; set; }
        public string OAS_ETR_YR { get; set; }
        public string DP_Code { get; set; }
        public DateTime? DP_Eff_Date { get; set; }
        public string DP_ETR_YR { get; set; }
        public string DV_Code { get; set; }
        public DateTime? DV_Eff_Date { get; set; }
        public string DV_ETR_YR { get; set; }
        public string LegalDescription { get; set; }
        public string SitusAddress { get; set; }
        public string Street_chng { get; set; }
        public string City_chng { get; set; }
        public string State_chng { get; set; }
        public string Zip_chng { get; set; }
        public string MailingAddress { get; set; }
        public string OwnerAddress1 { get; set; }
        public string OwnerAddress2 { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerState { get; set; }
        public string OwnerZip { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string fPercentComplete { get; set; }
        public string FirstOffImproved { get; set; }
        public string FirstOfcama_TSGLand_fHomesite { get; set; }
        public string FirstOfcama_TSGImp_fHomesite { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Completed { get; set; }
        public string Comment { get; set; }
        public string Search { get; set; }
        public string Assign { get; set; }
        public string Additional { get; set; }
        public string DocumentName { get; set; }
        public string DocumentTypeCode { get; set; }
        public DateTime? TimestampCreate { get; set; }
        public DateTime? Date_Added { get; set; }
 
        public Exemption()
        {
            this.AssignedUsers = new List<User>();
            this.Status = ExemptionStatus.NotWorked;
        }
    }
}