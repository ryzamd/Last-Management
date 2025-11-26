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
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
    {
        var result = await _purchaseOrderService.GetAllAsync(page, pageSize, status);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _purchaseOrderService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string requestorName;

        if (User.Identity?.IsAuthenticated == true)
        {
            requestorName = User.GetUsername();
            dto.RequestedBy = requestorName;
        }
        else
        {
            requestorName = dto.RequestedBy;
        }

        if (string.IsNullOrWhiteSpace(requestorName))
            return BadRequest(new { message = "RequestedBy name is required for guest users." });

        var result = await _purchaseOrderService.CreateAsync(dto, requestorName);
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