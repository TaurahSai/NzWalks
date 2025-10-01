using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalks.API.CustomActionFilters;
using NzWalks.API.Repositories;

namespace NzWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionsRepository regionRepository;

        public RegionsController(IRegionsRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();

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
            var region = await regionRepository.GetByIdAsync(id);

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
        [ValidateModel]
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
            await regionRepository.CreateAsync(regionDomainModel); 

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
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Model.DTO.UpdateRegionRequestDto updateRegionRequestDto)
        { 
            if (updateRegionRequestDto == null)
            {
                return BadRequest("Region data is required.");
            }
            var region = await regionRepository.UpdateAsync(id, new Model.Domain.Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            });

            if(region == null)
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

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);
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
    }
}