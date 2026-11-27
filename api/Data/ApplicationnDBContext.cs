using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace api.Data
{
    public class ApplicationnDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationnDBContext(DbContextOptions dbContextOptions) 
        : base(dbContextOptions)
        {
            
        }

        public DbSet<api.Models.Stock> Stocks { get; set; }
        public DbSet<api.Models.Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "9a3412a8-c45b-4355-b037-329bd8af6d0a",
                    ConcurrencyStamp = "75a83872-f52c-4c50-81b7-9fdb3a104a36",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "b924feee-16bf-4321-b2c2-dd43ba028439",
                    ConcurrencyStamp = "cdcb3a10-b7a0-40ba-aad0-83d5c12045e2",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
        
        
    }
}
