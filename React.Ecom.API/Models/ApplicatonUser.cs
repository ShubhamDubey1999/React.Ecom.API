using Microsoft.AspNetCore.Identity;

namespace React.Ecom.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
