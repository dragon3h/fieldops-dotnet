using FieldOps.Domain.Abstractions;

namespace FieldOps.Domain.Orders;

public sealed class Order : BaseEntity, ITracker
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
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}