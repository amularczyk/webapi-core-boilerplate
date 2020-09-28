using System;
using ProjectName.Core.Interfaces;

namespace ProjectName.Core.Services
{
    public interface IScopedDateTime : IScoped
    {
        DateTime UtcNow { get; }
    }

    public class ScopedDateTime : IScopedDateTime
    {
        public ScopedDateTime()
        {
            UtcNow = DateTime.UtcNow;
        }

        public DateTime UtcNow { get; }
    }
}