using Ecommerce.Authentication.Domain.Entities;

namespace Ecommerce.Authentication.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Customer> Customers { get; }
        Task<int> SaveChangesAsync();
    }
}
