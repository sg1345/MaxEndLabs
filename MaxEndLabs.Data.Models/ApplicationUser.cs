using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static MaxEndLabs.GCommon.EntityValidation.ApplicationUser;

namespace MaxEndLabs.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
