using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public class LibraryDbContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=localhost;Database=librarydb;Username=user;Password=password");
        options.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // author
        modelBuilder.Entity<Author>()
            .HasKey(a => a.Id);
        
        modelBuilder.Entity<Author>()
            .Property(a => a.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Author>()
            .HasMany(b => b.Books)
            .WithMany(b => b.Authors);
        
        // book
        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);
        
        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .HasMaxLength(100)
            .IsRequired();
        
        modelBuilder.Entity<Book>()
            .Property(b => b.Isbn)
            .HasMaxLength(100)
            .IsRequired();
        
        modelBuilder.Entity<Book>()
            .Property(b => b.PublishDate)
            .HasColumnType("date");
        
        // member
        modelBuilder.Entity<Member>()
            .HasKey(m => m.Id);
        
        modelBuilder.Entity<Member>()
            .Property(m => m.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        modelBuilder.Entity<Member>()
            .Property(m => m.Email)
            .HasMaxLength(100)
            .IsRequired();
        
        modelBuilder.Entity<Member>()
            .Property(m => m.PhoneNumber)
            .HasMaxLength(100)
            .IsRequired();
        
        modelBuilder.Entity<Member>()
            .HasMany(m => m.Loans)
            .WithOne(l => l.Member)
            .HasForeignKey(l => l.MemberId);
        
        // loan
        modelBuilder.Entity<Loan>()
            .HasKey(l => l.Id);
        
        modelBuilder.Entity<Loan>()
            .Property(l => l.StartDate)
            .HasColumnType("date");
        
        modelBuilder.Entity<Loan>()
            .Property(l => l.EndDate)
            .HasColumnType("date");

        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithMany()
            .HasForeignKey(l => l.BookId)
            .IsRequired();
    }
}