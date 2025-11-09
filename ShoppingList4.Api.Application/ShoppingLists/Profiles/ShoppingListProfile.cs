using AutoMapper;
using ShoppingList4.Api.Application.ShoppingLists.Commands.AddShoppingList;
using ShoppingList4.Api.Application.ShoppingLists.Commands.EditShoppingList;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;
using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Application.ShoppingLists.Profiles
{
    public class ShoppingListProfile : Profile
    {
        public ShoppingListProfile()
        {
            CreateMap<ShoppingList, ShoppingListDto>()
                .ForMember(x => x.ItemsCount, opt => opt.MapFrom(z => z.Entries.Count))
                .ForMember(x => x.ItemsBoughtCount, opt => opt.MapFrom(z => z.Entries.Count(x => x.IsBought)));

            CreateMap<AddShoppingListCommand, ShoppingList>();

            CreateMap<EditShoppingListCommand, ShoppingList>();
        }
    }
}