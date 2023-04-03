using AutoMapper;
using ToDoApp.Data.Data.Models;

namespace ToDoApp.API.DTOs.Requests.Mappers;

public class ItemMapper : Profile
{
    #region [- Constructor -]
    public ItemMapper()
    {
        CreateMap<ItemDTO,Item>();

        /*
           This part of code is just for example 
        
           - If we want to do some additional operations on property 
           .ForMember(model => model.Title,
                      dto => dto.MapFrom(p => MakeUpperCase(p.Title)))

           - For Bidirectional Mapping
           .ReverseMap(); 
        */
    }
    #endregion

    #region [- Methods -]

    #region [- MakeUpperCase -]
    private string MakeUpperCase(string title)
    {
        return title.ToUpper();
    }
    #endregion

    #endregion
}
