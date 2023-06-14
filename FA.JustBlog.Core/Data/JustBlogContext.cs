using FA.JustBlog.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FA.JustBlog.Core.Data
{
    public class JustBlogContext : IdentityDbContext
    {
        public JustBlogContext(DbContextOptions<JustBlogContext> options) : base(options)
        {
        }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<PostTagMap> PostTagMap { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Published).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_PostsCategoryId");
               

            });
            modelBuilder.Entity<PostTagMap>(entity =>
            {

                entity.HasKey(pt => new { pt.PostId, pt.TagId });
                entity.HasOne(x => x.Post).WithMany(y => y.PostTagMap).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Tag).WithMany(y => y.PostTagMap).HasForeignKey(x => x.TagId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(n => n.Name).HasMaxLength(255);
                entity.Property(c=>c.CommentHeader).HasMaxLength(255);
                entity.Property(c => c.CommentText).HasColumnType("ntext");
                entity.Property(e => e.CommentTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
                entity.HasOne(p => p.Post)
                      .WithMany(c => c.Comments)
                      .HasForeignKey(p => p.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
                  
            });

            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<IdentityUserLogin<string>>(x =>
            //{
            //    x.HasKey(p => new {p.UserId});

            //});
            
            modelBuilder.Seed();
            
        }
        public override EntityEntry Add(object entity)
        {
            return base.Add(entity);    
        }
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return base.Add(entity);
        }

    }
}