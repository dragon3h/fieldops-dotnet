namespace FieldOps.Application.Interfaces.IRepositories;

public interface IUnitOfWork
{
    IBouncyCastleRepository BouncyCastleRepository { get; }
    
    IClientRepository ClientRepository { get; }

    Task CompleteAsync();
}