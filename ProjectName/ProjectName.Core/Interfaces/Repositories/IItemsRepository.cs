﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectName.Core.Models;

namespace ProjectName.Core.Interfaces.Repositories
{
    public interface IItemsRepository : ITransient
    {
        Task<IEnumerable<Item>> RetrieveAllAsync();
        Task<Item> RetrieveByIdAsync(Guid itemId);
        Task<bool> InsertAsync(Item item);
        Task<bool> UpdateAsync(Item item);
    }
}