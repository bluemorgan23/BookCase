using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookCase.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(13)]
        [MinLength(10)]
        public string Isbn { get; set; }
     
        public string Genre { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        public int AuthorId { get; set; }
        [Required]
        public Author Author { get; set; }

        // UserId's in Identity FW are strings because they are GUIDs
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public ApplicationUser Owner { get; set; }

    }
}
