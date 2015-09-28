using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models
{
    public class Repository : DbContext
    {
        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<Department> Deparments { get; set; }
        public DbSet<CheckInOutRecord> CheckInOutRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set require something
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=10.9.1.100\SQLSERVER2012;Database=HRMSDB;Trusted_Connection=True;");
        //}
    }

    //public class BloggingContext : DbContext
    //{
    //    public DbSet<Blog> Blogs { get; set; }
    //    public DbSet<Post> Posts { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        // Make Blog.Url required
    //        modelBuilder.Entity<Blog>()
    //            .Property(b => b.Url)
    //            .Required();
    //    }
    //}

    //public class Blog
    //{
    //    public int BlogId { get; set; }
    //    public string Url { get; set; }

    //    public List<Post> Posts { get; set; }
    //}

    //public class Post
    //{
    //    public int PostId { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }

    //    public int BlogId { get; set; }
    //    public Blog Blog { get; set; }
    //}
}
