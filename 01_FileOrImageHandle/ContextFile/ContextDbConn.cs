using _01_FileOrImageHandle.Models;
using Microsoft.EntityFrameworkCore;

namespace _01_FileOrImageHandle.ContextFile
{
    public class ContextDbConn:DbContext
    {
        public ContextDbConn(DbContextOptions dbContext) : base(dbContext) { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
    }
}
