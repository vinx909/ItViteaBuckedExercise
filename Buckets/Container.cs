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
    public delegate void OverflowingDelagate(Container sender, IEnumerable<KeyValuePair<string, Action>> actions, EventArgs eventArgs);
    public delegate void OverflowedDelagate(Container sender, int overflowAmount, EventArgs eventArgs);

    public abstract class Container
    {
        private const int minimalCapacity = 0;
        private const int minimalFillCapacity = 0;
        private const int minimalEmptyCapacity = 0;

        private const string negativeFillAmountExceptionMessage = "can't fill with less then {}";
        private const string negativeEmptyAmountExceptionMessage = "can't empty with less then {}";
        private const string tooLowStartCapacityExceptionMessage = "can start with a capacity lower then {}";

        protected OverflowHandlingMethod overflowHandlingMethod;


        public int Capacity { get; init; }
        public int Content { get; protected set; }
        protected abstract IEnumerable<KeyValuePair<string, Action>> OverflowingResponceActions { get; }

        public void Fill(int amount)
        {
            Fill(amount, true);
        }
        protected void Fill(int amount, bool overflowingEventCall)
        {
            if (amount < minimalFillCapacity)
            {
                throw new Exception(string.Format(negativeFillAmountExceptionMessage, minimalFillCapacity));
            }
            else if (Content + amount > Capacity)
            {
                if(overflowingEventCall == true)
                {
                    Overflowing?.Invoke(this, OverflowingResponceActions, new());
                }
                switch (overflowHandlingMethod)
                {
                    case OverflowHandlingMethod.cancel:
                        return;

                    case OverflowHandlingMethod.fillTilFull:
                        if(Content != Capacity)
                        {
                            Fill(Capacity - Content);
                        }
                        break;

                    case OverflowHandlingMethod.overflow:
                        int overflowAmount = Content + amount - Capacity;
                        Content = Capacity;
                        Filled?.Invoke(this, new());
                        Overflowed?.Invoke(this, overflowAmount, new());
                        break;
                }
            }
            else
            {
                Content += amount;
                if(Content == Capacity)
                {
                    Filled?.Invoke(this, new());
                }
            }
        }
        public void Empty()
        {
            Empty(Content);
        }
        public void Empty(int amount)
        {
            if(amount < minimalEmptyCapacity)
            {
                throw new Exception(string.Format(negativeEmptyAmountExceptionMessage, minimalFillCapacity));
            }
            else
            {
                Content -= amount;
                if (Content < minimalCapacity)
                {
                    Content = minimalCapacity;
                }
            }
        }
        protected void InvokeOverflowing(Container sender, IEnumerable<KeyValuePair<string, Action>> actions, EventArgs eventArgs)
        {
            //TODO ask why you can't invoke an event in an implementing class
            Overflowing?.Invoke(sender, actions, eventArgs);
        }


        public event FilledDelagate Filled;
        public event OverflowingDelagate Overflowing;
        public event OverflowedDelagate Overflowed;


        protected Container(OverflowHandlingMethod overflowHandlingMethod)
        {
            this.overflowHandlingMethod = overflowHandlingMethod;
        }
        protected Container(int capacity, OverflowHandlingMethod overflowHandlingMethod) : this(overflowHandlingMethod)
        {
            this.Capacity = capacity;
        }
        protected Container(int capacity, int content, OverflowHandlingMethod overflowHandlingMethod) : this(capacity, overflowHandlingMethod)
        {
            if (content < minimalCapacity)
            {
                throw new Exception(string.Format(tooLowStartCapacityExceptionMessage, minimalCapacity));
            }
            if (minimalCapacity < minimalFillCapacity)
            {
                Content = content;
            }
            else
            {
                Fill(content);
            }
        }
    }
}
