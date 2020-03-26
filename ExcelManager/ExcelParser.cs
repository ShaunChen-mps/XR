using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StringToLambda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ExcelManager
{
    public class ExcelParser : IExcelProvider
    {
        public void Export<T>(IEnumerable<T> objs, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                IWorkbook workbook = null;
                var lPath = filePath.ToLower();
                if ((lPath.IndexOf(".xlsx") > 0) || (lPath.IndexOf(".csv") > 0)) // 2007版本
                    workbook = new XSSFWorkbook();
                else if (lPath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook();

                var type = typeof(T);
                var cAttrs = type.GetCustomAttributes<SheetNameAttribute>();
                if (cAttrs.Count() > 0)
                {
                    foreach (var cAttr in cAttrs)
                    {
                        var fliter = LambdaParser.Compile<Func<T, bool>>(cAttr.Lambda);
                        var datas = objs.Where(fliter);
                        CreateExcel(datas, workbook, cAttr.SheetName);
                    }
                }
                else
                {
                    CreateExcel(objs, workbook);
                }
                workbook.Write(fs);
            }
        }
        private void CreateExcel<T>(IEnumerable<T> objs, IWorkbook workbook, string sheetName = "")
        {
            ISheet sheet = null;
            foreach (var prop in typeof(T).GetProperties())
            {
                var attrs = prop.GetCustomAttributes<ColumnAttribute>(false).Where(p => sheetName == "" || p.SheetName == sheetName);
                foreach (var attr in attrs)
                {
                    sheet = workbook.GetSheet(attr.SheetName);
                    if (sheet == null)
                    {
                        sheet = workbook.CreateSheet(attr.SheetName);
                        for (int i = 0; i <= objs.Count(); i++)
                        {
                            sheet.CreateRow(i);
                        }
                    }
                    if (prop.PropertyType.IsArray && attr.ArrayLength > 0)
                    {
                        for (int i = 0; i < attr.ArrayLength; i++)
                        {
                            sheet.GetRow(0).CreateCell(attr.ColIndex + i).SetCellValue($"{attr.ColumnName}-{i + 1}");
                        }
                        int index0 = 1;
                        foreach (var obj in objs)
                        {
                            var value = prop.GetValue(obj);
                            var tmps = (int[])value;
                            for (int i = 0; i < tmps.Length; i++)
                            {
                                string val = "";
                                if (attr.PropType == PropType.IntToHex)
                                {
                                    val = $"0x{tmps[i].ToString("X2")}";
                                }
                                else
                                {
                                    val = tmps[i].ToString();
                                }
                                sheet.GetRow(index0).CreateCell(attr.ColIndex + i).SetCellValue(val);
                            }
                            index0++;
                        }
                    }
                    else
                    {
                        sheet.GetRow(0).CreateCell(attr.ColIndex).SetCellValue(attr.ColumnName);
                        int index = 1;
                        foreach (var obj in objs)
                        {
                            var objVal = prop.GetValue(obj);
                            string val = "";
                            if (attr.PropType == PropType.IntToHex)
                            {
                                val = $"0x{((int)objVal).ToString("X2")}";
                            }
                            else
                            {
                                val = objVal.ToString();
                            }
                            sheet.GetRow(index).CreateCell(attr.ColIndex).SetCellValue(val);
                            index++;
                        }
                    }
                }
            }
        }


        public void Load<T>(string filePath)
        {
            throw new NotImplementedException();
        }

        public void Load<T>(IEnumerable<T> objs, string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            {
                IWorkbook workbook = null;
                var lPath = filePath.ToLower();
                if ((lPath.IndexOf(".xlsx") > 0) || (lPath.IndexOf(".csv") > 0)) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (lPath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);
                if (workbook == null) throw new Exception($"Selected filepath is not exist : {filePath}");

                var props = typeof(T).GetProperties();
                var filterProps = new List<PropertyInfo>();
                var valueProps = new List<PropertyInfo>();
                foreach (var prop in props)
                {
                    var attrs = prop.GetCustomAttributes<ColumnAttribute>();
                    if (attrs.Count(p => p.Loader == Loader.Filter) > 0)
                    {
                        filterProps.Add(prop);
                    }
                    else if (attrs.Count(p => p.Loader == Loader.Value) > 0)
                    {
                        valueProps.Add(prop);
                    }
                }

                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);
                    if (sheet != null)
                    {
                        var headerDic = new Dictionary<string, int>();
                        var firstRow = sheet.GetRow(0);
                        for (int j = firstRow.FirstCellNum; j < firstRow.LastCellNum; j++)
                        {
                            headerDic.Add(firstRow.GetCell(j).ToString(), j);
                        }
                    }
                }

            }
        }
    }
}
