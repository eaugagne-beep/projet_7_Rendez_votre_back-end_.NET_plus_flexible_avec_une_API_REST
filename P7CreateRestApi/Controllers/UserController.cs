using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        
        private (string UserName, string? UserId, string Roles) GetCaller()
        {
            var userName = User?.Identity?.Name ?? "anonymous";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roles = string.Join(",", User.FindAll(ClaimTypes.Role).Select(r => r.Value));
            if (string.IsNullOrWhiteSpace(roles)) roles = "none";
            return (userName, userId, roles);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var (callerName, callerId, callerRoles) = GetCaller();
            _logger.LogInformation("GET /api/User by {UserName} ({UserId}) roles={Roles}",
                callerName, callerId, callerRoles);

            
            var users = _userManager.Users
                .Select(u => new { u.Id, u.UserName, u.FullName })
                .ToList();

            _logger.LogInformation("GET /api/User -> returned {Count} users", users.Count);
            return Ok(users);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var (callerName, callerId, callerRoles) = GetCaller();
            _logger.LogInformation("GET /api/User/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, callerName, callerId, callerRoles);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("GET /api/User/{Id} -> NotFound", id);
                return NotFound();
            }

            return Ok(new { user.Id, user.UserName, user.FullName });
        }

        public record UpdateUserDto(string? UserName, string? FullName);

       
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            var (callerName, callerId, callerRoles) = GetCaller();
            _logger.LogInformation("PUT /api/User/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, callerName, callerId, callerRoles);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("PUT /api/User/{Id} -> NotFound", id);
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrWhiteSpace(dto.FullName))
                user.FullName = dto.FullName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("PUT /api/User/{Id} -> BadRequest (Identity errors)", id);
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("PUT /api/User/{Id} -> Updated", id);
            return NoContent();
        }

        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var (callerName, callerId, callerRoles) = GetCaller();
            _logger.LogWarning("DELETE /api/User/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, callerName, callerId, callerRoles);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("DELETE /api/User/{Id} -> NotFound", id);
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("DELETE /api/User/{Id} -> BadRequest (Identity errors)", id);
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("DELETE /api/User/{Id} -> Deleted", id);
            return NoContent();
        }
    }
}
