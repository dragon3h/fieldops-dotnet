using FieldOps.Domain.Abstractions;

namespace FieldOps.Domain.Order;

public sealed class OrderItem : BaseEntity, ITracker
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal RentalPricePerUnit { get; set; }
    public decimal DepositPricePerUnit { get; set; }
    public decimal LineTotal => Quantity * RentalPricePerUnit;
    public decimal LineTotalDeposit => Quantity * DepositPricePerUnit;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}