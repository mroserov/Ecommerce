using Ecommerce.Authentication.Domain.Entities;
using Ecommerce.Authentication.Domain.Interfaces;
using Ecommerce.Authentication.Infrastructure.Data;

namespace Ecommerce.Authentication.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Customer> _customers;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Customer> Customers
        {
            get { return _customers ??= new CustomerRepository(_context); }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
