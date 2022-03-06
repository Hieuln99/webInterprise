using Microsoft.AspNetCore.Identity;

namespace WebEnterprise.Models
{
    public class CustomUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
