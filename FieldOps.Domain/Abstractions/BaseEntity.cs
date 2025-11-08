namespace FieldOps.Domain.Abstractions;

public abstract class BaseEntity : IEntity
{
  public Guid Id { get; set; } = Guid.NewGuid(); // todo: ask what is the best practice
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public string? CreatedBy { get; set; }
  public string? UpdatedBy { get; set; }
}