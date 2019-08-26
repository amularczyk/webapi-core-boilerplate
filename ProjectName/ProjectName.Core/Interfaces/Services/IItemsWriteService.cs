using System;
using System.Threading.Tasks;
using ProjectName.Core.Models;

namespace ProjectName.Core.Interfaces.Services
{
    public interface IItemsWriteService
    {
        Task<Guid> InsertAsync(Item item);
    }
}