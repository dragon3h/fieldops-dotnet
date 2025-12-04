using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Domain.Client;
using FieldOps.Infrastructure.Data;

namespace FieldOps.Infrastructure.Repositories;

public class ClientRepository : GenericRepository<Client>, IClientRepository
{
    public ClientRepository(FieldOpsDbContext context) : base(context)
    {
    }
    // add client specific methods here
}
