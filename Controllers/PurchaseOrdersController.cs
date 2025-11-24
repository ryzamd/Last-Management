using LastManagement.DTOs;
using LastManagement.Extensions;
using LastManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LastManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
    {
        var result = await _purchaseOrderService.GetAllAsync(page, pageSize, status);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _purchaseOrderService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderDto dto)
    {
        var username = User.GetUsername();
        var result = await _purchaseOrderService.CreateAsync(dto, username);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id}/confirm")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        var username = User.GetUsername();
        var success = await _purchaseOrderService.ConfirmOrderAsync(id, username);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/deny")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deny(Guid id, [FromBody] DenyOrderRequest request)
    {
        var username = User.GetUsername();
        var success = await _purchaseOrderService.DenyOrderAsync(id, username, request.Reason);
        if (!success)
            return NotFound();

        return NoContent();
    }
}

public record DenyOrderRequest(string Reason);