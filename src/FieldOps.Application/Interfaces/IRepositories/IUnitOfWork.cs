namespace FieldOps.Application.Interfaces.IRepositories;

public interface IUnitOfWork : IDisposable
{
    IBouncyCastleRepository BouncyCastleRepository { get; }
    
    IClientRepository ClientRepository { get; }

    Task CompleteAsync();
}