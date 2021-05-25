using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buckets
{
    public enum RainBarrelSize
    {
        small,
        medium,
        large
    }

    public class RainBarrel : Container
    {
        private static readonly Dictionary<RainBarrelSize, int> rainBarrelSizes = new()
        {
            { RainBarrelSize.small, 80 },
            { RainBarrelSize.medium, 100 },
            { RainBarrelSize.large, 120 }
        };

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


        public RainBarrel(RainBarrelSize size) : base(rainBarrelSizes[size], defaultOverflowHandlingMethod)
        {
            SetupOverflowingResponceAction();
        }
        public RainBarrel(RainBarrelSize size, int content) : base(rainBarrelSizes[size], content, defaultOverflowHandlingMethod)
        {
            SetupOverflowingResponceAction();
        }
    }
}
