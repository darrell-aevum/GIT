using HomesteadViewer.DAL;
using HomesteadViewer.SiteUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
namespace HomesteadViewer.Models
{
    [Table("Exemptions")]
    public class MasterExemption_DTO
    {
        #region Properties
        [Key]
        public int OnlineExemptionID { get; set; }
        public string QuickrefID { get; set; }
        public int PropertyID { get; set; }
        public string ApplicantName { get; set; }
        public Int16? ExYearRequested { get; set; }
        public string ApplicantMailing1 { get; set; }
        public string ApplicantMailing2 { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantZip { get; set; }
        public string DriverLicense { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime? SpouseBirthdate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DateFirstOccupied { get; set; }
        public string Esignature { get; set; }
        public int AdHocTaxYear { get; set; }
        public string PartyName_Orion { get; set; }
        public decimal Ownership_Orion { get; set; }
        public string ActiveEx { get; set; }
        public DateTime? ExEffectiveDate { get; set; }
        public DateTime? DeedDate { get; set; }
        public string MailingAddress_Orion { get; set; }
        public bool? HSImproved { get; set; }
        public bool? LandHSEligible { get; set; }
        public bool? ImpHSEligible { get; set; }
        public decimal? PercentComplete { get; set; }
        public string SellerName { get; set; }
        public string Situs { get; set; }
        public string Legal { get; set; }
        public DateTime? TimeStampCreate { get; set; }
        public bool? HS { get; set; }
        public bool? DP { get; set; }
        public bool? OA { get; set; }
        public bool? OAS { get; set; }
        public bool? DVX { get; set; }
        public bool? DVXS { get; set; }
        public bool? DV { get; set; }
        public bool? CDV { get; set; }
        public bool? CDVS { get; set; }
        public bool? DVXS2 { get; set; }
        public bool? TCTC { get; set; }
        public bool? DLEXFACILITY { get; set; }
        public string FACILITY { get; set; }
        public bool? CERTFORPART { get; set; }
        public bool? AF { get; set; }        
        public bool? EXEMPT521 { get; set; }
        public DateTime Created { get; set; }
        

        #endregion Properties

        #region Constructor
        public MasterExemption_DTO()
        {

        }
        #endregion Constructor

        #region DTO Helpers
        public static int Count()
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    return context.MasterExemptions.Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int Count(Func<MasterExemption_DTO, bool> func)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    return context.MasterExemptions.Count(func);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        
        public static int Count(DateTime createdDate)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    return context.MasterExemptions.Count(me=> DbFunctions.TruncateTime(me.Created) == createdDate.Date);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static bool Exists(int onlineExemptionId)
        {
            using (var context = new MasterHSContext())
            {
                return context.MasterExemptions.Any(me => me.OnlineExemptionID == onlineExemptionId);
            }
        }
        public static bool Exists(Func<MasterExemption_DTO, bool> func)
        {
            using (var context = new MasterHSContext())
            {
                return context.MasterExemptions.Any(func);
            }
        }
 
        public static List<MasterExemption_DTO> GetAll(Func<MasterExemption_DTO, bool> func = null)
        {
            try
            {
                using (var context = new MasterHSContext())
                {             
                    if(func != null)
                        return context.MasterExemptions.Where(func).ToList();
                    return context.MasterExemptions.ToList();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public static  MasterExemption_DTO GetMasterExemption_For(int onlinePropertyId)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    return context.MasterExemptions.FirstOrDefault(qe=>qe.OnlineExemptionID == onlinePropertyId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<MasterExemption_DTO> GetMasterExemptionsList_For(string quickRefId)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    return context.MasterExemptions.Where(qe => qe.QuickrefID == quickRefId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<MasterExemption_DTO> GetMasterExemptionsList_For(DateTime createdDate)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    return context.MasterExemptions.Where(qe => qe.Created.Date == createdDate.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<MasterExemption_DTO> GetMasterExemption_For(AdministrativeProperties_DTO administrativProperties)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    return context.MasterExemptions.Where(qe=>qe.OnlineExemptionID == administrativProperties.OnlineExemptionID).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<MasterExemption_DTO> GetMasterExemptionsList_For(List<AdministrativeProperties_DTO> administraiveProperties_collcetion)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    var mes = new List<MasterExemption_DTO>();
                    foreach (var ap in administraiveProperties_collcetion)
                    {
                        var me = context.MasterExemptions.FirstOrDefault(m => m.OnlineExemptionID == ap.OnlineExemptionID);
                        if(me != null && me.QuickrefID == ap.QuickrefID)
                            mes.Add(me);
                        else
                        {
                            var ex = new Exception("An Orphaned Item exists in the database.")
                                .Log("ID = " + ap.ID + "\n OnlineExemptionId = " + ap.OnlineExemptionID);
                        }
                    }
                    return mes;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<MasterExemption_DTO> GetMasterExemptionsList_Exclude(List<AdministrativeProperties_DTO> administraiveProperties_collcetion)
        {
            try
            {
                using (var context = new MasterHSContext())
                {
                    var mes =  context.MasterExemptions.ToList();
                    foreach (var ap in administraiveProperties_collcetion)
                        mes.RemoveAll(me => me.OnlineExemptionID == ap.OnlineExemptionID);
                    return mes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion DTO Helpers
    }    
}