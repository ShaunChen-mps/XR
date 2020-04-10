using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrCore.Pattern.Decorator
{
    public class BattercakeDecorator : IBattercake
    {
        protected IBattercake Battercake;
        public BattercakeDecorator(IBattercake battercake)
        {
            this.Battercake = battercake;
        }
        public virtual int Cost()
        {
            return this.Battercake.Cost();
        }

        public virtual string GetDesc()
        {
            return this.Battercake.GetDesc();
        }
    }
}
