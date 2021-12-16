using Microsoft.AspNetCore.Identity;

namespace AgencyApp.Models
{
    public class User : IdentityUser
    {
        public virtual Agent? Agent { get; set; }
        public virtual Client? Client { get; set; }
    }
}
