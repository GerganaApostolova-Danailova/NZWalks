using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repisitories;
using System.Collections.Generic;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext dbContext;
    private readonly IRegionRepository regionRepository;
    private readonly IMapper mapper;

    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.regionRepository = regionRepository;
        this.mapper = mapper;
    }
    //GET: https://localhost:portnumber/api/regions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //Get Data from Database - Domain Models
        var regionsDomain = await regionRepository.GetAllAsync();
               
        //Map domain model to dto model
        //Return Dto back to client
        return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
    }

    //GET: https://localhost:portnumber/api/regions/{id}

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        var regionDomain = await regionRepository.GetByIdAsync(id);

        if (regionDomain == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<RegionDto>(regionDomain));
    }

    //Create Region
    //https://localhost:portnumber/api/regions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        //Map or Convert Dto to Domain Model
        var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

        //Use domain model to create region in database
        regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

        //Map or Convert Domain model to Dto
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
    }


    //Update Region
    //https://localhost:portnumber/api/regions/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        //Map or Convert Dto to Domain Model
        var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

        //Check if region exists in database
        regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
  
        if (regionDomainModel == null)
        {
            return NotFound();
        }
        //Map or Convert Domain model to Dto
        return Ok(mapper.Map<RegionDto>(regionDomainModel));
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    //[Authorize(Roles = "Writer,Reader")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
      var regionDomainModel = await regionRepository.DeleteAsync(id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<RegionDto>(regionDomainModel));
    }
}
