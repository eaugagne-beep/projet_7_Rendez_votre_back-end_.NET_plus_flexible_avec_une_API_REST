using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // ADMIN endpoints
        

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("GET /api/User called by Admin={User}", User.Identity?.Name ?? "unknown");

            var users = _userManager.Users
                .Select(u => new { u.Id, u.UserName, u.FullName })
                .ToList();

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogInformation("GET /api/User/{Id} called by Admin={User}", id, User.Identity?.Name ?? "unknown");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.FullName,
                Roles = roles
            });
        }

        

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            _logger.LogInformation("PUT /api/User/{Id} called by Admin={User}", id, User.Identity?.Name ?? "unknown");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (dto.FullName != null)
                user.FullName = dto.FullName;

            var updateRes = await _userManager.UpdateAsync(user);
            if (!updateRes.Succeeded) return BadRequest(updateRes.Errors);

            // update role (si fourni)
            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                if (!await _roleManager.RoleExistsAsync(dto.Role))
                    await _roleManager.CreateAsync(new IdentityRole(dto.Role));

                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);

                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            _logger.LogInformation("User updated: Id={Id}", id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("DELETE /api/User/{Id} called by Admin={User}", id, User.Identity?.Name ?? "unknown");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var res = await _userManager.DeleteAsync(user);
            if (!res.Succeeded) return BadRequest(res.Errors);

            _logger.LogInformation("User deleted: Id={Id}", id);
            return NoContent();
        }

      
    }
}
