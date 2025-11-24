using LastManagement.DTOs;
using LastManagement.Extensions;
using LastManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LastManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryStocksController : ControllerBase
{
    private readonly IInventoryStockService _inventoryStockService;

    public InventoryStocksController(IInventoryStockService inventoryStockService)
    {
        _inventoryStockService = inventoryStockService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] Guid? locationId = null)
    {
        var result = await _inventoryStockService.GetAllAsync(page, pageSize, locationId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _inventoryStockService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateOrUpdate([FromBody] InventoryStockDto dto)
    {
        var result = await _inventoryStockService.CreateOrUpdateAsync(dto);
        return Ok(result);
    }

    [HttpPost("{id}/adjust")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdjustStock(Guid id, [FromBody] AdjustStockRequest request)
    {
        var username = User.GetUsername();
        var success = await _inventoryStockService.AdjustStockAsync(id, request.QuantityChange,
            request.MovementType, request.Reason ?? "", username);

        if (!success)
            return NotFound();

        return NoContent();
    }
}

public record AdjustStockRequest(int QuantityChange, string MovementType, string? Reason);