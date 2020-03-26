using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelManager
{
    /// <summary>
    /// 根据属性值建立表格
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SheetNameAttribute : Attribute
    {
        /// <summary>
        /// 表格名称
        /// </summary>
        public string SheetName { get; private set; }
        /// <summary>
        /// 筛选的Lambda表达式字符串
        /// </summary>
        public string Lambda { get; private set; }
        public SheetNameAttribute(string sheetName, string lambda)
        {
            this.SheetName = sheetName;
            this.Lambda = lambda;
        }
    }
}
