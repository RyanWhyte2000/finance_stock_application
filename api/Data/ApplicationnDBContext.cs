using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace api.Data
{
    public class ApplicationnDBContext : DbContext
    {
        public ApplicationnDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<api.Models.Stock> Stocks { get; set; }
        public DbSet<api.Models.Comment> Comments { get; set; }
        
        
    }
}