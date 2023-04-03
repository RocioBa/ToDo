

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ToDoApp.Data.Repository;
public class BaseRepository<T_Entity,T_Context> : IBaseRepository<T_Entity,T_Context> where T_Entity : class
                                                                                      where T_Context : DbContext
{

    #region [- Constructor -]

    private readonly ILogger _logger;
    public BaseRepository(T_Context context, ILoggerFactory logger)
    {
        Context = context;
        DbSet = Context.Set<T_Entity>();
        _logger = logger.CreateLogger("ToDoApp.Data(BaseRepository)");
    }
    #endregion

    #region [- Properties -]

    public T_Context Context { get; set; }

    public DbSet<T_Entity> DbSet { get; set; }

    #endregion

    #region [- Methods -]

    #region [- Add -]
    public void Add(T_Entity data)
    {
        DbSet.Add(data);
    }
    #endregion
   
    #region [- Update -]
    public void Update(T_Entity data)
    {
        DbSet.Update(data);
    }
    #endregion

    #region [- Remove -]
    public void Remove(T_Entity data)
    {
        DbSet.Remove(data);
    }
    #endregion

    #region [- GetAllAsync -]
    public async Task<List<T_Entity>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }
    #endregion

    #region [- SaveChangesAsync -]
    public async Task<bool> SaveChangesAsync(bool dispose)
    {
        bool result = true;
        using var transaction = Context.Database.BeginTransaction();

        try
        {
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            result = false;
            _logger.LogError(message: ex.GetType().Name, exception: ex);
        }
        finally
        {
            if (dispose)
                await Context.DisposeAsync();
        }

        return result;
    }
    #endregion
  
    #endregion

}
