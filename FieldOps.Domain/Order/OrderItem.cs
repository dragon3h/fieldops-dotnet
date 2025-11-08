using FieldOps.Domain.Abstractions;

namespace FieldOps.Domain.Order;

public sealed class OrderItem : BaseEntity
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
}
