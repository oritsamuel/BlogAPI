using Microsoft.EntityFrameworkCore;
using BlogPost.Models;

namespace BlogPost.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<BlogPostModel> BlogPosts { get; set; }
    }
}


