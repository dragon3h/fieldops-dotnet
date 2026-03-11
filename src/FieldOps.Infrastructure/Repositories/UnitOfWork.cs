using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.Data;

namespace FieldOps.Infrastructure.Repositories;

public class UnitOfWork(
    FieldOpsDbContext fieldOpsDbContext,
    IBouncyCastleRepository bouncyCastleRepository,
    IClientRepository clientRepository)
    : IUnitOfWork
{
    public IBouncyCastleRepository BouncyCastleRepository { get; } = bouncyCastleRepository;
    public IClientRepository ClientRepository { get; } = clientRepository;

    public async Task CompleteAsync()
    {
        // For simple operations, EF Core handles transactions implicitly
        // SaveChangesAsync() wraps all changes in a single transaction automatically
        await fieldOpsDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        fieldOpsDbContext.Dispose();
    }
}