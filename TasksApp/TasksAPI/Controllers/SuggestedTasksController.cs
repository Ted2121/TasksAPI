using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Data;
using TasksAPI.DTOs;
using TasksAPI.Models;

namespace TasksAPI.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class SuggestedTasksController : ControllerBase
{
    private readonly ISuggestedTaskRepository _suggestedTaskRepository;
    private readonly IMapper _mapper;

    public SuggestedTasksController(ISuggestedTaskRepository suggestedTaskRepository, IMapper mapper)
    {
        _suggestedTaskRepository = suggestedTaskRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<SuggestedTaskDto>> GetSuggestedTaskByIdAsync(int id)
    {
        var suggestedTask = await _suggestedTaskRepository.GetSuggestedTaskByIdAsync(id);

        if (suggestedTask == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<SuggestedTaskDto>(suggestedTask));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SuggestedTaskDto>>> GetAllSuggestedTasksAsync()
    {
        var suggestedTasks = await _suggestedTaskRepository.GetAllSuggestedTasksAsync();

        if (suggestedTasks == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<SuggestedTaskDto>>(suggestedTasks));
    }

    [HttpPost]
    public async Task<ActionResult<int>> InsertSuggestedTaskAsync(SuggestedTaskDto suggestedTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var suggestedTask = _mapper.Map<SuggestedTask>(suggestedTaskDto);

        var suggestedTaskId = await _suggestedTaskRepository.InsertSuggestedTaskAsync(suggestedTask);

        if (suggestedTaskId == 0)
        {
            return BadRequest();
        }

        return Ok(suggestedTaskId);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteSuggestedTaskAsync(int id)
    {
        if (await _suggestedTaskRepository.GetSuggestedTaskByIdAsync(id) == null)
        {
            return NotFound();
        }

        if (!await _suggestedTaskRepository.DeleteSuggestedTaskAsync(id))
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpPut]
    [Route("updatesuggestedtask/{id}")]
    public async Task<ActionResult> UpdateSuggestedTaskAsync(int id, SuggestedTaskDto suggestedTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var suggestedTask = _mapper.Map<SuggestedTask>(suggestedTaskDto);

        if (id == suggestedTask.Id)
        {
            return Unauthorized();
        }

        if (await _suggestedTaskRepository.GetSuggestedTaskByIdAsync(suggestedTask.Id) == null)
        {
            return NotFound();
        }

        if (!await _suggestedTaskRepository.UpdateSuggestedTaskAsync(suggestedTask))
        {
            return BadRequest();
        }

        return NoContent();
    }
}
