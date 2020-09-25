using System;
using ProjectName.Core.Models;

namespace ProjectName.Api.E2ETests.Models
{
    public class TestItem : Item
    {
        public TestItem(Guid id, string name) : base(name)
        {
            Id = id;
        }
    }
}