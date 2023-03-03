using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Data;
using TasksAPI.DTOs;
using TasksAPI.Models;

namespace TasksAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SuggestedLabelsController : ControllerBase
{
    private readonly ISuggestedLabelRepository _suggestedLabelRepository;
    private readonly IMapper _mapper;

    public SuggestedLabelsController(ISuggestedLabelRepository suggestedLabelRepository, IMapper mapper)
    {
        _suggestedLabelRepository = suggestedLabelRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<SuggestedLabelDto>> GetSuggestedLabelByIdAsync(int id)
    {
        var suggestedLabel = await _suggestedLabelRepository.GetSuggestedLabelByIdAsync(id);

        if (suggestedLabel == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<SuggestedLabelDto>(suggestedLabel));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SuggestedLabelDto>>> GetAllSuggestedLabelsAsync()
    {
        var suggestedLabels = await _suggestedLabelRepository.GetAllSuggestedLabelsAsync();

        if (suggestedLabels == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<SuggestedLabelDto>>(suggestedLabels));
    }

    [HttpPost]
    public async Task<ActionResult<int>> InsertSuggestedLabelAsync(SuggestedLabelDto suggestedLabelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var suggestedLabel = _mapper.Map<SuggestedLabel>(suggestedLabelDto);

        var suggestedLabelId = await _suggestedLabelRepository.InsertSuggestedLabelAsync(suggestedLabel);

        if (suggestedLabelId == 0)
        {
            return BadRequest();
        }

        return Ok(suggestedLabelId);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteSuggestedLabelAsync(int id)
    {
        if (await _suggestedLabelRepository.GetSuggestedLabelByIdAsync(id) == null)
        {
            return NotFound();
        }

        if (!await _suggestedLabelRepository.DeleteSuggestedLabelAsync(id))
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpPut]
    [Route("updatesuggestedlabel/{id}")]
    public async Task<ActionResult> UpdateSuggestedLabelAsync(int id, SuggestedLabelDto suggestedLabelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var suggestedLabel = _mapper.Map<SuggestedLabel>(suggestedLabelDto);

        if (id == suggestedLabel.Id)
        {
            return Unauthorized();
        }

        if (await _suggestedLabelRepository.GetSuggestedLabelByIdAsync(suggestedLabel.Id) == null)
        {
            return NotFound();
        }

        if (!await _suggestedLabelRepository.UpdateSuggestedLabelAsync(suggestedLabel))
        {
            return BadRequest();
        }

        return NoContent();
    }
}
