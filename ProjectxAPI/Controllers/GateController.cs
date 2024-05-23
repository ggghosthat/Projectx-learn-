using Microsoft.AspNetCore.Mvc;
using Projectx.Contracts.Repository;
using Projectx.Entity.DTO;
using Projectx.Entity.Models;

namespace ProjectxAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GateController : ControllerBase
{
    private readonly IRepositoryManager _repositoryManager;

    public GateController(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody]ClientRegisterDto clientDto)
    {
        var rnd = new Random();
        var client = new Client
        {
            ClientId = rnd.Next(0, int.MaxValue),
            Name = clientDto.Name
        };

        await _repositoryManager.Clients.Create(client);

        return Ok(client);
    }
}