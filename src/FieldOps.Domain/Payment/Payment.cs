using FieldOps.Domain.Abstractions;
using FieldOps.Domain.Orders;

namespace FieldOps.Domain.Payment;

public sealed class Payment : BaseEntity, ITracker
{
    public Guid ClientId { get; set; }
    public Client.Client? Client { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}