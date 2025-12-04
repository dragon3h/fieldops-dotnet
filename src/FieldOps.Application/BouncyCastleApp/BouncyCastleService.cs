using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Domain.BouncyCastle;

namespace FieldOps.Application.BouncyCastleApp;

public class BouncyCastleService(IUnitOfWork unitOfWork) : IBouncyCastleService
{
    public async Task<List<BouncyCastle>> GetAllBouncyCastlesAsync()
    {
        var bouncyCastles = await unitOfWork.BouncyCastleRepository.GetAllAsync();
        return bouncyCastles.ToList();
    }
    
    public async Task<BouncyCastle?> GetBouncyCastleByIdAsync(Guid id)
    {
        var castle = await unitOfWork.BouncyCastleRepository.GetByIdAsync(id);

        return castle;
    }
    
    public async Task<BouncyCastle> CreateBouncyCastleAsync(BouncyCastle castle)
    {
        castle.Id = Guid.NewGuid();
        castle.CreatedAt = DateTime.UtcNow;
        castle.CreatedBy = "system"; // TODO: replace with actual user
        await unitOfWork.BouncyCastleRepository.CreateAsync(castle);
        await unitOfWork.CompleteAsync();
        
        return castle;
    }
    
    public async Task UpdateBouncyCastleAsync(BouncyCastle castle)
    {
        castle.UpdatedAt = DateTime.UtcNow;
        castle.UpdatedBy = "system"; // TODO: replace with actual user
        await unitOfWork.BouncyCastleRepository.UpdateAsync(castle);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteBouncyCastleAsync(BouncyCastle castle)
    {
        await unitOfWork.BouncyCastleRepository.DeleteAsync(castle);
        await unitOfWork.CompleteAsync();
    }
}