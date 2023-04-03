
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data.Data.Models;
using ToDoApp.Data.Utils.Enums;

namespace ToDoApp.Data.Data.Context;
internal static class DbInitializer
{
    #region [- Methods -]

    #region [- Initialize -]
    public static void Initialize(ModelBuilder builder)
    {
        #region [- Items -]

        List<Item> items = new()
        {
           new Item(){ Id=1,Title="Microservices",Description="Reading the second microservice article and creating a practical project (Mehmet Ozkaya)",Priority=Priority.High },
           new Item(){ Id=2,Title="API Problem",Description="Solving authentication api problem",Priority=Priority.Normal },
           new Item(){ Id=3,Title="Self Study",Description="Watching the training video about ChatGPT",Priority=Priority.Low }
        };
        #endregion

        builder.Entity<Item>().HasData(items);  
    }
    #endregion

    #endregion
}
