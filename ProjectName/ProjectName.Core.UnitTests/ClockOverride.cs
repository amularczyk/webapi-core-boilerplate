using System;
using ProjectName.Core.Services;

namespace ProjectName.Core.UnitTests
{
    public class ClockOverride : IDisposable
    {
        private readonly IDateTime _previousClock;
        
        public ClockOverride(DateTime utcNow)
        {
            _previousClock = Clock.InternalDateTime;
            Clock.InternalDateTime = new TestDateTime(utcNow);
        }

        public void ForwardBy(TimeSpan timeSpan)
        {
            Clock.InternalDateTime = new TestDateTime(Clock.UtcNow.Add(timeSpan));
        }

        public void Dispose()
        {
            Clock.InternalDateTime = _previousClock;
        }
    }
}