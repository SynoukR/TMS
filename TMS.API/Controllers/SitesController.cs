using Microsoft.AspNetCore.Mvc;
using TMS.API.DTOs;
using TMS.Core.Entities;
using TMS.Core.Interfaces;

namespace TMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SitesController(ISiteRepository siteRepo, IWarehouseRepository warehouseRepo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // GetAllWithTerminalCountAsync inclut les navigations Terminals et Warehouse en une seule requête (pas de N+1)
        var sites = await siteRepo.GetAllWithTerminalCountAsync();

        var dtos = sites.Select(s => new SiteDto(
            s.Id, s.Name, s.Address, s.IsActive,
            s.WarehouseId,
            s.Warehouse?.Name ?? string.Empty,
            s.Terminals.Count,
            s.CreatedAt
        ));

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var site = await siteRepo.GetWithTerminalsAsync(id);
        if (site is null) return NotFound();

        var dto = new SiteDetailDto(
            site.Id, site.Name, site.Address, site.IsActive,
            site.WarehouseId, site.Warehouse.Name, site.CreatedAt,
            site.Terminals.Select(t => new TerminalSummaryDto(
                t.Id, t.Name, t.SerialNumber, t.Model, t.IpAddress,
                t.Status, t.EquipmentStatus, t.LastSeen
            ))
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSiteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Le nom du site est obligatoire.");

        var warehouse = await warehouseRepo.GetByIdAsync(dto.WarehouseId);
        if (warehouse is null) return BadRequest("Warehouse introuvable.");

        var site = new Site
        {
            Name = dto.Name.Trim(),
            Address = dto.Address?.Trim(),
            IsActive = dto.IsActive,
            WarehouseId = dto.WarehouseId
        };

        await siteRepo.AddAsync(site);
        return CreatedAtAction(nameof(GetById), new { id = site.Id }, site.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateSiteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Le nom du site est obligatoire.");

        var site = await siteRepo.GetByIdAsync(id);
        if (site is null) return NotFound();

        var warehouse = await warehouseRepo.GetByIdAsync(dto.WarehouseId);
        if (warehouse is null) return BadRequest("Warehouse introuvable.");

        site.Name = dto.Name.Trim();
        site.Address = dto.Address?.Trim();
        site.IsActive = dto.IsActive;
        site.WarehouseId = dto.WarehouseId;

        await siteRepo.UpdateAsync(site);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var site = await siteRepo.GetWithTerminalsAsync(id);
        if (site is null) return NotFound();

        // On déplace les terminaux hors du site avant suppression (SetNull géré par EF,
        // mais on s'assure que la suppression est propre)
        await siteRepo.DeleteAsync(site);
        return NoContent();
    }
}
