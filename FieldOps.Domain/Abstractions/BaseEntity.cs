namespace FieldOps.Domain.Abstractions;

public abstract class BaseEntity : IEntity
{
  public Guid Id { get; set; } = Guid.Empty;
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public string? CreatedBy { get; set; }
  public string? UpdatedBy { get; set; }
}