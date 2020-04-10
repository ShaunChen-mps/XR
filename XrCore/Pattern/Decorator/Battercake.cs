using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrCore.Pattern.Decorator
{
    public class Battercake : IBattercake
    {
        public int Cost()
        {
            return 8;
        }

        public string GetDesc()
        {
            return "普通煎饼果子";
        }
    }
}
