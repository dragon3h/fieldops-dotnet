namespace FieldOps.Application.Interfaces.IRepositories;

public interface IUnitOfWork : IDisposable
{
    IBouncyCastleRepository BouncyCastleRepository { get; } // we have only get here because we don't want to set the repository from outside. setting the repository should be done only inside the UnitOfWork class
    Task CompleteAsync(); // todo: why CompleteAsync and not SaveChangesAsync?
}