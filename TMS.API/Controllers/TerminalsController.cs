using Microsoft.AspNetCore.Mvc;
using TMS.Core.Entities;
using TMS.Core.Enums;
using TMS.Core.Interfaces;

namespace TMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TerminalsController : ControllerBase
{
    private readonly ITerminalRepository _repository;

    public TerminalsController(ITerminalRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repository.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var terminal = await _repository.GetByIdAsync(id);
        return terminal is null ? NotFound() : Ok(terminal);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(TerminalStatus status) =>
        Ok(await _repository.GetByStatusAsync(status));

    [HttpPost]
    public async Task<IActionResult> Create(Terminal terminal)
    {
        var created = await _repository.AddAsync(terminal);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Terminal terminal)
    {
        if (id != terminal.Id) return BadRequest();
        await _repository.UpdateAsync(terminal);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var terminal = await _repository.GetByIdAsync(id);
        if (terminal is null) return NotFound();
        await _repository.DeleteAsync(terminal);
        return NoContent();
    }
}
