using System;
using ProjectName.Core.Services;

namespace ProjectName.Core.UnitTests
{
    public class TestDateTime : IDateTime
    {
        public TestDateTime(DateTime dateTime)
        {
            UtcNow = dateTime;
        }

        public DateTime UtcNow { get; }
    }
}