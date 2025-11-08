using FieldOps.Domain.Abstractions;

namespace FieldOps.Application.Interfaces.IRepositories;

public interface IRepository<T> : IDisposable where T : IEntity
{
  Task<IEnumerable<T>> GetAllAsync();
  Task<T?> GetById(Guid id);
  Task<T> Create(T entity);
  Task<T> Update(T entity);
  Task<bool> Delete(Guid id);
}