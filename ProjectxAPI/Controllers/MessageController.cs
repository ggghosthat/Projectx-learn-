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

        _loggerManager.LogInfo($"Message {message.MessageId} was being sent.");

        return Ok(message);
    }

    [HttpGet("LastHistory")]
    public async Task<IActionResult> GetLastHistory()
    {
        DateTime startTime = DateTime.Now.AddMinutes(-10);
        DateTime endTime = DateTime.Now;
        var result = await _repositoryManager.Messages.GetByTimeframe(startTime, endTime);

        return Ok(result);
    }
}
