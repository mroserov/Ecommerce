using Ecommerce.Authentication.Domain.Entities;
using Ecommerce.Authentication.Domain.Interfaces;
using Ecommerce.Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Authentication.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer entity)
        {
            await _context.Customers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer entity)
        {
            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task UpdateAsync(Customer entity)
        {
            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
