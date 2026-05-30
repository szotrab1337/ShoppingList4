using AutoMapper;
using ShoppingList4.Api.Application.Entries.Commands.AddEntry;
using ShoppingList4.Api.Application.Entries.Commands.EditEntry;
using ShoppingList4.Api.Application.Entries.Dtos;
using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Application.Entries.Profiles
{
    public class EntryProfile : Profile
    {
        public EntryProfile()
        {
            CreateMap<Entry, EntryDto>();

            CreateMap<AddEntryCommand, Entry>()
                .ForMember(x => x.CreatedOn, z => z.MapFrom(_ => DateTime.Now));

            CreateMap<EditEntryCommand, Entry>();
        }
    }
}