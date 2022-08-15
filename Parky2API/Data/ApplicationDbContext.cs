using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Parky2API.Model;
using Parky2API.Model.Dtos;

namespace Parky2API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<NationalPark> NationalPark { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
