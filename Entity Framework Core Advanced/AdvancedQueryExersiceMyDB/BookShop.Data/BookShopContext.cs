using BookShop.Data.Common;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;


namespace BookShop.Data;

public class BookShopContext:DbContext
{
	public BookShopContext()
	{

	}
	public BookShopContext(DbContextOptions options):base(options)
	{

	}

    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookCategory> BooksCategories { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

            optionsBuilder.UseSqlServer(DbConfig.ConnectionString); 
                }

        base.OnConfiguring(optionsBuilder);
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity <BookCategory> (entity =>
        {
            entity.HasKey(ps => new { ps.BookId , ps.CategoryId});
        });

    }

}

