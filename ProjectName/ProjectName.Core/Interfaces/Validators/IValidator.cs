using System.Threading.Tasks;

namespace ProjectName.Core.Interfaces.Validators
{
    public interface IValidator<in T> : ITransient
        where T : class
    {
        Task ValidateEntityAsync(T entity);
    }
}