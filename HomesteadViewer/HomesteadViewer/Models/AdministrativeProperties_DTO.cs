using HomesteadViewer.DAL;
using HomesteadViewer.Lists;
using HomesteadViewer.SiteUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace HomesteadViewer.Models
{
    [Table("AdministrativeProperties")]
    public class AdministrativeProperties_DTO
    {
        public int ID { get; set; }

        [Index(IsUnique = true)]
        public int OnlineExemptionID { get; set; }

        public string QuickrefID { get; set; } // To insure we have the right data
        public DateTime? FollowUpDate { get; set; }
        public string Comment { get; set; }
        public ExemptionStatus Status { get; set; }
        public int? AssignedUserID { get; set; }
        //   public virtual User_DTO AssignedUser { get; set; }

        public AdministrativeProperties_DTO()
        {
            this.Status = ExemptionStatus.NotWorked;
        }

        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime? DateAssigned { get; set; }
        public DateTime? DateStatusChanged { get; set; }

        #region DTO Helpers
        public static int Count()
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    return context.AdministrativeProperties_Collection.Count();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int Count(Func<AdministrativeProperties_DTO, bool> func)
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    return context.AdministrativeProperties_Collection.Count(func);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static AdministrativeProperties_DTO Get(int onlineExemptionId)
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    return context.AdministrativeProperties_Collection
                        .FirstOrDefault(x => x.OnlineExemptionID == onlineExemptionId);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static AdministrativeProperties_DTO Get(Func<AdministrativeProperties_DTO, bool> func)
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    return context.AdministrativeProperties_Collection
                        .FirstOrDefault(func);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<AdministrativeProperties_DTO> GetAll(Func<AdministrativeProperties_DTO, bool> func = null)
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    if (func != null)
                        return context.AdministrativeProperties_Collection.Where(func).ToList();
                    return context.AdministrativeProperties_Collection.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<AdministrativeProperties_DTO> GetOrphanedData()
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    var ap_c = context.AdministrativeProperties_Collection.ToList();

                    //Check if object exists in Master Exemptions

                    var orphans = ap_c.Where(ap => !MasterExemption_DTO.Exists(ap.OnlineExemptionID)).ToList();
                    //Check if Quick Ref ID's Work

                    //orphans.AddRange(ap_c.Where(ap => MasterExemption_DTO.Exists(x => x.OnlineExemptionID == ap.OnlineExemptionID && x.QuickrefID != ap.QuickrefID)).ToList());
                    return orphans;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static AdministrativeProperties_DTO AddOrUpdate(AdministrativeProperties_DTO administrativeProperties)
        {
            try
            {
                if (administrativeProperties.AssignedUserID != null)
                    administrativeProperties.AssignedUserID = (administrativeProperties.AssignedUserID <= 0) ? null : administrativeProperties.AssignedUserID;

                var ap = AdministrativeProperties_DTO.Get(administrativeProperties.OnlineExemptionID);
                //Creaet New
                if (ap == null)
                {
                    using (var context = new HomesteadViewerContext())
                    {
                        var newAp = new AdministrativeProperties_DTO()
                        {
                            OnlineExemptionID = administrativeProperties.OnlineExemptionID,
                            Comment = administrativeProperties.Comment,
                            FollowUpDate = administrativeProperties.FollowUpDate,
                            Status = administrativeProperties.Status,
                            QuickrefID = administrativeProperties.QuickrefID,
                            AssignedUserID = administrativeProperties.AssignedUserID
                        };

                        if (newAp.Status != ExemptionStatus.NotWorked)
                            newAp.DateStatusChanged = DateTime.Now;

                        if (administrativeProperties.AssignedUserID != null)
                            newAp.DateAssigned = DateTime.Now;

                        context.AdministrativeProperties_Collection.Add(newAp);
                        context.SaveChanges();

                        return LogChange(administrativeProperties.OnlineExemptionID);
                    }
                }

                //Update
                using (var context = new HomesteadViewerContext())
                {
                    ap.Comment = administrativeProperties.Comment;
                    ap.FollowUpDate = administrativeProperties.FollowUpDate;
                    ap.QuickrefID = administrativeProperties.QuickrefID;

                    if (ap.Status != administrativeProperties.Status)
                        ap.DateStatusChanged = DateTime.Now;
                    ap.Status = administrativeProperties.Status;

                    if (ap.AssignedUserID != administrativeProperties.AssignedUserID)
                        ap.DateAssigned = DateTime.Now;

                    ap.AssignedUserID = administrativeProperties.AssignedUserID;

                    context.AdministrativeProperties_Collection.Attach(ap);
                    context.Entry(ap).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return LogChange(administrativeProperties.OnlineExemptionID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static AdministrativeProperties_DTO LogChange(int onlineExemptionID)
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    var ap = AdministrativeProperties_DTO.Get(onlineExemptionID);
                    if (ap == null)
                        return null;


                    ap.Modified = DateTime.Now;
                    ap.ModifiedBy = UserSession.GetCurrentUser().FullName;

                    context.AdministrativeProperties_Collection.Attach(ap);
                    context.Entry(ap).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return AdministrativeProperties_DTO.Get(onlineExemptionID);
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