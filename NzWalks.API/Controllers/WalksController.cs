using Microsoft.AspNetCore.Mvc;
using NzWalks.API.CustomActionFilters;
using NzWalks.API.Model.Domain;
using NzWalks.API.Model.DTO;
using NzWalks.API.Repositories;
using System.Linq.Expressions;

namespace NzWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalksRepository walksRepository;

        public WalksController(IWalksRepository walksRepository)
        {
            this.walksRepository = walksRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            var walkDomainModel = new Model.Domain.Walk
            {
                Id = addWalksRequestDto.Id,
                Description = addWalksRequestDto.Description,
                Name = addWalksRequestDto.Name,
                LengthInKm = addWalksRequestDto.LengthInKm,
                WalkImageUrl = addWalksRequestDto.WalkImageUrl,
                DifficultyId = addWalksRequestDto.DifficultyId,
                RegionId = addWalksRequestDto.RegionId
            };

            await walksRepository.CreateAsync(walkDomainModel);

            //map domain model to DTO
            var walkDto = new WalkDto
            {
                Id = walkDomainModel.Id,
                Description = walkDomainModel.Description,
                Name = walkDomainModel.Name,
                LengthInKm = walkDomainModel.LengthInKm,
                WalkImageUrl = walkDomainModel.WalkImageUrl,
                Difficulty = walkDomainModel.Difficulty == null ? null : new DifficultyDto
                {
                    Id = walkDomainModel.Difficulty.Id,
                    Name = walkDomainModel.Difficulty.Name
                },
                Region = walkDomainModel.Region == null ? null : new RegionDto
                {
                    Id = walkDomainModel.Region.Id,
                    Code = walkDomainModel.Region.Code,
                    Name = walkDomainModel.Region.Name,
                    RegionImageUrl = walkDomainModel.Region.RegionImageUrl
                }
            };

            return Ok(walkDto);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filteron, [FromQuery] string ? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber =1, [FromQuery] int pageSize=1000)
        {
            Expression<Func<Walk, bool>>? filter = null;
            Expression<Func<Walk, object>>? orderBy = null;

            if (!string.IsNullOrWhiteSpace(filteron) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (filteron.ToLower())
                {
                    case "name":
                        filter = w => w.Name.Contains(filterQuery);
                        break;
                    case "description":
                        filter = w => w.Description.Contains(filterQuery);
                        break;
                    case "lengthinkm":
                        if (double.TryParse(filterQuery, out var length))
                        {
                            filter = w => w.LengthInKm == length;
                        }
                        break;
                }
            }

            // Sorting logic
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        orderBy = w => w.Name;
                        break;
                    case "lengthinkm":
                        orderBy = w => w.LengthInKm;
                        break;
                }
            }

            var walksDomainModel = await walksRepository.GetAllAsync(filter, orderBy, isAscending, pageNumber, pageSize, w => w.Difficulty, w => w.Region);

            var walkDtos = walksDomainModel.Select(walk => new WalkDto
            {
                Id = walk.Id,
                Description = walk.Description,
                Name = walk.Name,
                LengthInKm = walk.LengthInKm,
                WalkImageUrl = walk.WalkImageUrl,
                Difficulty = walk.Difficulty == null ? null : new DifficultyDto
                {
                    Id = walk.Difficulty.Id,
                    Name = walk.Difficulty.Name
                },
                Region = walk.Region == null ? null : new RegionDto
                {
                    Id = walk.Region.Id,
                    Code = walk.Region.Code,
                    Name = walk.Region.Name,
                    RegionImageUrl = walk.Region.RegionImageUrl
                }
            }).ToList();

            return Ok(walkDtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walksRepository.GetByIdAsync(id, w => w.Difficulty, w => w.Region);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            var walkDto = new WalkDto
            {
                Id = walkDomainModel.Id,
                Description = walkDomainModel.Description,
                Name = walkDomainModel.Name,
                LengthInKm = walkDomainModel.LengthInKm,
                WalkImageUrl = walkDomainModel.WalkImageUrl,
                Difficulty = walkDomainModel.Difficulty == null ? null : new DifficultyDto
                {
                    Id = walkDomainModel.Difficulty.Id,
                    Name = walkDomainModel.Difficulty.Name
                },
                Region = walkDomainModel.Region == null ? null : new RegionDto
                {
                    Id = walkDomainModel.Region.Id,
                    Code = walkDomainModel.Region.Code,
                    Name = walkDomainModel.Region.Name,
                    RegionImageUrl = walkDomainModel.Region.RegionImageUrl
                }
            };
            return Ok(walkDto);
        }

        [HttpPut("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalksRequestDto updateWalksRequestDto)
        {
            if (updateWalksRequestDto == null)
            {
                return BadRequest("Invalid walk data.");
            }
            var walkDomainModel = new Model.Domain.Walk
            {
                Id = id,
                Description = updateWalksRequestDto.Description,
                Name = updateWalksRequestDto.Name,
                LengthInKm = updateWalksRequestDto.LengthInKm,
                WalkImageUrl = updateWalksRequestDto.WalkImageUrl,
                DifficultyId = updateWalksRequestDto.DifficultyId,
                RegionId = updateWalksRequestDto.RegionId
            };
            var updatedWalk = await walksRepository.UpdateAsync(id, walkDomainModel);
            if (updatedWalk == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            var walkDto = new WalkDto
            {
                Id = updatedWalk.Id,
                Description = updatedWalk.Description,
                Name = updatedWalk.Name,
                LengthInKm = updatedWalk.LengthInKm,
                WalkImageUrl = updatedWalk.WalkImageUrl,
                Difficulty = updatedWalk.Difficulty == null ? null : new DifficultyDto
                {
                    Id = updatedWalk.Difficulty.Id,
                    Name = updatedWalk.Difficulty.Name
                },
                Region = updatedWalk.Region == null ? null : new RegionDto
                {
                    Id = updatedWalk.Region.Id,
                    Code = updatedWalk.Region.Code,
                    Name = updatedWalk.Region.Name,
                    RegionImageUrl = updatedWalk.Region.RegionImageUrl
                }
            };
            return Ok(walkDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalk = await walksRepository.DeleteAsync(id);
            if (deletedWalk == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            var walkDto = new WalkDto
            {
                Id = deletedWalk.Id,
                Description = deletedWalk.Description,
                Name = deletedWalk.Name,
                LengthInKm = deletedWalk.LengthInKm,
                WalkImageUrl = deletedWalk.WalkImageUrl,
                Difficulty = deletedWalk.Difficulty == null ? null : new DifficultyDto
                {
                    Id = deletedWalk.Difficulty.Id,
                    Name = deletedWalk.Difficulty.Name
                },
                Region = deletedWalk.Region == null ? null : new RegionDto
                {
                    Id = deletedWalk.Region.Id,
                    Code = deletedWalk.Region.Code,
                    Name = deletedWalk.Region.Name,
                    RegionImageUrl = deletedWalk.Region.RegionImageUrl
                }
            };
            return Ok(walkDto);
        }
    }
}
