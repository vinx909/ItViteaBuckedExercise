using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buckets
{
    public class OilBarrel : Container
    {
        private const int oilBarrelCapacity = 159;
        private const OverflowHandlingMethod defaultOverflowHandlingMethod = OverflowHandlingMethod.overflow;

        private IEnumerable<KeyValuePair<string, Action>> overflowingResponceActions;



        protected override IEnumerable<KeyValuePair<string, Action>> OverflowingResponceActions => overflowingResponceActions;



        private void SetupOverflowingResponceAction()
        {
            overflowingResponceActions = new Dictionary<string, Action>()
            {
                { "on overflow cancel fill", () => { overflowHandlingMethod = OverflowHandlingMethod.cancel; } },
                { "on overflow fill till full", () => { overflowHandlingMethod = OverflowHandlingMethod.fillTilFull; } },
                { "on overflow overflow", () => { overflowHandlingMethod = OverflowHandlingMethod.overflow; } }
            };
        }



        public OilBarrel() : base(oilBarrelCapacity, defaultOverflowHandlingMethod) 
        {
            SetupOverflowingResponceAction();
        }
        public OilBarrel(int content) : base(oilBarrelCapacity, content, defaultOverflowHandlingMethod)
        {
            SetupOverflowingResponceAction();
        }
    }
}
