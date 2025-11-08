using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FieldOps.Infrastructure.Services.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IDbContextTransaction _fieldOpsTransaction; // todo: what is it and how to use it?
    private readonly FieldOpsDbContext _fieldOpsDbContext;
    public IBouncyCastleRepository BouncyCastleRepository { get; } // todo: what is it?

    public UnitOfWork(FieldOpsDbContext fieldOpsDbContext)
    {
        _fieldOpsDbContext = fieldOpsDbContext;
        _fieldOpsTransaction = _fieldOpsDbContext.Database.BeginTransaction();
        BouncyCastleRepository = new BouncyCastleRepository(_fieldOpsDbContext); // todo: what is correct argument here?
    }
    
    public async Task CompleteAsync()
    {
        try
        {
            await _fieldOpsDbContext.SaveChangesAsync();
            await _fieldOpsTransaction.CommitAsync();
        }
        catch
        {
            await _fieldOpsTransaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _fieldOpsTransaction.DisposeAsync();
            _fieldOpsTransaction = null;
        }  
    }
    
    public void Dispose()
    {
        if (_fieldOpsTransaction != null)
        {
            _fieldOpsTransaction.Rollback();
            _fieldOpsTransaction.Dispose();
        }
        
        _fieldOpsDbContext?.Dispose();
    }
}