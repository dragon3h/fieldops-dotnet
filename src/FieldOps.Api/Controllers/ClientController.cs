using AutoMapper;
using FieldOps.Application.DTOs;
using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Domain.Client;
using Microsoft.AspNetCore.Mvc;

namespace FieldOps.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IClientService _clientService;

    public ClientController(IMapper mapper, IClientService clientService)
    {
        _mapper = mapper;
        _clientService = clientService;
    }

    // GET: api/v1/client
    [HttpGet]
    public async Task<ActionResult<List<ClientDTO>>> GetClients()
    {
        var clientList = await _clientService.GetAllClientsAsync();

        var clientDtos = _mapper.Map<List<ClientDTO>>(clientList);
        return Ok(clientDtos);
    }

    // GET: api/v1/client/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClientDTO>> GetClientById(Guid id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        var clientDto = _mapper.Map<ClientDTO>(client);
        return Ok(clientDto);
    }

    // POST: api/v1/client
    [HttpPost]
    public async Task<ActionResult<ClientDTO>> CreateClient(ClientDTO clientDto)
    {
        var clientEntity = _mapper.Map<Client>(clientDto);
        var createdClient = await _clientService.CreateClientAsync(clientEntity);
        var createdClientDto = _mapper.Map<ClientDTO>(createdClient);
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
        var clientEntity = _mapper.Map<Client>(clientDto);

        if (await _clientService.GetClientByIdAsync(id) == null)
        {
            return NotFound();
        }

        await _clientService.UpdateClientAsync(clientEntity);
        return NoContent();
    }

    // DELETE: api/v1/client/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        await _clientService.DeleteClientAsync(client);
        return NoContent();
    }
}
