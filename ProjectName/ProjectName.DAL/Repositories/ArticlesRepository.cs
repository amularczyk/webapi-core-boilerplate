using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Models;

namespace ProjectName.DAL.Repositories
{
    public class ArticlesRepository : IArticlesRepository
    {
        private readonly DataContext _context;

        public ArticlesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> RetrieveAllAsync()
        {
            return await _context.Articles.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Article> RetrieveByIdAsync(Guid articleId)
        {
            return await _context.Articles.FindAsync(articleId).ConfigureAwait(false);
        }

        public async Task<bool> InsertAsync(Article article)
        {
            await _context.Articles.AddAsync(article).ConfigureAwait(false);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}