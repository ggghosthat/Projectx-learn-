using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projectx.Contracts.Logging;
using Projectx.Contracts.Repository;
using Projectx.Entity.DTO;
using Projectx.Entity.Models;

namespace ProjectxAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;

    public MessageController(
        ILoggerManager loggerManager,
        IRepositoryManager repositoryManager)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
    }

    [HttpPost("SendMessage")]
    public async Task<IActionResult> Send([FromBody] MessageDto messageDto)
    {
        var message = new Message
        {
            MessageId = Guid.NewGuid(),
            ClientId = messageDto.ClientId,
            Created = DateTime.Now,
            Content = messageDto.Content
        };

        await _repositoryManager.Messages.Create(message);

        return Ok(message);
    }
}
