// User Model
// identity user is a user class that identity fm gives us. Has built in properties. Does NOT have first and last name.

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookCase.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        // REad only property, can only get
        // Not mapped gives us the ability to use this property 
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
