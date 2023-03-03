using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Data;
using TasksAPI.DTOs;
using TasksAPI.Models;

namespace TasksAPI.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class PinnedTasksController : ControllerBase
{
    private readonly IPinnedTaskRepository _pinnedTaskRepository;
    private readonly IMapper _mapper;

    public PinnedTasksController(IPinnedTaskRepository pinnedTaskRepository, IMapper mapper)
    {
        _pinnedTaskRepository = pinnedTaskRepository;
        _mapper = mapper;
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

        return Ok(_mapper.Map<PinnedTaskDto>(pinnedTask));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PinnedTaskDto>>> GetAllPinnedTasksAsync()
    {
        var pinnedTasks = await _pinnedTaskRepository.GetAllPinnedTasksAsync();

        if(pinnedTasks == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<PinnedTaskDto>>(pinnedTasks));
    }

    [HttpPost]
    public async Task<ActionResult<int>> InsertPinnedTaskAsync(PinnedTaskDto pinnedTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pinnedTask = _mapper.Map<PinnedTask>(pinnedTaskDto);

        var pinnedTaskId = await _pinnedTaskRepository.InsertPinnedTaskAsync(pinnedTask);

        if (pinnedTaskId == 0)
        {
            return BadRequest();
        }

        return Ok(pinnedTaskId);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeletePinnedTaskAsync(int id)
    {
        if(await _pinnedTaskRepository.GetPinnedTaskByIdAsync(id) == null)
        {
            return NotFound();
        }

        if(! await _pinnedTaskRepository.DeletePinnedTaskAsync(id))
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpPut]
    [Route("updatepinnedtask/{id}")]
    public async Task<ActionResult> UpdatePinnedTaskAsync(int id, PinnedTaskDto pinnedTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pinnedTask = _mapper.Map<PinnedTask>(pinnedTaskDto);

        if(id == pinnedTask.Id)
        {
            return Unauthorized();
        }

        if (await _pinnedTaskRepository.GetPinnedTaskByIdAsync(pinnedTask.Id) == null)
        {
            return NotFound();
        }

        if(! await _pinnedTaskRepository.UpdatePinnedTaskAsync(pinnedTask))
        {
            return BadRequest();
        }

        return NoContent();
    }

}
