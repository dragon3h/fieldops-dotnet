using FieldOps.Domain.Orders;
using FieldOps.Domain.Payment;
using FieldOps.Domain.Shared;

namespace FieldOps.Application.DTOs
{
    public class PatchClientDTO
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? ContactName { get; set; } = null;
        public string? ContactPhone { get; set; } = null;
        public string? ContactEmail { get; set; } = null;
    }
}