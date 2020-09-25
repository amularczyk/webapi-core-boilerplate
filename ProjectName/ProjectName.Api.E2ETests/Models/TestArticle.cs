using System;
using ProjectName.Core.Models;

namespace ProjectName.Api.E2ETests.Models
{
    public class TestArticle : Article
    {
        public TestArticle(Guid id, string name) : base(name)
        {
            Id = id;
        }
    }
}