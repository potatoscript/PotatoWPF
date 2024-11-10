using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace PotatoWPF.Models
{
    

    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext")
        {
        }

        public DbSet<User> Users { get; set; }
    }


}
