using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelManager
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelAttribute : Attribute
    {
        public string SheetName { get; private set; }
        public string ColumnName { get; private set; }
        public int ColIndex { get; private set; }
        public ExcelAttribute(string sheetName, string columnName, int colIndex)
        {
            this.SheetName = sheetName;
            this.ColIndex = colIndex;
            this.ColumnName = columnName;
        }
    }
}
