using System;

namespace ProjectName.Core.Services
{
    public static class Clock
    {
        private static IDateTime _dateTime;

        public static IDateTime InternalDateTime
        {
            get { return _dateTime ??= new SystemDateTime(); }
            set => _dateTime = value;
        }

        public static DateTime UtcNow => InternalDateTime.UtcNow;
    }

    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }

    public class SystemDateTime : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}