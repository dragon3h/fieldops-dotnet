using FieldOps.Domain.Abstractions;

namespace FieldOps.Domain.Payment;

public sealed class Payment : BaseEntity
{
  public Guid ClientId { get; set; }
  public Client.Client? Client { get; set; }
  public Guid OrderId { get; set; }
  public Order.Order? Order { get; set; }
  public decimal Amount { get; set; }
  public DateTime PaymentDate { get; set; }
  public string? PaymentMethod { get; set; }
  public string? TransactionId { get; set; }
}