using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelManager
{
    public interface IExcelProvider
    {
        void Load<T>(string filePath);
        void Export<T>(List<T> obj, string filePath);
    }
}
