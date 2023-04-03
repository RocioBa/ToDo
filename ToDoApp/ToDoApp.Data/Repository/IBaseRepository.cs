
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Data.Repository;
public interface IBaseRepository<T_Entity,T_Context> where T_Entity : class 
                                                     where T_Context : DbContext
{
    #region [- Properties -]

    T_Context Context { get; set; }

    DbSet<T_Entity> DbSet { get; set; }

    #endregion

    #region [- Methods -] 
    void Add(T_Entity data);
    void Update(T_Entity data);  
    void Remove(T_Entity data);
    Task<List<T_Entity>> GetAllAsync();
    Task<bool> SaveChangesAsync(bool dispose);
  
    #endregion

}
