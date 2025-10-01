using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NzWalks.API.Model.DTO;

namespace NzWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public UserManager<IdentityUser> userManager { get; }

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityResult = await userManager.CreateAsync(new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            }, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                var user = await userManager.FindByNameAsync(registerRequestDto.Username);
                if (user != null && registerRequestDto.Roles != null)
                {
                    await userManager.AddToRolesAsync(user, registerRequestDto.Roles);
                }
            }

            return identityResult.Succeeded ? Ok("User registered successfully.") : BadRequest(identityResult.Errors);
        }
    }
}
