using AutoMapper;
using FieldOps.Application.DTOs;
using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Domain.Client;
using Microsoft.AspNetCore.Mvc;

namespace FieldOps.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientController(IMapper mapper, IClientService clientService) : ControllerBase
{
    // GET: api/v1/client
    [HttpGet]
    public async Task<ActionResult<List<ClientDTO>>> GetClients()
    {
        var clientList = await clientService.GetAllClientsAsync();

        var clientDtos = mapper.Map<List<ClientDTO>>(clientList);
        return Ok(clientDtos);
    }

    // GET: api/v1/client/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClientDTO>> GetClientById(Guid id)
    {
        var client = await clientService.GetClientByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        var clientDto = mapper.Map<ClientDTO>(client);
        return Ok(clientDto);
    }

    // POST: api/v1/client
    [HttpPost]
    public async Task<ActionResult<ClientDTO>> CreateClient(ClientDTO clientDto)
    {
        var clientEntity = mapper.Map<Client>(clientDto);
        var createdClient = await clientService.CreateClientAsync(clientEntity);
        var createdClientDto = mapper.Map<ClientDTO>(createdClient);
        return CreatedAtAction(nameof(GetClientById), new { id = createdClientDto.Id }, createdClientDto);
    }

    // PUT: api/v1/client/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateClient(Guid id, ClientDTO clientDto)
    {
        if (id != clientDto.Id)
        {
            return BadRequest();
        }
        var clientEntity = mapper.Map<Client>(clientDto);

        if (await clientService.GetClientByIdAsync(id) == null)
        {
            return NotFound();
        }

        await clientService.UpdateClientAsync(clientEntity);
        return NoContent();
    }
    
    // PATCH: api/v1/client/{id}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PatchClient(Guid id, PatchClientDTO patchClientDto)
    {
        var existingClient = await clientService.PatchClientAsync(id, patchClientDto);
        if (existingClient == null)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    // DELETE: api/v1/client/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var client = await clientService.GetClientByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        await clientService.DeleteClientAsync(client);
        return NoContent();
    }
}
