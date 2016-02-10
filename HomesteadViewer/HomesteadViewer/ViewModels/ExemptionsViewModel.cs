using HomesteadViewer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System;
using AutoMapper;
using HomesteadViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using HomesteadViewer.Models;
using System.Linq;
using System.Collections.Generic;
using HomesteadViewer.DAL;
using HomesteadViewer.Lists;

namespace HomesteadViewer.ViewModels
{

    [NotMapped]
    public class Exemption : Models.MasterExemption_DTO
    {
        [UIHint("UserList")]
        public User_DTO AssignedUser {get;set;}          
        public int AssignedUserID {get;set;}
        [UIHint("Status")]
        public ExemptionStatus Status {get;set;}
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Comment { get; set; }  
        public List<string> ExemptionTypes { get; set; }
        public DateTime? DateAssigned { get; set; }
        public DateTime? DateStatusChanged { get; set; }

        public int? ExemptionYear { get; set; }
        public Exemption()
        {
        }

        public Exemption(Models.MasterExemption_DTO masterExemption, AdministrativeProperties_DTO administrationProperties)
        {
            try
            {
                Mapper.CreateMap<MasterExemption_DTO, Exemption>();
                Mapper.Map(masterExemption, this);

                if (administrationProperties == null)
                    administrationProperties = new Models.AdministrativeProperties_DTO();

                Mapper.CreateMap<AdministrativeProperties_DTO, Exemption>()
                    .ForMember(dest => dest.OnlineExemptionID, opt => opt.Ignore())
                    .ForMember(dest => dest.QuickrefID, opt => opt.Ignore());
                
                Mapper.Map(administrationProperties, this);

                if (this.AssignedUserID != null)
                    this.AssignedUser = User_DTO.Get(this.AssignedUserID);
                
                this.ExemptionTypes = new List<string>();

                if (this.AF != null && this.AF == true)
                    this.ExemptionTypes.Add("AF");
                if (this.CDV != null && this.CDV == true)
                    this.ExemptionTypes.Add("CDV");
                if (this.CDVS != null && this.CDVS == true)
                    this.ExemptionTypes.Add("CDVS");
                if (this.CERTFORPART != null && this.CERTFORPART == true)
                    this.ExemptionTypes.Add("CERTFORPART");
                if (this.DP != null && this.DP == true)
                    this.ExemptionTypes.Add("DP");
                if (this.DV != null && this.DV == true)
                    this.ExemptionTypes.Add("DV");
                if (this.DVX != null && this.DVX == true)
                    this.ExemptionTypes.Add("DVX");
                if (this.DVXS != null && this.DVXS == true)
                    this.ExemptionTypes.Add("DVXS");
                if (this.DVXS2 != null && this.DVXS2 == true)
                    this.ExemptionTypes.Add("DVXS2");
                if (this.EXEMPT521 != null && this.EXEMPT521 == true)
                    this.ExemptionTypes.Add("EXEMPT521");
                if (this.HS != null && this.HS == true)
                    this.ExemptionTypes.Add("HS");
                if (this.OA != null && this.OA == true)
                    this.ExemptionTypes.Add("OA");
                if (this.OAS != null && this.OAS == true)
                    this.ExemptionTypes.Add("OAS");
                if (this.TCTC != null && this.TCTC == true)
                    this.ExemptionTypes.Add("TCTC");

                if (this.ExEffectiveDate != null)
                {
                    this.ExemptionYear = this.ExEffectiveDate.Value.Year;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Exemption> Load(string modifier)
        {
            if (string.IsNullOrEmpty(modifier))
                modifier = "all";

            switch (modifier.ToLower())
            {
                case "unassigned":
                    return GetUnassignedExemptions()  ?? new List<Exemption>();
                case "assigned":
                    return GetAssignedExemptions() ?? new List<Exemption>();
                case "approved":
                    return GetExemptionsFor(ExemptionStatus.Approved) ?? new List<Exemption>();
                case "pending":
                    return GetExemptionsFor(ExemptionStatus.Pending) ?? new List<Exemption>();
                case "notworked":
                    return GetExemptionsFor(ExemptionStatus.NotWorked)  ?? new List<Exemption>();
                case "all":
                    return GetAllExemptions() ?? new List<Exemption>();
                default:
                    int id = -1;
                    //Assigned to User
                    if (int.TryParse(modifier, out id))
                    {
                        var user = User_DTO.Get(id);
                        if (user == null)
                            return new List<Exemption>(); ;
                        return GetExemptionsFor(user) ?? new List<Exemption>();
                    }
                    else
                    {
                        //Return all
                        return GetExemptionsFor(modifier);
                    }                    
            }
        }

        public static int GetCountOfAssigned()
        {
            return AdministrativeProperties_DTO.Count(x=>x.AssignedUserID != null && x.Status != ExemptionStatus.Approved);
        }
        public static int GetCountOfUnassigned()
        {
            int me = MasterExemption_DTO.Count();
            int assignedOrApproved = AdministrativeProperties_DTO.Count(x => x.AssignedUserID != null || x.Status == ExemptionStatus.Approved);
            return me - assignedOrApproved;
        }
        public static int GetCountFor(ExemptionStatus status)
        {
            switch (status)
            {
                case ExemptionStatus.NotWorked:                    
                    int approvePending = AdministrativeProperties_DTO.Count(x=>x.Status == ExemptionStatus.Approved) + AdministrativeProperties_DTO.Count(x=>x.Status  == ExemptionStatus.Pending);
                    int me = MasterExemption_DTO.Count();
                    return me - approvePending;
                default:
                    return AdministrativeProperties_DTO.Count(x=>x.Status == status);
            }
        }
        public static List<Exemption> GetExemptionsFor(User_DTO user)
        {
            try
            {
                var apc = user.AssignedExemptions;
                apc.RemoveAll(x => x.Status == ExemptionStatus.Approved);
                var mec = MasterExemption_DTO.GetMasterExemptionsList_For(apc);
                return mec.Select(x => new Exemption(x, apc.FirstOrDefault(a => a.OnlineExemptionID == x.OnlineExemptionID))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Exemption> GetExemptionsFor(ExemptionStatus status)
        {
            try
            {
                switch (status)
                {
                    case ExemptionStatus.NotWorked:
                        var notworked = AdministrativeProperties_DTO.GetAll(x=>x.Status == status);
                        var worked = AdministrativeProperties_DTO.GetAll(x=>x.Status == ExemptionStatus.Approved);
                            worked.AddRange(AdministrativeProperties_DTO.GetAll(x=>x.Status == ExemptionStatus.Pending));
                        var m = MasterExemption_DTO.GetMasterExemptionsList_Exclude(worked);
                        return m.Select(x => new Exemption(x, notworked.FirstOrDefault(a => a.OnlineExemptionID == x.OnlineExemptionID))).ToList();
                    default:
                        var apc = AdministrativeProperties_DTO.GetAll(x=>x.Status == status);
                        var mec = MasterExemption_DTO.GetMasterExemptionsList_For(apc);
                        return mec.Select(x => new Exemption(x, apc.FirstOrDefault(a => a.OnlineExemptionID == x.OnlineExemptionID))).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Exemption> GetExemptionsFor(string quickRefId)
        {
            try
            {
                var apc = new List<AdministrativeProperties_DTO>();
                var mec = MasterExemption_DTO.GetMasterExemptionsList_For(quickRefId).Distinct();

                foreach (var me in mec)
                {
                   var ap = AdministrativeProperties_DTO.Get(me.OnlineExemptionID);
                   if (ap != null)
                       apc.Add(ap);
                }
                return mec.Select(x => new Exemption(x, apc.FirstOrDefault(a => a.OnlineExemptionID == x.OnlineExemptionID))).ToList();              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        public static List<Exemption> GetAssignedExemptions()
        {
            try
            {
                var apc = AdministrativeProperties_DTO.GetAll(x=>x.AssignedUserID != null);
                apc.RemoveAll(x => x.Status == ExemptionStatus.Approved);
                var mec = MasterExemption_DTO.GetMasterExemptionsList_For(apc);
                return mec.Select(x => new Exemption(x, apc.FirstOrDefault(a => a.OnlineExemptionID == x.OnlineExemptionID))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Exemption> GetUnassignedExemptions()
        {
            try
            {
                var assigned = AdministrativeProperties_DTO.GetAll(x=>x.AssignedUserID != null || x.Status == ExemptionStatus.Approved);
                var unassigned = AdministrativeProperties_DTO.GetAll(x => x.AssignedUserID == null && x.Status != ExemptionStatus.Approved);

                var mes = MasterExemption_DTO.GetAll();
                mes.RemoveAll(x => assigned.Exists(y => y.OnlineExemptionID == x.OnlineExemptionID));

                return mes.Select(x => new Exemption(x, unassigned.FirstOrDefault(a => a.OnlineExemptionID == x.OnlineExemptionID))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Exemption> GetAllExemptions()
        {
            try
            {
                var apc = AdministrativeProperties_DTO.GetAll();
                var mec = MasterExemption_DTO.GetAll();

                return mec.Select(x => new Exemption(x, apc.FirstOrDefault(a => a.OnlineExemptionID == x.OnlineExemptionID))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}