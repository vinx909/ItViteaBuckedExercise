using System;
using System.Collections.Generic;

namespace Buckets
{
    public class Bucket : Container
    {
        private const int defaultCapacity = 12;
        private const int minimalCapacity = 10;
        private const OverflowHandlingMethod defaultOverflowHandlingMethod = OverflowHandlingMethod.overflow;

        private const string fillBucketWithItselfExceptionMessage = "can't fill a bucket with itself";
        private const string tooLowStartCapacityExceptionMessage = "can't have a capacity lower then {}";

        private IEnumerable<KeyValuePair<string, Action>> overflowingResponceActions;

        protected override IEnumerable<KeyValuePair<string, Action>> OverflowingResponceActions => overflowingResponceActions;


        public void Fill(Bucket bucket)
        {
            if(bucket == this)
            {
                throw new Exception(fillBucketWithItselfExceptionMessage);
            }
            else if(bucket.Content + this.Content > this.Capacity)
            {
                InvokeOverflowing(this, OverflowingResponceActions, new());
                switch (overflowHandlingMethod)
                {
                    case OverflowHandlingMethod.cancel:
                        return;

                    case OverflowHandlingMethod.fillTilFull:
                        if (Content != Capacity)
                        {
                            int amountFilled = this.Capacity - this.Content;
                            Fill(amountFilled);
                            bucket.Empty(amountFilled);
                        }
                        break;

                    case OverflowHandlingMethod.overflow:
                        Fill(bucket.Content, false);
                        bucket.Empty();
                        break;
                }
            }
            else
            {
                Fill(bucket.Content);
                bucket.Empty();
            }
        }
        private void SetupOverflowingResponceAction()
        {
            overflowingResponceActions = new Dictionary<string, Action>()
            {
                { "on overflow cancel fill", () => { overflowHandlingMethod = OverflowHandlingMethod.cancel; } },
                { "on overflow fill till full", () => { overflowHandlingMethod = OverflowHandlingMethod.fillTilFull; } },
                { "on overflow overflow", () => { overflowHandlingMethod = OverflowHandlingMethod.overflow; } }
            };
        }


        public Bucket() : base(defaultCapacity, defaultOverflowHandlingMethod) 
        {
            SetupOverflowingResponceAction();
        }
        public Bucket(int capacity) : base(capacity, defaultOverflowHandlingMethod)
        {
            if (capacity < minimalCapacity)
            {
                throw new Exception(string.Format(tooLowStartCapacityExceptionMessage, minimalCapacity));
            }
            SetupOverflowingResponceAction();
        }
        public Bucket(int capacity, int content) : this(capacity)
        {
            SetupOverflowingResponceAction();
            Fill(content);
        }
    }
}
