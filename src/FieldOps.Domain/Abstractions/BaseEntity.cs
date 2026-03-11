namespace FieldOps.Domain.Abstractions;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; }
}