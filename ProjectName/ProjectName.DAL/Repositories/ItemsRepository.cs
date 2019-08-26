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
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> RetrieveByIdAsync(Guid itemId)
        {
            return await _context.Items.FindAsync(itemId);
        }

        public async Task<bool> InsertAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}