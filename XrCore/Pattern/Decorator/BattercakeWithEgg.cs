using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrCore.Pattern.Decorator
{
    public class BattercakeWithEgg : BattercakeDecorator
    {
        public BattercakeWithEgg(IBattercake battercake) : base(battercake)
        {
            var a = new Battercake();
            var b = new BattercakeWithEgg(a);
            var c = new BattercakeWithEgg(b);
        }
        public override int Cost()
        {
            return base.Cost() + 3;
        }
        public override string GetDesc()
        {
            return base.GetDesc() + "加个鸡蛋";
        }
    }
}
