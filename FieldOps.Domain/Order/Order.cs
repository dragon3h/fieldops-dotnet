using FieldOps.Domain.Abstractions;

namespace FieldOps.Domain.Order;

public sealed class Order : BaseEntity
{
  public required string OrderNumber { get; set; }
  public Guid ClientId { get; set; }
  public Client.Client? Client { get; set; }
  public DateTime EventDate { get; set; }
  public string? EventLocation { get; set; }
  public string? Notes { get; set; }
  public List<OrderItem> OrderItems { get; set; } = [];
  public List<Payment.Payment> Payments { get; set; } = [];
  public DateTime RentalStart { get; set; }
  public decimal TotalAmount { get; set; }
  public decimal DepositAmount { get; set; }
  public bool IsPaid { get; set; }
}
