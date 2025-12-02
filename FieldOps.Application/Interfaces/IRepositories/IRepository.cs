using FieldOps.Domain.Abstractions;

namespace FieldOps.Application.Interfaces.IRepositories;

public interface IRepository<T> : IDisposable where T : IEntity
{
  Task<IEnumerable<T>> GetAllAsync();
  Task<T?> GetByIdAsync(Guid id);
  Task<T> CreateAsync(T entity);
  Task<bool> UpdateAsync(T entity);
  Task<bool> DeleteAsync(T entity);
}