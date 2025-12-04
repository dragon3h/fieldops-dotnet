using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.Data;

namespace FieldOps.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FieldOpsDbContext _fieldOpsDbContext;
    public IBouncyCastleRepository BouncyCastleRepository { get; }
    public IClientRepository ClientRepository { get; }

    public UnitOfWork(FieldOpsDbContext fieldOpsDbContext, IBouncyCastleRepository bouncyCastleRepository, IClientRepository clientRepository)
    {
        _fieldOpsDbContext = fieldOpsDbContext;
        BouncyCastleRepository = bouncyCastleRepository;
        ClientRepository = clientRepository;
    }

    public async Task CompleteAsync()
    {
        // For simple operations, EF Core handles transactions implicitly
        // SaveChangesAsync() wraps all changes in a single transaction automatically
        await _fieldOpsDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _fieldOpsDbContext.Dispose();
    }
}