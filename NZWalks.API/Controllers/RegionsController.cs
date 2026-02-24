using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Collections.Generic;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext dbContext;
    public RegionsController(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    //GET: https://localhost:portnumber/api/regions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regionsDomain = await dbContext.Regions.ToListAsync();
       
        var regionsDto = new List<RegionDto>();

       foreach (var regionDomain in regionsDomain)
        {
            regionsDto.Add(new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            });
        }


        return Ok(regionsDto);
    }

    //GET: https://localhost:portnumber/api/regions/{id}

    [HttpGet]
    [Route("{id:Guid}")]
    public IActionResult GetById([FromRoute]Guid id)
    {
       

        var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if (regionDomain == null)
        {
            return NotFound();
        }

        var regionDto = new RegionDto()
        {
            Id = regionDomain.Id,
            Code = regionDomain.Code,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl
        };

        return Ok(regionDto);
    }

    //Create Region
    //https://localhost:portnumber/api/regions
    [HttpPost]

    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = new Region
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        dbContext.Regions.Add(regionDomainModel);
        dbContext.SaveChanges();

        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };
        return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
    }


    //Update Region
    //https://localhost:portnumber/api/regions/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
        if (regionDomainModel == null)
        {
            return NotFound();
        }
        regionDomainModel.Code = updateRegionRequestDto.Code;
        regionDomainModel.Name = updateRegionRequestDto.Name;
        regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
        dbContext.SaveChanges();
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    //[Authorize(Roles = "Writer,Reader")]
    public IActionResult Delete([FromRoute] Guid id)
    {
       var regionDomainModel= dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        dbContext.Regions.Remove(regionDomainModel);
        dbContext.SaveChanges();

        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };
        return Ok(regionDto);
    }
}
