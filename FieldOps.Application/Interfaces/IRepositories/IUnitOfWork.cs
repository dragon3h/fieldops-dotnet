namespace FieldOps.Application.Interfaces.IRepositories;

public interface IUnitOfWork
{
    IBouncyCastleRepository BouncyCastleRepository { get; }
    Task CompleteAsync();
}