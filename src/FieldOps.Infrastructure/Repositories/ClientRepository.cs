using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Domain.Client;
using FieldOps.Infrastructure.Data;

namespace FieldOps.Infrastructure.Repositories;

public class ClientRepository(FieldOpsDbContext context) : GenericRepository<Client>(context), IClientRepository
{    // add client specific methods here
}
