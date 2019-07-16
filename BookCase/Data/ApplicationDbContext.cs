using System;
using System.Collections.Generic;
using System.Text;
using BookCase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookCase.Data
{ ///pass in ApplicationUser to the inherited class. Takes no parameter out of the box
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }

        // This is run whenever you do a migration. This will feed the migration. The first thing you have to do is call the parent/base version of the class `base.OnModelCreating(modelBuilder)`

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create a new user for Identity Framework
            ApplicationUser user = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            var author = new Author()
            {
                Id = 1,
                FirstName = "Stephen",
                LastName = "King",
                PenName = "Richard Bachman",
                Genre = "Horror"
            };

            modelBuilder.Entity<Author>().HasData(author);

            var book = new Book()
            {
                Id = 1,
                Isbn = "1234512345",
                Title = "It",
                AuthorId = author.Id,
                PublishDate = new DateTime(1988, 4, 1),
                OwnerId = user.Id
            };

            modelBuilder.Entity<Book>().HasData(book);

        }
    }
}

// A migration is code that is generated that will go and build the structure of our database. In order to build a migration Tools > NuGet Package Manager > NuGet PM Console. 'Add-Migration Initial' Initial = Migration Name
// This will create the Migration folder with the file inside with Up and Down methods.
// Then go back to console and 'Update-Database'