using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("appi/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<api.Models.AppUser> _userManager;

        public AccountController(UserManager<api.Models.AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Dtos.Account.RegisterRequestDto registerRequest)
        {
            try
            {

                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new api.Models.AppUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
              var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                    }
                    else
                    {
                        return Ok(new { Message = "User registered successfully." });
                    }
                }
                {
                    return BadRequest(result.Errors);
                }
        
                
            }catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            
        }
        
    }
}