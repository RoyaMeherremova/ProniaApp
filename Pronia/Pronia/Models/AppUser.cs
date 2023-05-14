using Microsoft.AspNetCore.Identity;

namespace Pronia.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }   

        public string LastName { get; set; }

        public bool IsRememberMe { get; set; }

        public ICollection<ProductComment> ProductComments { get; set; }


        public ICollection<BlogComment> BlogComments { get; set; }

    }
}
