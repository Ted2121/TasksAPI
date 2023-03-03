using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Data;
using TasksAPI.DTOs;

namespace TasksAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PinnedTasksController : ControllerBase
{
    IPinnedTaskRepository _pinnedTaskRepository;

    public PinnedTasksController(IPinnedTaskRepository pinnedTaskRepository)
    {
        _pinnedTaskRepository = pinnedTaskRepository;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<PinnedTaskDto>> GetPinnedTaskByIdAsync(int id)
    {
        var pinnedTask = await _pinnedTaskRepository.GetPinnedTaskByIdAsync(id);

        if(pinnedTask == null)
        { 
        return NotFound();
        }

        return Ok(pinnedTask);
    }
}
