using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.Data;

namespace FieldOps.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FieldOpsDbContext _fieldOpsDbContext;
    public IBouncyCastleRepository BouncyCastleRepository { get; }

    public UnitOfWork(FieldOpsDbContext fieldOpsDbContext)
    {
        _fieldOpsDbContext = fieldOpsDbContext;
        BouncyCastleRepository = new BouncyCastleRepository(_fieldOpsDbContext);
    }

    public async Task CompleteAsync()
    {
        // For simple operations, EF Core handles transactions implicitly
        // SaveChangesAsync() wraps all changes in a single transaction automatically
        await _fieldOpsDbContext.SaveChangesAsync();
    }

}