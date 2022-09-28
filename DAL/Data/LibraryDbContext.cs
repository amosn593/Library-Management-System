using DOMAIN.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
           : base(options)
        {
        }

        public DbSet<Form> Form { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookSource> BookSource { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Borrowing> Borrowing { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Borrowing>()
                        .HasOne<Student>(s => s.CurrentStudent)
                        .WithMany(g => g.Borrowings)
                        .HasForeignKey(s => s.StudentID)
                        .OnDelete(DeleteBehavior.Cascade); // <= This entity has cascading behaviour on deletion

            modelBuilder.Entity<Borrowing>()
                        .HasOne<Book>(e => e.CurrentBook)
                        .WithMany(g => g.Borrowings)
                        .HasForeignKey(e => e.BookID)
                        .OnDelete(DeleteBehavior.Restrict); // <= This entity has restricted behaviour on deletion
        }
    }
}
