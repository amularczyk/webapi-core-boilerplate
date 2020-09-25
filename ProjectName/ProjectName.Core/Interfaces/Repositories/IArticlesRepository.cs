using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectName.Core.Models;

namespace ProjectName.Core.Interfaces.Repositories
{
    public interface IArticlesRepository : ITransient
    {
        Task<IEnumerable<Article>> RetrieveAllAsync();
        Task<Article> RetrieveByIdAsync(Guid articleId);
        Task<bool> InsertAsync(Article article);
        Task<bool> UpdateAsync(Article article);
    }
}