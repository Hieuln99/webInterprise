using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Models;

namespace WebEnterprise.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserLikePost> UserLikePosts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            var passwordHasher = new PasswordHasher<CustomUser>();
            CustomUser user = new CustomUser()
            {
                Id = "1",
                UserName = "Admin",
                Email = "admin@gmail.com",
                NormalizedUserName = "admin",
                PasswordHash = passwordHasher.HashPassword(null, "Abc@12345"),
                LockoutEnabled = true,
                EmailConfirmed = true,
            };


            builder.Entity<CustomUser>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "1", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "admin" },
                new IdentityRole() { Id = "2", Name = "Assurance", ConcurrencyStamp = "2", NormalizedName = "assurance" },
                new IdentityRole() { Id = "3", Name = "Coordinator", ConcurrencyStamp = "3", NormalizedName = "coordinator" },
                new IdentityRole() { Id = "4", Name = "Staff", ConcurrencyStamp = "4", NormalizedName = "staff" }
                );
        }
        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { RoleId = "1", UserId = "1" });
        }
    }
}