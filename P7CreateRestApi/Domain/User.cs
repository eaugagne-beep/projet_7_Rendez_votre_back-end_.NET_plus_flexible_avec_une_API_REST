using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
