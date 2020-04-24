using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NewPhotoFunction
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=aspnet-WebApplication;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        public DbSet<Photo> Photos { get; set; }
    }
}
