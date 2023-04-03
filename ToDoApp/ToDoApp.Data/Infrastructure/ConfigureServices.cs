

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Data.Data.Context;
using ToDoApp.Data.Repository;

namespace ToDoApp.Data.Infrastructure;
public static class ConfigureServices
{
    #region [- Methods -]

    #region [- ConfigureDbContext -]
    public static void ConfigureDbContext(this IServiceCollection services, string connection)
    {
        services.AddDbContext<ToDoAppContext>(p => p.UseSqlServer(connection, q => { q.MigrationsHistoryTable("Migrations","ToDo"); }));
        services.AddScoped(typeof(IBaseRepository<,>),typeof(BaseRepository<,>));
    }
    #endregion

    #endregion
}
