using FieldOps.Domain.Shared;

namespace FieldOps.Domain.Abstractions;

public abstract class Product : BaseEntity, ITracker
{
    public required string Name { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal ResalePrice { get; set; }
    public decimal RentalPrice { get; set; }
    public decimal DepositPrice { get; set; }
    public ProductType ProductType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}