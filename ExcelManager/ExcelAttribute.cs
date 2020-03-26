using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelManager
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        public string SheetName { get; private set; }
        public string ColumnName { get; private set; }
        public int ColIndex { get; private set; }
        public int ArrayLength { get; private set; }
        public PropType PropType { get; private set; }
        public Loader Loader { get; private set; }
        public ColumnAttribute(string sheetName, string columnName, int colIndex, int arrayLength = 0, PropType propType = PropType.String,Loader loader = Loader.None)
        {
            this.SheetName = sheetName;
            this.ColIndex = colIndex;
            this.ColumnName = columnName;
            this.ArrayLength = arrayLength;
            this.PropType = propType;
            this.Loader = loader;
        }
    }
    public enum PropType
    {
        String = 1,
        IntToHex = 2,
    }

    public enum Loader
    {
        None = 1,
        Filter = 2,
        Value = 3,
    }
}
