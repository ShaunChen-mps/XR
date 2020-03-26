using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelManager
{
    public class RomRegExecelRow
    {
        [Column("ROM", "Register Page", 1)]
        public ushort Page { get; private set; }
        [Column("ROM", "Register Address", 2)]
        public string Address { get; private set; }
        [Column("ROM", "Register Name", 0)]
        public string AddressName { get; private set; }
        [Column("ROM", "Register Bit Count", 3)]
        public int BitsCount { get; private set; }
        [Column("ROM", "Register Value", 4)]
        public int Value { get; private set; }
        [Column("ROM", "Register Cfgs", 5)]
        public int[] Cfgs { get; set; }
        public RomRegExecelRow(ushort page, string address, int value, string addressName, int bitsCount, int cfgCount)
        {
            this.Page = page;
            this.Address = address;
            this.Value = value;
            this.AddressName = addressName;
            this.BitsCount = bitsCount;
            this.Cfgs = new int[cfgCount];
        }
    }
    public class ExcelProvider : IExcelProvider
    {
        public void Export(IEnumerable<Register> data, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                IWorkbook workbook = null;
                if ((fileName.IndexOf(".xlsx") > 0) || (fileName.IndexOf(".XLSX") > 0) || (fileName.IndexOf(".csv") > 0)) // 2007版本
                    workbook = new XSSFWorkbook();
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook();

                var ramSheet = workbook.CreateSheet("RAM");
                var ramRegs = data.Where(p => !p.IsRom);
                for (int i = 0; i <= ramRegs.Count(); i++)
                {
                    ramSheet.CreateRow(i);
                }
                
                foreach (var prop in typeof(Register).GetProperties())
                {
                    var attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true) as IEnumerable<ColumnAttribute>;
                    var attr = attrs.FirstOrDefault(p => p.SheetName == "RAM");
                    if (attr != null)
                    {
                        var cell = ramSheet.GetRow(0).CreateCell(attr.ColIndex);
                        cell.SetCellValue(attr.ColumnName);
                        int index = 1;
                        foreach (var reg in ramRegs)
                        {
                            var cellV = ramSheet.GetRow(index).CreateCell(attr.ColIndex);
                            cellV.SetCellValue(prop.GetValue(reg).ToString());
                            index++;
                        }
                    }
                }

                var romSheet = workbook.CreateSheet("ROM");
                var romRegs = data.Where(p => p.IsRom);
                var romRegsGByPage = romRegs.GroupBy(p => p.CfgN).OrderBy(p => p.Key).ToArray();
                var cfgCount = romRegsGByPage.Length;
                var romRegExecelRows = new List<RomRegExecelRow>();
                for (int i = 0; i < cfgCount; i++)
                {
                    var romRegsG = romRegsGByPage[i];
                    foreach (var romReg in romRegsG)
                    {
                        var romRegExecelRow = romRegExecelRows.FirstOrDefault(p => p.Address == romReg.Address && p.Page == romReg.Page);
                        if (romRegExecelRow == null)
                        {
                            romRegExecelRow = new RomRegExecelRow(romReg.Page, romReg.Address, romReg.RegisterValue, romReg.Name, romReg.BitsCount, cfgCount);
                            romRegExecelRows.Add(romRegExecelRow);
                        }
                        romRegExecelRow.Cfgs[i] = romReg.RegisterValue;
                    }
                }
                for (int i = 0; i <= romRegExecelRows.Count; i++)
                {
                    romSheet.CreateRow(i);
                }
                foreach (var prop in typeof(RomRegExecelRow).GetProperties())
                {
                    var attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true) as IEnumerable<ColumnAttribute>;
                    var attr = attrs.FirstOrDefault(p => p.SheetName == "ROM");
                    if (attr != null)
                    {
                        if (prop.PropertyType.IsArray)
                        {
                            for (int i = 0; i < cfgCount; i++)
                            {
                                var cell = romSheet.GetRow(0).CreateCell(attr.ColIndex + i);
                                cell.SetCellValue($"{attr.ColumnName}-{i + 1}");
                            }

                            int index = 1;
                            foreach (var dr in romRegExecelRows)
                            {
                                var value = prop.GetValue(dr) as int[];
                                for (int i = 0; i < value.Length; i++)
                                {
                                    var cellV = romSheet.GetRow(index).CreateCell(attr.ColIndex + i);
                                    cellV.SetCellValue(value[i].ToString());
                                }
                                index++;
                            }
                        }
                        else
                        {
                            var cell = romSheet.GetRow(0).CreateCell(attr.ColIndex);
                            cell.SetCellValue(attr.ColumnName);

                            int index = 1;
                            foreach (var dr in romRegExecelRows)
                            {
                                var cellV = romSheet.GetRow(index).CreateCell(attr.ColIndex);
                                cellV.SetCellValue(prop.GetValue(dr).ToString());
                                index++;
                            }
                        }
                    }
                }
                workbook.Write(fs);
            }
        }

        public void Export<T>(IEnumerable<T> obj, string filePath)
        {
            throw new NotImplementedException();
        }

        public void Export<T>(List<T> obj, string filePath)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var execelDic = new Dictionary<string, List<object>>();
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true).Where(t => t.GetType() == typeof(ColumnAttribute)) as IEnumerable<ColumnAttribute>;
                if (attrs?.Count() > 0)
                {
                    foreach (var attr in attrs)
                    {
                        if (!execelDic.Keys.Contains(attr.SheetName))
                            execelDic.Add(attr.SheetName, new List<object>());
                        var columns = execelDic[attr.SheetName];
                        columns.Add(new
                        {

                        });
                    }
                }
            }
        }

        public void Load<T>(string filePath)
        {
            throw new NotImplementedException();
        }
    }
    public class ExcelTable
    {
        public string SheetName;
        public List<string> ColumnNames = new List<string>();
    }
}
