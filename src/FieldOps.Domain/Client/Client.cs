using FieldOps.Domain.Abstractions;
using FieldOps.Domain.Shared;
using FieldOps.Domain.Orders;

namespace FieldOps.Domain.Client;

public sealed class Client : BaseEntity, ITracker
{
    public required string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public required string ContactName { get; set; }
    public required string ContactPhone { get; set; }
    public string ContactEmail { get; set; }
    public Address? Address { get; set; }
    public List<Order> Orders { get; set; } = [];
    public List<Payment.Payment> Payments { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

}