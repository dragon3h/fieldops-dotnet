using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Domain.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldOps.Application.ClientApp;

public class ClientService(IUnitOfWork unitOfWork) : IClientService
{
    public async Task<List<Client>> GetAllClientsAsync()
    {
        var clients = await unitOfWork.ClientRepository.GetAllAsync();

        return clients.ToList();
    }

    public async Task<Client?> GetClientByIdAsync(Guid id)
    {
        var client = await unitOfWork.ClientRepository.GetByIdAsync(id);

        return client;
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        client.Id = Guid.NewGuid();
        client.CreatedAt = DateTime.UtcNow;
        client.CreatedBy = "system"; // This should ideally come from the authenticated user context
        var createdClient = await unitOfWork.ClientRepository.CreateAsync(client);
        await unitOfWork.CompleteAsync();

        return createdClient;
    }

    public async Task DeleteClientAsync(Client client)
    {
        await unitOfWork.ClientRepository.DeleteAsync(client);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateClientAsync(Client client)
    {
        client.UpdatedAt = DateTime.UtcNow;
        client.UpdatedBy = "system"; // This should ideally come from the authenticated user context
        await unitOfWork.ClientRepository.UpdateAsync(client);
        await unitOfWork.CompleteAsync();
    }
}
