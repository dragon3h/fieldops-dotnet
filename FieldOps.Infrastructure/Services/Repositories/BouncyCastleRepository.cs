using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.Data;
using BouncyCastleEntity = FieldOps.Domain.BouncyCastle.BouncyCastle;

namespace FieldOps.Infrastructure.Services.Repositories;

public class BouncyCastleRepository : GenericRepository<BouncyCastleEntity>, IBouncyCastleRepository
{
  public BouncyCastleRepository(FieldOpsDbContext context): base(context)
  {
  }
  
  // add bouncy castle specific methods here
}