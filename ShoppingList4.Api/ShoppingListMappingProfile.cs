using AutoMapper;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Models;

namespace ShoppingList4.Api
{
    public class ShoppingListMappingProfile : Profile
    {
        public ShoppingListMappingProfile()
        {
            CreateMap<ShoppingListDto, ShoppingList>();
        }
    }
}
