using LastManagement.DTOs;
using LastManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LastManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LastNamesController : ControllerBase
{
    private readonly ILastNameService _lastNameService;

    public LastNamesController(ILastNameService lastNameService)
    {
        _lastNameService = lastNameService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] Guid? customerId = null, [FromQuery] string? status = null)
    {
        var result = await _lastNameService.GetAllAsync(page, pageSize, customerId, status);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _lastNameService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] LastNameDto dto)
    {
        var result = await _lastNameService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LastNameDto dto)
    {
        var result = await _lastNameService.UpdateAsync(id, dto);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _lastNameService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}