using Microsoft.AspNetCore.Mvc;
using TMS.Core.Entities;
using TMS.Core.Interfaces;

namespace TMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _repository;

    public CommandsController(ICommandRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repository.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var command = await _repository.GetByIdAsync(id);
        return command is null ? NotFound() : Ok(command);
    }

    [HttpGet("terminal/{terminalId:int}")]
    public async Task<IActionResult> GetByTerminal(int terminalId) =>
        Ok(await _repository.GetByTerminalIdAsync(terminalId));

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending() =>
        Ok(await _repository.GetPendingCommandsAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Command command)
    {
        var created = await _repository.AddAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Command command)
    {
        if (id != command.Id) return BadRequest();
        await _repository.UpdateAsync(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = await _repository.GetByIdAsync(id);
        if (command is null) return NotFound();
        await _repository.DeleteAsync(command);
        return NoContent();
    }
}
