using HomesteadViewer.Lists;
using HomesteadViewer.Models;
using HomesteadViewer.SiteUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
namespace HomesteadViewer.ViewModels
{

    [NotMapped]
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {

        }
        public static List<ChartDateStat> AssignedByDate(int days = 0)
        {
            var data = new List<ChartDateStat>();
            try
            {
                if (days > 0)
                    days = -days;

                List<AdministrativeProperties_DTO> apList = new List<AdministrativeProperties_DTO>();
                if (days == 0)
                    apList = AdministrativeProperties_DTO.GetAll(x => x.AssignedUserID != null && x.DateAssigned != null);
                else
                    apList = AdministrativeProperties_DTO.GetAll(x => x.AssignedUserID != null && x.DateAssigned != null && x.DateAssigned.Value.Date > DateTime.Now.AddDays(days));

                data = apList
                    .GroupBy(x => x.DateAssigned.Value.Date)
                    .Select(x => x.First())
                    .Select(x => new ChartDateStat()
                    {
                        Id = x.DateAssigned.Value.Date,   
                        Total = AdministrativeProperties_DTO.Count(
                            y => y.DateAssigned != null && y.DateAssigned.Value.Date == x.DateAssigned.Value.Date)
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "AssignedByDate");
            }
            return data;
        }
        public static List<ListDateItem> PendingExemptions()
        {
            var data = new List<ListDateItem>();
            foreach (var pe in AdministrativeProperties_DTO.GetAll(x => x.Status == ExemptionStatus.Pending))
            {
                try
                {
                    if (pe.OnlineExemptionID == null)
                        continue;
                    var me = MasterExemption_DTO.GetMasterExemption_For(pe.OnlineExemptionID);
                    if (me == null)
                        continue;
                    data.Add(new ListDateItem()
                    {
                        Id = pe.OnlineExemptionID,
                        Title = me.QuickrefID,
                        Date = (pe.DateStatusChanged != null) ? pe.DateStatusChanged.Value : pe.Modified.Value
                    });
                }
                catch (Exception ex)
                {
                    ex.Log("DashboardViewModel", "PendingExemptions");
                }
            }
            return data.OrderBy(x=>x.Date).ToList();
        }
        public static List<ChartStat> AssignedToUsers()
        {
            var data = new List<ChartStat>();
            try
            {
                data = AdministrativeProperties_DTO.GetAll(x => x.AssignedUserID != null)
                       .GroupBy(x => x.AssignedUserID)
                       .Select(x => x.First())
                       .Select(x => new ChartStat()
                       {
                           Id = (User_DTO.Get((int)x.AssignedUserID) != null) ? User_DTO.Get((int)x.AssignedUserID).FullName : "",
                           Total = AdministrativeProperties_DTO.Count(y => y.AssignedUserID == x.AssignedUserID)
                       })
                       .ToList();
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "AssignedToUsers");
            }
            return data;
        }
        public static List<ChartStat> ApprovedByUsers()
        {
            var data = new List<ChartStat>();
            try
            {
                var approved = AdministrativeProperties_DTO.GetAll(x => x.Status == ExemptionStatus.Approved);
                foreach (var userExemptions in approved.GroupBy(x => x.AssignedUserID))
                {
                    var chartstat = new ChartStat();
                    if (userExemptions.First().AssignedUserID != null)
                    {
                        chartstat.Id = User_DTO.Get((int)userExemptions.First().AssignedUserID).FullName;
                        chartstat.Total = AdministrativeProperties_DTO.Count(y => y.Status == ExemptionStatus.Approved && y.AssignedUserID == (int)userExemptions.First().AssignedUserID);
                    }
                    else
                    {
                        chartstat.Id = "Not Assigned";
                        chartstat.Total = AdministrativeProperties_DTO.Count(y => y.Status == ExemptionStatus.Approved && (y.AssignedUserID == null || y.AssignedUserID == 0));
                    }
                    data.Add(chartstat);
                }
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "AssignedToUsers");
            }
            return data;
        }

        public static List<ChartDateStat> ChartDateStat(ExemptionStatus status, int days)
        {
            var data = new List<ChartDateStat>();
            try
            {
                List<AdministrativeProperties_DTO> apList = new List<AdministrativeProperties_DTO>();
                data = AdministrativeProperties_DTO.GetAll(x => x.Status == status && x.DateStatusChanged != null && x.DateStatusChanged.Value.Date > DateTime.Now.AddDays(-15))
                    .GroupBy(x => x.DateStatusChanged.Value.Date)
                    .Select(x => x.First())
                    .Select(x => new ChartDateStat()
                    {
                        Id = x.DateStatusChanged.Value.Date,
                        Total = AdministrativeProperties_DTO.Count(y =>
                            y.Status == ExemptionStatus.Pending &&
                            y.DateStatusChanged != null &&
                            y.DateStatusChanged.Value.Date == x.DateStatusChanged.Value.Date)
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "PendingByDate");
            }
            return data;
        }
 
        public static List<ChartDateStat> PendingByDate(Func<AdministrativeProperties_DTO, bool> func = null)
        {
            var data = new List<ChartDateStat>();
            try
            {
                List<AdministrativeProperties_DTO> apList = new List<AdministrativeProperties_DTO>();
                data = AdministrativeProperties_DTO.GetAll(func)
                    .Where(x => x.Status == ExemptionStatus.Pending && x.DateStatusChanged != null)
                    .GroupBy(x => x.DateStatusChanged.Value.Date)
                    .Select(x => x.First())
                    .Select(x => new ChartDateStat()
                    {
                        Id = x.DateStatusChanged.Value.Date,
                        Total = AdministrativeProperties_DTO.Count(y => 
                            y.Status == ExemptionStatus.Pending &&
                            y.DateStatusChanged != null &&                            
                            y.DateStatusChanged.Value.Date == x.DateStatusChanged.Value.Date)
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "PendingByDate");
            }
            return data;
        }
        public static List<ChartDateStat> ApprovedByDate()
        {
            var data = new List<ChartDateStat>();
            try
            {
                data = AdministrativeProperties_DTO.GetAll(x => x.Status == ExemptionStatus.Approved && x.DateStatusChanged != null)
                    .GroupBy(x => x.DateStatusChanged.Value.Date)
                    .Select(x => x.First())
                    .Select(x => new ChartDateStat()
                    {
                        Id = x.DateStatusChanged.Value.Date,
                        Total = AdministrativeProperties_DTO.Count(y =>
                            y.Status == ExemptionStatus.Approved &&
                            y.DateStatusChanged != null &&
                            y.DateStatusChanged.Value.Date == x.DateStatusChanged.Value.Date)
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "ApprovedByDate");
            }
            return data;
        }
        public static List<ChartDateStat> CreatedByDate(Func<MasterExemption_DTO, bool> func = null)
        {
            var data = new List<ChartDateStat>();
            try
            {
                data = MasterExemption_DTO.GetAll(func)
                     .GroupBy(x => x.Created.Date)
                     .Select(x => x.First())
                     .Select(x => new ChartDateStat()
                     {
                         Id = x.Created.Date,
                         Total = MasterExemption_DTO.Count(x.Created)
                     })
                     .ToList();
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "CreatedByDate");
            }
            return data;
        }
        public static List<ChartStat> ExemptionsByStatus(bool ignoreApproved = false)
        {
            var data = new List<ChartStat>();
            try
            {
                int pendingCount = MasterExemption_DTO.Count() - AdministrativeProperties_DTO.Count() + AdministrativeProperties_DTO.Count(x => x.Status == ExemptionStatus.NotWorked);
                data.Add(new ChartStat()
                {
                    Id = "Not Worked",
                    Total = pendingCount
                });

                data.Add(new ChartStat()
                {
                    Id = "Pending",
                    Total = AdministrativeProperties_DTO.Count(x => x.Status == ExemptionStatus.Pending)
                });

                if (ignoreApproved)
                    return data.OrderBy(x => x.Total).ToList();

                data.Add(new ChartStat()
                {
                    Id = "Approved",
                    Total = AdministrativeProperties_DTO.Count(x=>x.Status == ExemptionStatus.Approved)
                });
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "ExemptionsByStatus");
            }
            return data;
        }
    }

    public class ChartStat
    {
        public string Id { get; set; }
        public int Total { get; set; }
    }

    public class ChartDateStat
    {
        public DateTime Id { get; set; }
        public int Total { get; set; }
    }

    public class ListDateItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}