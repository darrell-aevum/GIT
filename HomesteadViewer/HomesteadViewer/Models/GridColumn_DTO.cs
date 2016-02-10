using HomesteadViewer.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

namespace HomesteadViewer.Models
{
    [Table("GridColumns")]
    public class GridColumn_DTO
    {
        [Key]
        public string PropertyName { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }  
        public int ColumnNumber { get; set; }
        public bool Displayed { get; set; }
        public bool Editable { get; set; }
        public int? Width { get; set; }
        
        public static List<GridColumn_DTO> GetAllDisplayedColumns()
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    return context.GridColumns.Where(gc=>gc.Displayed).OrderBy(gc=>gc.ColumnNumber).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GridColumn_DTO Get(string propertyName)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.GridColumns.FirstOrDefault(x => x.PropertyName.ToLower() == propertyName.ToLower()) ?? null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<GridColumn_DTO> GetAll()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.GridColumns.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GridColumn_DTO Get(Func<GridColumn_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.GridColumns.FirstOrDefault(func) ?? null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<GridColumn_DTO> GetAll(Func<GridColumn_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.GridColumns.Where(func).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void ReorderGridColumns()
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    var displayedCols = context.GridColumns.Where(g => g.Displayed).OrderBy(g => g.ColumnNumber).ToList();
                    for (int i = 0; i < displayedCols.Count(); i++)
                    {
                        using (var newContext = new HomesteadViewerContext())
                        {
                            var col = displayedCols[i];
                            col.ColumnNumber = i + 1;
                            newContext.GridColumns.Attach(col);
                            newContext.Entry(col).State = System.Data.Entity.EntityState.Modified;
                            newContext.SaveChanges();
                        }
                    }
                    var hiddenCols = context.GridColumns.Where(g => !g.Displayed).OrderBy(g => g.PropertyName).ToList();

                    for (int i = 0; i < hiddenCols.Count(); i++)
                    {
                        using (var newContext = new HomesteadViewerContext())
                        {
                            var col = hiddenCols[i];
                            col.ColumnNumber = i + displayedCols.Count() + 1;
                            newContext.GridColumns.Attach(col);
                            newContext.Entry(col).State = System.Data.Entity.EntityState.Modified;
                            newContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GridColumn_DTO ReorderGridColumn(GridColumn_DTO new_gc)
        {
            try
            {
                using (var context = new HomesteadViewerContext())
                {
                    var orig_gc = Get(new_gc.PropertyName);
                    if (orig_gc == null)
                        return null;

                    if (orig_gc.ColumnNumber < new_gc.ColumnNumber)
                        foreach (var col in GetAll(g => g.ColumnNumber < orig_gc.ColumnNumber && g.ColumnNumber >= new_gc.ColumnNumber))
                        {
                            using (var newContext = new HomesteadViewerContext())
                            {
                                col.ColumnNumber--;
                                newContext.GridColumns.Attach(col);
                                newContext.Entry(col).State = System.Data.Entity.EntityState.Modified;
                                newContext.SaveChanges();
                            }
                        }
                    else if (orig_gc.ColumnNumber > new_gc.ColumnNumber)
                        foreach (var col in GetAll(g => g.ColumnNumber >= new_gc.ColumnNumber && g.ColumnNumber < orig_gc.ColumnNumber))
                        {
                            using (var newContext = new HomesteadViewerContext())
                            {
                                col.ColumnNumber++;
                                newContext.GridColumns.Attach(col);
                                newContext.Entry(col).State = System.Data.Entity.EntityState.Modified;
                                newContext.SaveChanges();
                            }
                        }
                    else
                    {                       
                        return orig_gc;                       
                    }

                    orig_gc.ColumnNumber = new_gc.ColumnNumber;

                    context.GridColumns.Attach(orig_gc);
                    context.Entry(orig_gc).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    ReorderGridColumns();
                    return Get(orig_gc.PropertyName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GridColumn_DTO UpdateGridColumn(GridColumn_DTO gridColumn)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var gc = Get(gridColumn.PropertyName);
                    if (gc == null)
                        return null;
                    gc = ReorderGridColumn(gc);

                    Mapper.CreateMap<GridColumn_DTO, GridColumn_DTO>();
                    Mapper.Map<GridColumn_DTO, GridColumn_DTO>(gridColumn, gc);

                    

                    context.GridColumns.Attach(gc);
                    context.Entry(gc).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    ReorderGridColumns();
                    return Get(gridColumn.PropertyName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
    }
}