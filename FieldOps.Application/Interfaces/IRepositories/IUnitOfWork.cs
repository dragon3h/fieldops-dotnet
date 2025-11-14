namespace FieldOps.Application.Interfaces.IRepositories;

public interface IUnitOfWork : IDisposable
{
    IBouncyCastleRepository BouncyCastleRepository { get; }
    Task CompleteAsync();
}