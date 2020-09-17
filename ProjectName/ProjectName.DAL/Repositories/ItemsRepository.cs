using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Models;

namespace ProjectName.DAL.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly DataContext _context;

        public ItemsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> RetrieveAllAsync()
        {
            return await _context.Items.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Item> RetrieveByIdAsync(Guid itemId)
        {
            return await _context.Items.FindAsync(itemId).ConfigureAwait(false);
        }

        public async Task<bool> InsertAsync(Item item)
        {
            await _context.Items.AddAsync(item).ConfigureAwait(false);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}