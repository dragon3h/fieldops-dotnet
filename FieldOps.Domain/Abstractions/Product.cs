using FieldOps.Domain.Shared;

namespace FieldOps.Domain.Abstractions;

public abstract class Product : BaseEntity
{
  public required string Name { get; set; }
  public string Description { get; set; } = string.Empty;
  public bool IsAvailable { get; set; }
  public decimal PurchasePrice { get; set; }
  public decimal ResalePrice { get; set; }
  public decimal RentalPrice { get; set; }
  public decimal DepositPrice { get; set; }
  public ProductType ProductType { get; set; }
}
