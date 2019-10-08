using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectName.Core.Models;

namespace ProjectName.Core.Interfaces.Services
{
    public interface IItemsReadService : ITransient
    {
        Task<IEnumerable<Item>> RetrieveAllAsync();
        Task<Item> RetrieveByIdAsync(Guid itemId);
    }
}