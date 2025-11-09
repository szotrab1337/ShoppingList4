using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShoppingList4.Api.Application.Entries.Commands.AddEntry;
using ShoppingList4.Api.Application.Entries.Profiles;
using ShoppingList4.Api.Application.Entries.Queries.GetEntryById;
using ShoppingList4.Api.Application.ShoppingLists.Profiles;
using ShoppingList4.Api.Application.Users.Profiles;
using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetEntryByIdQuery).Assembly));

            services.AddScoped(_ => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntryProfile());
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new ShoppingListProfile());
            }).CreateMapper());

            services.AddValidatorsFromAssemblyContaining<AddEntryCommandValidator>()
                .AddFluentValidationAutoValidation();
        }
    }
}