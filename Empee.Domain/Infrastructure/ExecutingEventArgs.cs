using System;

namespace Empee.Domain.Infrastructure
{
    public class ExecutingEventArgs : EventArgs
    {
        public ExecutingEventArgs(double executingDelta)
        {
            ExecutingDelta = executingDelta;
        }

        public double ExecutingDelta { get; private set; }
    }
}