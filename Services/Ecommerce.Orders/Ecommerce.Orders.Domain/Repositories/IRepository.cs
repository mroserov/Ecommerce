using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Orders.Domain.Repositories;
public interface IRepository<T> where T : class
{
    void Add(T entity);
    // Otros métodos CRUD
}
