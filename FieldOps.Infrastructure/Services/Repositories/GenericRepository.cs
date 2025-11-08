using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Domain.Abstractions;
using FieldOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FieldOps.Infrastructure.Services.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly FieldOpsDbContext _context;
    private readonly DbSet<T> _dbSet;
    
    public GenericRepository(FieldOpsDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // todo: should use cancellation token?
    public async Task<T?> GetById(Guid id)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        };
    }

    public async Task<T> Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync(); // todo: should this be here or in a unit of work?
        return entity; // todo? ask what should be returned here, from DB or the entity itself
    }

    public async Task<T> Update(T entity)
    {
        var existingEntity = await GetById(entity.Id);
        if (existingEntity == null)
        {
            throw new InvalidOperationException($"Entity with id {entity.Id} does not exist.");
        }
        else
        {
            _context.Update(entity); // todo: should i first fetch the entity from DB?
            await  _context.SaveChangesAsync();
            return entity;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        if (entity == null)
        {
            return false;
        }
        else
        {
            _context.Remove(id);
            return true;
        }
    }
    // todo: should i use try/catch in all methods?
}