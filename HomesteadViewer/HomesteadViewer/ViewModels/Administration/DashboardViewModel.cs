using HomesteadViewer.Lists;
using HomesteadViewer.Models;
using HomesteadViewer.SiteUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Humanizer;

namespace HomesteadViewer.ViewModels.Administration
{

    [NotMapped]
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {

        }
        public static List<ChartTimeStat> UserActivity(Func<UserTimeLog_DTO, bool> func = null)
        {
            var data = new List<ChartTimeStat>();
            try
            {
                if (func == null)
                    func = x => x.Id != null;

                var userGroup = UserTimeLog_DTO.GetAll(func).GroupBy(x => x.UserId).ToList();

                foreach (var userTime in userGroup)
                {
                    var user = (User_DTO.Get(userTime.First().UserId) != null) ? User_DTO.Get(userTime.First().UserId).FullName : "Unknown User";
                    var dateGroup = userTime.GroupBy(x => x.FirstLogDate.Date);
                    foreach (var date in dateGroup)
                    {
                        var time = TimeSpan.FromMilliseconds(date.Sum(y => y.Duration));
                        data.Add(new ChartTimeStat()
                        {
                            Id = user,
                            date = date.First().FirstLogDate.Date,
                            Total = time.TotalHours,
                            Humanized = time.Humanize(2)
                        });
                    }
                }
                //Id = (User_DTO.Get(x.UserId) != null) ? User_DTO.Get(x.UserId).FullName : "Unknown User"
            }
            catch (Exception ex)
            {
                ex.Log("DashboardViewModel", "UserActivity");
            }
            return data;
        }
    
    }

    public class ChartTimeStat
    {
        public string Id { get; set; }
        public DateTime date {get;set;}
        public double Total { get; set; }
        public string Humanized { get; set; }
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