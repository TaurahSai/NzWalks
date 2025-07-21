using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;

namespace NzWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext dbContext;

        public RegionsController(NzWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await dbContext.Regions.ToListAsync();

            var regionsDto = new List<Model.DTO.RegionDto>();

            foreach (var region in regions)
            {
                regionsDto.Add(new Model.DTO.RegionDto
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id); // Use FirstOrDefaultAsync instead of FirstOrDefault

            if (region == null)
            {
                return NotFound();
            }

            var regionDto = new Model.DTO.RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.DTO.AddRegionRequestDto addRegionRequestDto)
        {
            if (addRegionRequestDto == null)
            {
                return BadRequest("Region data is required.");
            }
            var regionDomainModel = new Model.Domain.Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };
            await dbContext.Regions.AddAsync(regionDomainModel); 
            await dbContext.SaveChangesAsync();

            var regionDto = new Model.DTO.RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Model.DTO.UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (updateRegionRequestDto == null)
            {
                return BadRequest("Region data is required.");
            }
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return NotFound();
            }
            region.Code = updateRegionRequestDto.Code;
            region.Name = updateRegionRequestDto.Name; 
            region.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            var regionDto = new Model.DTO.RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();
            var regionDto = new Model.DTO.RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}