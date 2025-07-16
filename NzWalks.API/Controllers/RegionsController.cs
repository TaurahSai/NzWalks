using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll()
        {
            var regions = dbContext.Regions.ToList();
            return Ok(regions);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {

            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }
    }
}
