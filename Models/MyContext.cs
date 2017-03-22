using Microsoft.EntityFrameworkCore;
 
namespace weddingPlanner.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Wedding> Weddings {get;set;}

        public DbSet<Reserve> Reserves {get;set;}
    }
}
