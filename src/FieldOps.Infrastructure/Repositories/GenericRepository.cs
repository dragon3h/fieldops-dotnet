using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Domain.Abstractions;
using FieldOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FieldOps.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly FieldOpsDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(FieldOpsDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // todo: should use cancellation token?
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T> CreateAsync(T entity)  // on create always return the entity from DB
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity) // read docs about EF update entity, check Attach method
    {
        var existingEntity = await GetByIdAsync(entity.Id);
        if (existingEntity == null)
        {
            throw new InvalidOperationException($"Entity with id {entity.Id} does not exist.");
        }
        else
        {
            _context.Update(entity);
            return true; // on update always return true or false
        }
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        var existingEntity = await GetByIdAsync(entity.Id);
        if (existingEntity == null)
        {
            return false;
        }
        else
        {

            _context.Remove(existingEntity);
            return true;
        }
    }
    // todo: should i use try/catch in all methods? - centralized exception handling is better, it is too expensive to have try/catch in all methods
}