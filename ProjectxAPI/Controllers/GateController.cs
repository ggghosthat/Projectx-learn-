using Microsoft.AspNetCore.Mvc;
using Projectx.Contracts.Logging;
using Projectx.Contracts.Repository;
using Projectx.Entity.DTO;
using Projectx.Entity.Models;

namespace ProjectxAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GateController : ControllerBase
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;

    public GateController(
        ILoggerManager loggerManager,
        IRepositoryManager repositoryManager)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
    }

    [HttpPost("RegisterUser")]
    public async Task<IActionResult> Register([FromBody]ClientRegisterDto clientDto)
    {
        var rnd = new Random();
        var client = new Client
        {
            ClientId = rnd.Next(0, int.MaxValue),
            Name = clientDto.Name
        };

        await _repositoryManager.Clients.Create(client);

        _loggerManager.LogInfo($"Client '{client.Name}' registered.");
        
        return Ok(client.ClientId);
    }
}