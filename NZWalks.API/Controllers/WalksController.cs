using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repisitories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    public WalksController(IMapper mapper, IWalkRepository walkRepository)
    {
        this.mapper = mapper;
        this.walkRepository = walkRepository;
    }

    public IMapper mapper { get; }
    public IWalkRepository walkRepository { get; }

    //Create Walks
    //Post: /api/walks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        //Map DTO to Domain Model

        var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

        await walkRepository.CreateAsync(walkDomainModel);

        //Map domain model to dto

        return Ok(mapper.Map<WalkDto>(walkDomainModel));
    }

    //Get Walks
    //Get: /api/walks
    [HttpGet]

    public async Task<IActionResult> GetAll()
    {
        var walksDomainModel = await walkRepository.GetAllAsync();
        return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
    }

    //Get Walks by Id
    //Get: /api/walks/{id}

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        var walkDomainModel = await walkRepository.GetByIdAsync(id);
        if (walkDomainModel == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<WalkDto>(walkDomainModel));
    }

    //Update Walks
    //Put: /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
            
            var updatedWalkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
            if (updatedWalkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(updatedWalkDomainModel));
    }

    //Delete Walks
    //Delete: /api/walks/{id}
    [HttpDelete]
    [Route("{id:Guid}")]

    public async Task<IActionResult> Delete([FromRoute]Guid id)
    {
        var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
        if (deletedWalkDomainModel == null)
        {
            return NotFound();
        }
        //Delete the walk
        //await walkRepository.DeleteAsync(id);
        return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
    }
}
