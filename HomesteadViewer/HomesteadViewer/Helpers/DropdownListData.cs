using AutoMapper;
using HomesteadViewer.Models;
using HomesteadViewer.ViewModels.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HomesteadViewer.Lists;

namespace HomesteadViewer.Helpers
{
    public class DropdownListData
    {
        public static List<GridColumn_DTO> DateColumns()
        {
            var list = new List<GridColumn_DTO>();
            foreach (var gc in GridColumn_DTO.GetAll())
                {
                    var gsvm = new GridSettingsViewModel();                                        
                    var propType = gsvm.GetType().GetProperty(gc.PropertyName).PropertyType;
                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propType = propType.GetGenericArguments()[0];
                    }
                    if (propType == typeof(DateTime))
                    {
                        list.Add(gc);
                    }
                }

            return list;
        }
        public static SelectList Users()
        {
            Dictionary<int, string> rawData = new Dictionary<int, string>();
            
            rawData.Add(0, "- Unassigned -");
            foreach (var user in User_DTO.GetAll().OrderBy(x=>x.FullName))
            {
                rawData.Add(user.Id, user.FullName);
            }
            return new SelectList(rawData, "Key", "Value"); 
        }
        public static SelectList Status()
        {
            Dictionary<int, string> rawData = new Dictionary<int,string>();
            foreach(var status in Enum.GetValues(typeof(ExemptionStatus))){
                rawData.Add((int)status, status.ToString());
            }
            return new SelectList(rawData, "Key", "Value", rawData[2]);
        }
        public static SelectList GridColumns()
        {
            var rawData = new Dictionary<string, string>();
            foreach (var gc in GridColumn_DTO.GetAll())
            {
                var gsvm = new GridSettingsViewModel();
                var propType = gsvm.GetType().GetProperty(gc.PropertyName).PropertyType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propType = propType.GetGenericArguments()[0];
                }
                rawData.Add(gc.PropertyName, gc.DisplayName);
            }

            return new SelectList(rawData, "Key", "Value");
        }
        public static SelectList ExemptionTypes()
        {
            var rawData = new Dictionary<int, string>();
            rawData.Add(0, "AF");
            rawData.Add(1, "CDV");
            rawData.Add(2, "CDVS");
            rawData.Add(3, "CERTFORPART");
            rawData.Add(4, "DP");
            rawData.Add(5, "DV");
            rawData.Add(6, "DVX");
            rawData.Add(7, "DVXS");
            rawData.Add(8, "DVXS2");
            rawData.Add(9, "EXEMPT521");
            rawData.Add(10, "HS");
            rawData.Add(11, "OA");
            rawData.Add(12, "OAS");
            rawData.Add(13, "TCTC");
                    
            return new SelectList(rawData, "Key", "Value");
        }
        public static SelectList Filters()
        {
            var rawData = new Dictionary<string, string>();
            rawData.Add("ExemptionType", "Exemption Type");
            rawData.Add("DateFirstOccupied", "Date First Occupied");
            rawData.Add("ExRequestDate", "Exemption Requested");
            rawData.Add("TaxYear", "Tax Year");
            rawData.Add("AssignedUser", "Assigned User");

            return new SelectList(rawData, "Key", "Value");
        }
    }
    
}