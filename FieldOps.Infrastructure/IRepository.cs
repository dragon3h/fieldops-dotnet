using System.Collections.Generic;
using System.Threading.Tasks;

namespace FieldOps.Infrastructure;

public interface IRepository<T> where T : class
{
  Task<List<T>> GetAllAsync();
  Task<T?> GetById(int id);
  Task Create(T entity);
  Task Update(T entity);
  Task Delete(int id);
}