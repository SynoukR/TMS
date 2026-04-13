using Microsoft.AspNetCore.Mvc;
using TMS.API.DTOs;
using TMS.Core.Entities;
using TMS.Core.Interfaces;

namespace TMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController(IWarehouseRepository warehouseRepo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var warehouses = await warehouseRepo.GetAllWithSitesAsync();

        var dtos = warehouses.Select(w => new WarehouseDto(
            w.Id, w.Name, w.Address, w.IsActive,
            w.Sites.Count,
            w.SpareTerminals.Count,
            w.CreatedAt
        ));

        return Ok(dtos);
    }

    // Endpoint enrichi pour la vue expandable : Sites avec leurs terminaux inclus
    [HttpGet("with-sites")]
    public async Task<IActionResult> GetAllWithSites()
    {
        var warehouses = await warehouseRepo.GetAllWithSitesAsync();

        var dtos = warehouses.Select(w => new WarehouseDetailDto(
            w.Id, w.Name, w.Address, w.IsActive, w.CreatedAt,
            w.Sites.Select(s => new SiteDto(
                s.Id, s.Name, s.Address, s.IsActive,
                s.WarehouseId, w.Name,
                s.Terminals.Count,
                s.CreatedAt
            )),
            w.SpareTerminals.Select(t => new TerminalSummaryDto(
                t.Id, t.Name, t.SerialNumber, t.Model, t.IpAddress,
                t.Status, t.EquipmentStatus, t.LastSeen
            ))
        ));

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var warehouse = await warehouseRepo.GetWithSitesAndTerminalsAsync(id);
        if (warehouse is null) return NotFound();

        var dto = new WarehouseDetailDto(
            warehouse.Id, warehouse.Name, warehouse.Address, warehouse.IsActive, warehouse.CreatedAt,
            warehouse.Sites.Select(s => new SiteDto(
                s.Id, s.Name, s.Address, s.IsActive,
                s.WarehouseId, warehouse.Name,
                s.Terminals.Count,
                s.CreatedAt
            )),
            warehouse.SpareTerminals.Select(t => new TerminalSummaryDto(
                t.Id, t.Name, t.SerialNumber, t.Model, t.IpAddress,
                t.Status, t.EquipmentStatus, t.LastSeen
            ))
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Le nom de la warehouse est obligatoire.");

        var warehouse = new Warehouse
        {
            Name = dto.Name.Trim(),
            Address = dto.Address?.Trim(),
            IsActive = dto.IsActive
        };

        await warehouseRepo.AddAsync(warehouse);
        return CreatedAtAction(nameof(GetById), new { id = warehouse.Id }, warehouse.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateWarehouseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Le nom de la warehouse est obligatoire.");

        var warehouse = await warehouseRepo.GetByIdAsync(id);
        if (warehouse is null) return NotFound();

        warehouse.Name = dto.Name.Trim();
        warehouse.Address = dto.Address?.Trim();
        warehouse.IsActive = dto.IsActive;

        await warehouseRepo.UpdateAsync(warehouse);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var warehouse = await warehouseRepo.GetWithSitesAndTerminalsAsync(id);
        if (warehouse is null) return NotFound();

        // On bloque la suppression si des sites sont encore rattachés (cohérence avec DeleteBehavior.Restrict)
        if (warehouse.Sites.Count > 0)
            return BadRequest($"Impossible de supprimer cette warehouse : {warehouse.Sites.Count} site(s) y sont rattaché(s). Supprimez ou déplacez les sites d'abord.");

        await warehouseRepo.DeleteAsync(warehouse);
        return NoContent();
    }
}
