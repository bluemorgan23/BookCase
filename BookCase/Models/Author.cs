using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookCase.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string PenName { get; set; }

        public string Genre { get; set; }

        public List<Book> Books { get; set; }

        // REad only property, can only get
        // Not mapped gives us the ability to use this property 
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        
    }
}
