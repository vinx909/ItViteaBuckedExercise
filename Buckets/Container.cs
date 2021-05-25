using System;
using System.Collections.Generic;

namespace Buckets
{
    public enum OverflowHandlingMethod
    {
        cancel,
        fillTilFull,
        overflow
    }

    public delegate void FilledDelagate(Container sender, EventArgs eventArgs);
    public delegate void OverflowedDelagate(Container sender, int overflowAmount, EventArgs eventArgs);

    public abstract class Container
    {
        private const int defaultCapacity = 12;
        private const int minimalCapacity = 10;
        private const int minimalStartCapacity = 0;
        private const int minimalFillCapacity = 0;

        private const string negativeFillAmountExceptionMessage = "can't fill with less then {}";
        private const string tooLowStartCapacity = "can start with a capacity lower then {}";

        private const OverflowHandlingMethod defaultOverflowHandlingMethod = OverflowHandlingMethod.overflow;

        protected OverflowHandlingMethod overflowHandlingMethod;

        protected Container(OverflowHandlingMethod overflowHandlingMethod)
        {
            this.overflowHandlingMethod = overflowHandlingMethod;
        }
        public Container() : this(defaultCapacity) { }
        protected Container(int capacity, OverflowHandlingMethod overflowHandlingMethod) : this(overflowHandlingMethod)
        {
            if(capacity < minimalCapacity)
            {
                throw new Exception();
            }
            this.Capacity = capacity;
        }
        public Container(int capacity) : this(capacity, defaultOverflowHandlingMethod) { }
        public Container(int capacity, int content) : this(capacity)
        {
            if(content < minimalStartCapacity)
            {
                throw new Exception(string.Format(tooLowStartCapacity, minimalStartCapacity));
            }
            if(minimalStartCapacity < minimalFillCapacity)
            {
                Content = content;
            }
            else
            {
                Fill(content);
            }
        }

        public int Capacity { get; init; }
        public int Content { get; protected set; }

        public void Fill(int amount)
        {
            if (amount < 0)
            {
                throw new Exception(string.Format(negativeFillAmountExceptionMessage, minimalFillCapacity));
            }
            else if(Content + amount > Capacity)
            {
                
            }
            else
            {

            }
        }

        protected void Fill(int amount)
        {
            Content += amount;
            if(Content >= )
        }

        public event FilledDelagate Filled;
        public event OverflowedDelagate Overflowed;
    }
}
