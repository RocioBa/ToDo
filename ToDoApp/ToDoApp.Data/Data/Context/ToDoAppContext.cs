
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data.Data.Configurations;
using ToDoApp.Data.Data.Models;
using static ToDoApp.Data.Data.Context.DbInitializer;


namespace ToDoApp.Data.Data.Context;
public class ToDoAppContext : DbContext
{
    #region [- Constructor -]
    public ToDoAppContext(DbContextOptions<ToDoAppContext> options) : base(options)
    {

    }
    #endregion

    #region [- Properties -]
    public DbSet<Item> Items { get; set; }
    #endregion

    #region [- Methods -]

    #region [- OnModelCreating -]
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("ToDo");
        builder.ApplyConfiguration(new ItemConfiguration());
        Initialize(builder);
    }
    #endregion

    #endregion
}
