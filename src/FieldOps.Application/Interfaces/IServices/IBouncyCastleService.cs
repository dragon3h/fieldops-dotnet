using FieldOps.Domain.BouncyCastle;

namespace FieldOps.Application.Interfaces.IServices;

public interface IBouncyCastleService
{
    Task<List<BouncyCastle>> GetAllAsync();
    Task<BouncyCastle?> GetByIdAsync(Guid id);
    Task<BouncyCastle> CreateAsync(BouncyCastle castle);
    Task UpdateAsync(BouncyCastle castle);
    Task DeleteAsync(BouncyCastle castle);
}